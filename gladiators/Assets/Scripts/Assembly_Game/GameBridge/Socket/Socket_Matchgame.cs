using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Gladiators.Main;
using Gladiators.Socket.Matchgame;
using LitJson;
using MongoDB.Bson;
using Newtonsoft.Json;
using Scoz.Func;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Gladiators.Socket.SocketContent;

namespace Gladiators.Socket {
    public partial class GladiatorsSocket {
        TcpClient TCP_MatchgameClient;
        UdpSocket UDP_MatchgameClient;
        ServerTimeSyncer TimeSyncer;
        string UDP_MatchgameConnToken;// 連Matchgame需要的Token，由AUTH_REPLY時取得

        Dictionary<Tuple<string, int>, Action<string>> CMDCallback = new Dictionary<Tuple<string, int>, Action<string>>();

        readonly Subject<Unit> JoinRoomSubject = new Subject<Unit>();
        public IObservable<Unit> JoinRoomObservable => JoinRoomSubject;

        public int GetMatchgamePing() {
            if (TimeSyncer)
                return Mathf.RoundToInt(TimeSyncer.GetLantency() * 1000);
            return 0;
        }
        void RegisterMatchgameCommandCB(string _command, int _packetID, Action<string> _ac) {
            var cmdID = new Tuple<string, int>(_command, _packetID);
            if (CMDCallback.ContainsKey(cmdID)) {
                WriteLog.LogError("Command remain here should not happen.");
                CMDCallback.Remove(cmdID);
            }
            CMDCallback.Add(cmdID, _ac);
        }
        public void JoinMatchgame(Action _onDisconnected, string _realmToken, string _tcpIP, string _udpIP, int _port) {
            CreateClientObject(ref TCP_MatchgameClient, _tcpIP, _port, "JoinMatchgame", "TCP_MatchgameClient");
            CreateClientObject(ref UDP_MatchgameClient, _udpIP, _port, "JoinMatchgame", "UDP_MatchgameClient");
            TCP_MatchgameClient.OnReceiveMsg += OnRecieveMatchgameTCPMsg;

            TCP_MatchgameClient.StartConnect((bool connected) => OnMatchgameTCPConnect(connected, _realmToken));
            TCP_MatchgameClient.RegistOnDisconnect(_onDisconnected);
        }

        public void OnMatchgameUDPDisconnect() {
            UDP_MatchgameClient.OnReceiveMsg -= OnRecieveMatchgameUDPMsg;
            if (UDP_MatchgameClient != null) {
                this.MatchgameUDPEndWithDisconnection();
                return;
            }

            UDP_MatchgameClient.Close();
            UDP_MatchgameClient = new GameObject("GameUdpSocket").AddComponent<UdpSocket>();
            UniTask.Void(async () => {
                try {
                    var dbMatchgame = await GamePlayer.Instance.GetMatchGame();
                    if (dbMatchgame == null) {
                        WriteLog.LogError("OnMatchgameUDPDisconnect時重連失敗，dbMatchgame is null");
                        this.MatchgameUDPEndWithDisconnection();
                        return;
                    } else UDP_MatchgameClient.Init(dbMatchgame.IP, dbMatchgame.Port ?? 0);
                } catch (Exception _e) {
                    WriteLog.LogError("OnMatchgameUDPDisconnect時嘗試重連失敗: " + _e);
                    OnMatchgameUDPDisconnect();
                    return;
                }
                UDP_MatchgameClient.StartConnect(UDP_MatchgameConnToken, (bool connected) => {
                    WriteLog.LogColor("OnMatchgameUDPDisconnect後重連結果 :" + connected, WriteLog.LogType.Connection);
                    if (connected) {
                        UDP_MatchgameClient.OnReceiveMsg += OnRecieveMatchgameUDPMsg;
                    } else {
                        this.MatchgameUDPEndWithDisconnection();
                        return;
                    }
                });
                UDP_MatchgameClient.RegistOnDisconnect(OnMatchgameUDPDisconnect);
            });
        }
        public void MatchgameUDPEndWithDisconnection() {
            WriteLog.LogColor("MatchgameUDPEndWithDisconnection", WriteLog.LogType.Connection);
        }
        public int UDPSend<T>(SocketCMD<T> cmd) where T : SocketContent {
            cmd.SetConnToken(UDP_MatchgameConnToken);//設定封包ConnToken
            if (string.IsNullOrEmpty(cmd.ConnToken)) {
                WriteLog.LogError("UDP封包的ConnToken不可為空");
                return -1;
            }
            return UDP_MatchgameClient.Send(cmd);
        }
        public int TCPSend<T>(SocketCMD<T> cmd) where T : SocketContent {
            return TCP_MatchgameClient.Send(cmd);
        }


