using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Gladiators.Socket;
using Gladiators.Socket.Lobby;
using LitJson;
using Scoz.Func;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gladiators.Main {
    /// <summary>
    /// 玩家目前所在遊戲房間的資料，CreateRoom後會從Matchmaker回傳取得資料
    /// </summary>
    public class AllocatedLobby {
        public static AllocatedLobby Instance { get; private set; }
        Dictionary<Tuple<string, int>, Action<string>> CMDCallback = new Dictionary<Tuple<string, int>, Action<string>>();
        GameConnector connector;

        // Ping相關宣告
        Accumulator accumulator_Ping;
        private CancellationTokenSource pingCancellationTokenSource;
        private Dictionary<long, long> pingSendDic = new Dictionary<long, long>();
        private const int MaxLatencySamples = 10;
        private Queue<double> latencySamples = new Queue<double>();
        /// <summary>
        /// 網路延遲平均豪秒數
        /// </summary>
        public double Lantency { get; private set; }

        long firstPackServerTimestamp; // 配對開始後第一次收到封包的時間戳
        public long ClientTimeStamp { get { return (long)(Time.realtimeSinceStartup * 1000) + firstPackServerTimestamp; } } // 本地相對時間戳
        public long RenderTimestamp { get { return ClientTimeStamp - (long)Lantency - INTERPOLATION_DELAY_MILISECS; } } // 本地渲染時間戳
        void setFirstPackServerTimestamp(long _time) {
            if (firstPackServerTimestamp == 0) {
                firstPackServerTimestamp = _time - (long)(Time.realtimeSinceStartup * 1000);
            }
        }
        const long INTERPOLATION_DELAY_MILISECS = 100; // 緩衝延遲毫秒(進行插值計算時clientRender時間太接近或超前最新的封包的過程會找不到新封包而無法進行差值，所以要定義一個延遲時間)


        public static void Init() {
            Instance = new AllocatedLobby();
        }

        /// <summary>
        /// 連上Lobby後設定Lobby資料
        /// </summary>
        public void SetLobby(GameConnector _connector) {
            connector = _connector;
            connector.RegisterOnPacketReceived(onReceiveMsg);
            WriteLog.LogColorFormat("設定Lobby資料: {0}", WriteLog.LogType.Connection, DebugUtils.ObjToStr(Instance));
        }
        /// <summary>
        /// 清空配對房間(AllocatedRoom)資訊
        /// </summary>
        public void clearRoom() {
            stopPingLoop();
            connector.Disconnect();
            WriteLog.LogColorFormat("清空大廳(AllocatedLobby)資訊: {0}", WriteLog.LogType.Debug, DebugUtils.ObjToStr(Instance));
        }
        async UniTaskVoid pingLoop() {
            pingCancellationTokenSource = new CancellationTokenSource();
            var token = pingCancellationTokenSource.Token;
            try {
                while (!token.IsCancellationRequested) {
                    sendPing();
                    await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: token);
                }
            } catch (OperationCanceledException) {
                WriteLog.Log("PingLoop cancelled.");
            } catch (Exception ex) {
                WriteLog.LogError($"PingLoop error: {ex.Message}");
            }
        }
        void sendPing() {
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var cmd = new SocketCMD<PING>(new PING());
            connector.Send(cmd);
            pingSendDic[cmd.PackID] = currentTime;
        }
        public void stopPingLoop() {
            pingCancellationTokenSource?.Cancel();
        }
        /// <summary>
        /// 更新網路延遲估算
        /// </summary>
        void updateLatency(double newOneWayLatency) {
            if (latencySamples.Count >= MaxLatencySamples) {
                latencySamples.Dequeue();
            }
            latencySamples.Enqueue(newOneWayLatency);
            double sum = 0.0;
            foreach (var latency in latencySamples) {
                sum += latency;
            }
            Lantency = sum / latencySamples.Count;
            //WriteLog.LogColor($"近{MaxLatencySamples}筆Ping計算出的網路延遲為: {MyMath.Round((float)Lantency, 2)} ms", WriteLog.LogType.Connection);
        }
        void onReceiveMsg(string _msg) {
            try {
                SocketCMD<SocketContent> data = JsonMapper.ToObject<SocketCMD<SocketContent>>(_msg);
                Tuple<string, int> cmdID = new Tuple<string, int>(data.CMD, data.PackID);
                SocketContent.LobbyCMD_TCP cmdType;
                if (!MyEnum.TryParseEnum(data.CMD, out cmdType)) {
                    WriteLog.LogErrorFormat("收到錯誤的命令類型: {0}", cmdType);
                    return;
                } else {
                    // 不輸出的Conn Log加到清單中
                    List<string> dontShowLogCMDs = new List<string>();
                    dontShowLogCMDs.Add(SocketContent.LobbyCMD_TCP.PING_TOCLIENT.ToString());
                    if (!dontShowLogCMDs.Contains(data.CMD)) WriteLog.LogColor($"(TCP)接收: {_msg}", WriteLog.LogType.Connection);
                    if (CMDCallback.TryGetValue(cmdID, out Action<string> _cb)) {
                        CMDCallback.Remove(cmdID);
                        _cb?.Invoke(_msg);
                    }
                    switch (cmdType) {
                        case SocketContent.LobbyCMD_TCP.AUTH_TOCLIENT:
                            var authPacket = LitJson.JsonMapper.ToObject<SocketCMD<AUTH_TOCLIENT>>(_msg);
                            handleAuth(authPacket);
                            break;
                        case SocketContent.LobbyCMD_TCP.PING_TOCLIENT:
                            var pingPacket = LitJson.JsonMapper.ToObject<SocketCMD<PING_TOCLIENT>>(_msg);
                            handlerPing(pingPacket);
                            break;
                        case SocketContent.LobbyCMD_TCP.MATCH_TOCLIENT:
                            var matchPacket = LitJson.JsonMapper.ToObject<SocketCMD<MATCH_TOCLIENT>>(_msg);
                            handlerMatch(matchPacket);
                            break;
                        default:
                            WriteLog.LogErrorFormat("收到尚未定義的命令類型: {0}", cmdType);
                            break;
                    }
                }
            } catch (Exception _e) {
                WriteLog.LogError("Parse收到的封包時出錯 : " + _e.ToString());
            }
        }
        /// <summary>
        /// Lobby驗證完成時執行
        /// </summary>
        void handleAuth(SocketCMD<AUTH_TOCLIENT> _packet) {
            if (_packet.Content == null || !_packet.Content.IsAuth) {
                WriteLog.LogError("Auth錯誤，登入大廳失敗");
                return;
            }
            if (!_packet.Content.IsAuth) {
                WriteLog.LogError("大廳Auth失敗");
                return;
            }
            WriteLog.LogColor("登入(AUTH)大廳成功", WriteLog.LogType.Connection);


            // 開始PingLoop
            setFirstPackServerTimestamp(_packet.Content.Time);
            accumulator_Ping = new Accumulator();
            pingLoop().Forget();

            WriteLog.LogColor("開始大廳 PingLoop", WriteLog.LogType.Connection);
        }
        /// <summary>
        /// 收到Ping封包回傳
        /// </summary>
        void handlerPing(SocketCMD<PING_TOCLIENT> _packet) {
            long pingReceiveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long pingSendTime = pingSendDic[_packet.PackID];
            pingSendDic.Remove(_packet.PackID);
            // 計算Ping送返時間
            long rtt = pingReceiveTime - pingSendTime;
            double oneWayLatency = rtt / 2.0;
            updateLatency(oneWayLatency);
        }
        /// <summary>
        /// 收到Match封包回傳
        /// </summary>
        async void handlerMatch(SocketCMD<MATCH_TOCLIENT> _packet) {
            PopupUI.HideLoading();
            if (_packet == null || _packet.Content == null) {
                WriteLog.LogError("Match錯誤，配對失敗");
                return;
            }
            if (!string.IsNullOrEmpty(_packet.ErrMsg)) {
                WriteLog.LogError($"配房失敗: {_packet.ErrMsg}");
                return;
            }
            PopupUI.ShowLoading("已建立配對，加入房間中...");
            var content = _packet.Content;
            // 連線Matchgame之前先離開本來的Matchgame
            AllocatedRoom.Instance.LeaveRoom();
            await UniTask.Delay(500); // 等待0.5秒再去連server
            // 開始連線Matchgame
            var gameState = GamePlayer.Instance.GetDBData<DBGameState>();
            string serverName = "Matchgame";
            GameConnector.NewConnector(serverName, content.IP, content.Port, () => {
                PopupUI.HideLoading();
                var connector = GameConnector.GetConnector(serverName);
                if (connector != null) {
                    AllocatedRoom.Instance.SetRoom(connector, content.CreaterID, content.DBMatchgameID, content.IP, content.Port);
                    AllocatedRoom.Instance.Auth();
                }
            }, AllocatedRoom.Instance.LeaveRoom).Forget();
        }


        /// <summary>
        /// 送AUTH給Server
        /// </summary>
        public void Auth() {
            var dbPlayer = GamePlayer.Instance.GetDBData<DBPlayer>();
            var cmd = new SocketCMD<AUTH>(new AUTH(dbPlayer.ConnToken));
            connector.Send(cmd);
        }
        public void LeaveRoom() {
            WriteLog.LogColor($"呼叫離開 Lobby Server", WriteLog.LogType.Connection);
            clearRoom();
        }
        /// <summary>
        /// 開始配對
        /// </summary>
        public void Match(string _dbMapID) {
            var cmd = new SocketCMD<MATCH>(new MATCH { DBMapID = _dbMapID });
            connector.Send(cmd);
            PopupUI.ShowLoading("配對中...");
        }

    }
}