        private void OnMatchgameTCPConnect(bool connected, string realmToken) {
            WriteLog.LogColor($"TCP_MatchgameClient connection: {connected}", WriteLog.LogType.Connection);
            if (!connected) {
                JoinRoomSubject?.OnError(new Exception("client connection error"));
                return;
            }
            // Close Matchmaker client
            if (TCP_MatchmakerClient != null) {
                TCP_MatchmakerClient.OnReceiveMsg -= OnRecieveMatchmakerTCPMsg;
                TCP_MatchmakerClient.Close();
                WriteLog.LogColor($"JoinMatchgame成功後 TCP_MatchmakerClient不需要了關閉 {TCP_MatchmakerClient}", WriteLog.LogType.Connection);
            }
            SocketCMD<AUTH> cmd = new SocketCMD<AUTH>(new AUTH(realmToken));

            int id = TCP_MatchgameClient.Send(cmd);
            if (id < 0) {
                JoinRoomSubject?.OnError(new Exception("packet id < 0"));
                return;
            }
            RegisterMatchgameCommandCB(SocketContent.MatchgameCMD_TCP.AUTH_TOCLIENT.ToString(), id, TCPClientCmdCallback);
        }

        private void TCPClientCmdCallback(string msg) {
            SocketCMD<AUTH_TOCLIENT> packet = LitJson.JsonMapper.ToObject<SocketCMD<AUTH_TOCLIENT>>(msg);
            if (packet.Content.IsAuth) {
                try {
                    //目前遊戲沒使用UDP, 有需要再開
                    //ConnectUDPMatchgame(packet.Content.ConnToken, packet.Content.Index);

                    JoinRoomSubject?.OnNext(Unit.Default);
                } catch (Exception e) {
                    WriteLog.LogError($"UDP error: " + e);
                    JoinRoomSubject?.OnError(e);
                }
                if (TimeSyncer == null)
                    TimeSyncer = new GameObject("TimeSyncer").AddComponent<ServerTimeSyncer>();
                TimeSyncer.StartCountTime();
            } else {
                JoinRoomSubject?.OnError(new Exception("auth is invalid"));
            }
        }

        private void ConnectUDPMatchgame(string connToken, int index) {
            //取得Matchgame Auth的回傳結果 UDP socket的ConnToken與遊戲房間的座位索引
            WriteLog.LogColor($"Matchgame auth success! UDP_MatchgameConnToken: {UDP_MatchgameConnToken}", WriteLog.LogType.Connection);
            UDP_MatchgameConnToken = connToken;

            //取得ConnToken後就能進行UDP socket連線
            UDP_MatchgameClient.StartConnect(UDP_MatchgameConnToken, (bool connected) => {
                WriteLog.LogColor($"UDP Is connected: {connected}", WriteLog.LogType.Connection);
                if (connected) {
                    UDP_MatchgameClient.OnReceiveMsg += OnRecieveMatchgameUDPMsg;
                } else {
                    WriteLog.LogError("UDP連線失敗");
                    OnMatchgameUDPDisconnect();
                }
                UDP_MatchgameClient.RegistOnDisconnect(OnMatchgameUDPDisconnect);
            });

        }

        public void OnRecieveMatchgameUDPMsg(string _msg) {
            try {
                SocketCMD<SocketContent> data = JsonMapper.ToObject<SocketCMD<SocketContent>>(_msg);
                SocketContent.MatchgameCMD_UDP cmdType;
                if (!MyEnum.TryParseEnum(data.CMD, out cmdType)) {
                    WriteLog.LogErrorFormat("收到錯誤的命令類型: {0}", cmdType);
                    return;
                }
                WriteLog.LogColorFormat("(UDP)接收: {0}", WriteLog.LogType.Connection, _msg);
                switch (cmdType) {
                    //case SocketContent.MatchgameCMD_UDP.UPDATEGAME_TOCLIENT:
                    //    var updateGamePacket = LitJson.JsonMapper.ToObject<SocketCMD<UPDATEGAME_TOCLIENT>>(_msg);
                    //    HandleUpdateGame(updateGamePacket);
                    //    break;
                    default:
                        WriteLog.LogErrorFormat("收到尚未定義的命令類型: {0}", cmdType);
                        break;
                }
            } catch (Exception e) {
                WriteLog.LogError("Parse UDP Message with Error : " + e.ToString());
            }
        }

        private void OnRecieveMatchgameTCPMsg(string _msg) {
            try {

                //WriteLog.LogColorFormat("(TCP)接收: {0}", WriteLog.LogType.Connection, _msg);
                SocketCMD<SocketContent> data = JsonMapper.ToObject<SocketCMD<SocketContent>>(_msg);
                Tuple<string, int> cmdID = new Tuple<string, int>(data.CMD, data.PackID);
                SocketContent.MatchgameCMD_TCP cmdType;
                if (!MyEnum.TryParseEnum(data.CMD, out cmdType)) {
                    WriteLog.LogErrorFormat("收到錯誤的命令類型: {0}", cmdType);
                    return;
                } else {
                    // 不輸出的Conn Log加到清單中
                    List<string> dontShowLogCMDs = new List<string>();
                    dontShowLogCMDs.Add(MatchgameCMD_TCP.PING_TOCLIENT.ToString());
                    dontShowLogCMDs.Add(MatchgameCMD_TCP.BATTLESTATE_TOCLIENT.ToString());
                    if (!dontShowLogCMDs.Contains(data.CMD)) WriteLog.LogColorFormat("(TCP)接收: {0}", WriteLog.LogType.Connection, _msg);
                }
                if (CMDCallback.TryGetValue(cmdID, out Action<string> _cb)) {
                    CMDCallback.Remove(cmdID);
                    _cb?.Invoke(_msg);
                }
                switch (cmdType) {
                    case SocketContent.MatchgameCMD_TCP.AUTH_TOCLIENT:
                        var authPacket = LitJson.JsonMapper.ToObject<SocketCMD<AUTH_TOCLIENT>>(_msg);
                        HandleAuth(authPacket);
                        break;
                    case SocketContent.MatchgameCMD_TCP.SETPLAYER_TOCLIENT:
                        var setPlayerPacket = LitJson.JsonMapper.ToObject<SocketCMD<SETPLAYER_TOCLIENT>>(_msg);
                        HandleSetPlayer(setPlayerPacket);
                        break;
                    case SocketContent.MatchgameCMD_TCP.SETREADY_TOCLIENT:
                        var readyPacket = LitJson.JsonMapper.ToObject<SocketCMD<SETREADY_TOCLIENT>>(_msg);
                        HandleReady(readyPacket);
                        break;
                    case SocketContent.MatchgameCMD_TCP.SETDIVINESKILL_TOCLIENT:
                        var bribePacket = LitJson.JsonMapper.ToObject<SocketCMD<SETDIVINESKILL_TOCLIENT>>(_msg);
                        HandleSetDivineSkill(bribePacket);
                        break;
                    case SocketContent.MatchgameCMD_TCP.STARTFIGHTING_TOCLIENT:
                        var startFightingPacket = LitJson.JsonMapper.ToObject<SocketCMD<STARTFIGHTING_TOCLIENT>>(_msg);
                        HandleStartFighting(startFightingPacket);
                        break;
                    case SocketContent.MatchgameCMD_TCP.BATTLESTATE_TOCLIENT:
                        var battlePacket = LitJson.JsonMapper.ToObject<SocketCMD<BATTLESTATE_TOCLIENT>>(_msg);
                        HandlerBattleState(battlePacket);
                        break;
                    case SocketContent.MatchgameCMD_TCP.PLAYERACTION_TOCLIENT:
                        //收到封包要先進行PLAYERACTION_TOCLIENT反序列化之後再根據ActionType類型來使用泛型解析成正確的ActionContent
                        var actionBasePacket = JsonMapper.ToObject<SocketCMD<PLAYERACTION_TOCLIENT<JsonData>>>(_msg);
                        var actionContentJson = actionBasePacket.Content.ActionContent.ToJson();
                        HandlerPlayerAction(actionBasePacket.Content.ActionType, actionBasePacket.Content.PlayerDBID, actionContentJson);
                        break;
                    case SocketContent.MatchgameCMD_TCP.PING_TOCLIENT:
                        var pingPacket = LitJson.JsonMapper.ToObject<SocketCMD<PING_TOCLIENT>>(_msg);
                        HandlerPing(pingPacket);
                        break;
                    case SocketContent.MatchgameCMD_TCP.MELEE_TOCLIENT:
                        var meleePacket = LitJson.JsonMapper.ToObject<SocketCMD<MELEE_TOCLIENT>>(_msg);
                        HandlerMelee(meleePacket);
                        break;
                    default:
                        WriteLog.LogErrorFormat("收到尚未定義的命令類型: {0}", cmdType);
                        break;
                }
            } catch (Exception e) {
                WriteLog.LogError("Parse收到的封包時出錯 : " + e.ToString());
                if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString()) {
                    WriteLog.LogErrorFormat("不在{0}就釋放資源: ", MyScene.BattleScene, e.ToString());
                    Release();
                }
            }
        }
        void HandleAuth(SocketCMD<AUTH_TOCLIENT> _packet) {
            //if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString()) return;
            if (_packet.Content == null || !_packet.Content.IsAuth) {
                WriteLog.LogError("Auth錯誤 遊戲無法開始");
                return;
            }
            AllocatedRoom.Instance.ReceiveAuth();
        }
        void HandleSetPlayer(SocketCMD<SETPLAYER_TOCLIENT> _packet) {
            AllocatedRoom.Instance.ReceiveSetPlayer(_packet.Content.MyPackPlayer, _packet.Content.OpponentPackPlayer);
        }
        void HandleReady(SocketCMD<SETREADY_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString()) return;
            AllocatedRoom.Instance.ReceiveReady(_packet.Content.PlayerReadies);
        }
        void HandleSetDivineSkill(SocketCMD<SETDIVINESKILL_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString()) return;
            AllocatedRoom.Instance.ReceiveDivineSkill(_packet.Content.MyPlayerState, _packet.Content.OpponentPlayerState);
        }
        void HandleStartFighting(SocketCMD<STARTFIGHTING_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString()) return;
            AllocatedRoom.Instance.ReceiveStartFighting();
        }
        void HandlerBattleState(SocketCMD<BATTLESTATE_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString()) return;
            AllocatedRoom.Instance.ReceiveBattleState(_packet.Content);
        }
        void HandlerPlayerAction(string _actionType, string _playerID, string _jsonStr) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString()) return;
            switch (_actionType) {
                case "Action_Rush":
                    var rush = JsonMapper.ToObject<PackAction_Rush_ToClient>(_jsonStr);
                    AllocatedRoom.Instance.ReceiveRush(_playerID, rush.On);
                    break;
                default:
                    Debug.LogError($"未知的 ActionType: {_actionType}");
                    break;
            }
        }
        void HandlerPing(SocketCMD<PING_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString()) return;
            AllocatedRoom.Instance.ReceivePing();
        }
        void HandlerMelee(SocketCMD<MELEE_TOCLIENT> _packet) {
            if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString()) return;
            AllocatedRoom.Instance.ReceiveMelee(_packet.Content);
        }
    }
}

