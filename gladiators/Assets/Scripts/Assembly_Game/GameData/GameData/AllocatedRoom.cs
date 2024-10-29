using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Gladiators.Socket;
using Gladiators.Socket.Matchgame;
using Newtonsoft.Json.Linq;
using PlasticPipe.PlasticProtocol.Server.Stubs;
using Scoz.Func;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Gladiators.Main.AllocatedRoom;

namespace Gladiators.Main {
    /// <summary>
    /// 玩家目前所在遊戲房間的資料，CreateRoom後會從Matchmaker回傳取得資料
    /// </summary>
    public class AllocatedRoom {
        public static AllocatedRoom Instance { get; private set; }
        /// <summary>
        /// 創房者ID
        /// </summary>
        public string CreaterID { get; private set; }
        /// <summary>
        /// 房間內的所有PlayerID, 索引就是玩家的座位, 一進房間後就不會更動 PlayerIDs[0]就是在座位0玩家的PlayerID
        /// </summary>
        public string[] PlayerIDs { get; private set; }
        /// <summary>
        /// DB地圖ID
        /// </summary>
        public string DBMapID { get; private set; }
        /// <summary>
        /// DBMatchgame的ID(由Matchmaker產生，格視為[玩家ID]_[累加數字]_[日期時間])
        /// </summary>
        public string DBMatchgameID { get; private set; }
        /// <summary>
        ///  Matchmaker派發Matchgame的IP
        /// </summary>
        public string TcpIP { get; private set; }
        /// <summary>
        ///  Matchmaker派發Matchgame的IP
        /// </summary>
        public string UdpIP { get; private set; }

        /// <summary>
        ///  Matchmaker派發Matchgame的Port
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Matchmaker派發Matchgame的Pod名稱
        /// </summary>
        public string PodName { get; private set; }

        public PackPlayer MyPackPlayer { get; private set; }
        public PackPlayer OpponentPackPlayer { get; private set; }

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



        public enum GameState {
            GameState_NotInGame,// 不在遊戲中
            GameState_UnAuth,//已經從Matchmaker收到配對房間但還沒從Matchgame收到Auth回傳true
            GameState_WaitingPlayersData,//等待收到雙方玩家資料
            GameState_WaitingPlayersReady,//等待雙方玩家進入BattleScene
            GameState_SelectingDivineSkill,//等待選擇神祉技能中
            GameState_Fighting,//戰鬥中
            GameState_End,//戰鬥結束
        }
        public GameState CurGameState { get; private set; } = GameState.GameState_NotInGame;
        public void SetGameState(GameState _value) {
            CurGameState = _value;
            WriteLog.Log("遊戲狀態切換為:" + _value);
        }
        public static void Init() {
            Instance = new AllocatedRoom();
        }

        /// <summary>
        /// 設定被Matchmaker分配到的房間資料，CreateRoom後會從Matchmaker回傳取得此資料
        /// </summary>
        public void SetRoom(string _createID, string[] _playerIDs, string _dbMapID, string _dbMatchgameID, string _ip, int _port, string _podName) {
            CreaterID = _createID;
            PlayerIDs = _playerIDs;
            DBMapID = _dbMapID;
            DBMatchgameID = _dbMatchgameID;
            TcpIP = _ip;
            UdpIP = _ip;
            Port = _port;
            PodName = _podName;
            firstPackServerTimestamp = 0;
            WriteLog.LogColorFormat("設定被Matchmaker分配到的房間資料: {0}", WriteLog.LogType.Debug, DebugUtils.ObjToStr(Instance));
        }
        /// <summary>
        /// 設定被Matchmaker分配到的房間資料，CreateRoom後會從Matchmaker回傳取得此資料
        /// </summary>
        public async UniTask SetRoom_TestvVer(string _createID, string[] _playerIDs, string _dbMapID, string _dbMatchgameID, string _tcpIP, string _udpIP, int _port, string _podName) {
            CreaterID = _createID;
            PlayerIDs = _playerIDs;
            DBMapID = _dbMapID;
            DBMatchgameID = _dbMatchgameID;
            TcpIP = _tcpIP;
            UdpIP = _udpIP;
            Port = _port;
            PodName = _podName;
            WriteLog.LogColorFormat("設定被Matchmaker分配到的房間資料: {0}", WriteLog.LogType.Debug, DebugUtils.ObjToStr(Instance));
        }

        /// <summary>
        /// 清空配對房間(AllocatedRoom)資訊
        /// </summary>
        public void ClearRoom() {
            SetGameState(GameState.GameState_NotInGame);
            CreaterID = null;
            PlayerIDs = null;
            DBMapID = null;
            DBMatchgameID = null;
            Port = 0;
            PodName = null;
            WriteLog.LogColorFormat("清空配對房間(AllocatedRoom)資訊: {0}", WriteLog.LogType.Debug, DebugUtils.ObjToStr(Instance));
        }
        /// <summary>
        /// 送玩家資料給Server
        /// </summary>
        public void SetPlayer(string _dbGladiatorID) {
            var cmd = new SocketCMD<SETPLAYER>(new SETPLAYER(_dbGladiatorID));
            GameConnector.Instance.SendTCP(cmd);
        }
        /// <summary>
        /// 通知Server此玩家已經進入BattleScene
        /// </summary>
        public void SetReady() {
            var cmd = new SocketCMD<SETREADY>(new SETREADY());
            GameConnector.Instance.SendTCP(cmd);
        }

        /// <summary>
        /// 通知Server此玩家已經進入BattleScene
        /// </summary>
        public void SetDivineSkills(int[] _jsonSKillIDs) {
            var cmd = new SocketCMD<SETDIVINESKILL>(new SETDIVINESKILL(_jsonSKillIDs));
            GameConnector.Instance.SendTCP(cmd);
        }
        /// <summary>
        /// 通知Server此玩家已經進入Run
        /// </summary>
        public void SetRun(bool isRun) {
            var cmd = new SocketCMD<PLAYERACTION>(new PLAYERACTION("Action_Rush", new PackAction_Rush(isRun)));
            GameConnector.Instance.SendTCP(cmd);
        }

        /// <summary>
        /// 設定技能
        /// </summary>
        public void ActiveSkill(int _skillID, bool _on) {
            var cmd = new SocketCMD<PLAYERACTION>(new PLAYERACTION("Action_Skill", new PackAction_Skill(_on, _skillID)));
            GameConnector.Instance.SendTCP(cmd);
        }
        /// <summary>
        /// Matchgame驗證完成時執行
        /// </summary>
        public void ReceiveAuth() {
            SetGameState(GameState.GameState_WaitingPlayersData);
            SetPlayer("660926d4d0b8e0936ddc6afe");
        }
        /// <summary>
        /// 收到雙方玩家資料後, 將目前狀態設定為GotEnemy並通知BattleScene送Ready
        /// </summary>
        public void ReceiveSetPlayer(long _serverTimestamp, PackPlayer _myPlayer, PackPlayer _opponentPlayer) {
            MyPackPlayer = _myPlayer;
            OpponentPackPlayer = _opponentPlayer;
            //收到雙方玩家資料
            if (MyPackPlayer != null && OpponentPackPlayer != null && !string.IsNullOrEmpty(MyPackPlayer.DBID) && !string.IsNullOrEmpty(OpponentPackPlayer.DBID)) {
                SetGameState(GameState.GameState_WaitingPlayersReady);
                if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString())
                    PopupUI.CallSceneTransition(MyScene.BattleScene);
                BattleSceneUI.InitPlayerData(MyPackPlayer.MyPackGladiator, OpponentPackPlayer.MyPackGladiator, MyPackPlayer.MyPackGladiator.HandSkillIDs);
                TestTool.Instance.UpdateSkills(MyPackPlayer.MyPackGladiator.HandSkillIDs, 0);
            }

            // 開始PingLoop
            accumulator_Ping = new Accumulator();
            PingLoop().Forget();
            setFirstPackServerTimestamp(_serverTimestamp);
        }

        async UniTaskVoid PingLoop() {
            pingCancellationTokenSource = new CancellationTokenSource();
            var token = pingCancellationTokenSource.Token;
            try {
                while (!token.IsCancellationRequested) {
                    SendPing();
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                }
            } catch (OperationCanceledException) {
                WriteLog.Log("PingLoop cancelled.");
            } catch (Exception ex) {
                WriteLog.LogError($"PingLoop error: {ex.Message}");
            }
        }
        public void StopPingLoop() {
            pingCancellationTokenSource?.Cancel();
        }
        private void SendPing() {
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var cmd = new SocketCMD<PING>(new PING());
            GameConnector.Instance.SendTCP(cmd);
            pingSendDic[cmd.PackID] = currentTime;
        }
        public void ReceivePing(int _packID, PING_TOCLIENT _ping) {
            long pingReceiveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long pingSendTime = pingSendDic[_packID];
            pingSendDic.Remove(_packID);
            // 計算Ping送返時間
            long rtt = pingReceiveTime - pingSendTime;
            double oneWayLatency = rtt / 2.0;
            UpdateLatency(oneWayLatency);
        }
        /// <summary>
        /// 更新網路延遲估算
        /// </summary>
        private void UpdateLatency(double newOneWayLatency) {
            if (latencySamples.Count >= MaxLatencySamples) {
                latencySamples.Dequeue();
            }
            latencySamples.Enqueue(newOneWayLatency);
            double sum = 0.0;
            foreach (var latency in latencySamples) {
                sum += latency;
            }
            Lantency = sum / latencySamples.Count;
            WriteLog.LogColor($"近{MaxLatencySamples}筆Ping計算出的網路延遲為: {MyMath.Round((float)Lantency, 2)} ms", WriteLog.LogType.Connection);
        }

        /// <summary>
        /// 收到準備完成, 收到雙方準備完成 就會進入神祉技能選擇階段
        /// </summary>
        public void ReceiveReady(bool[] _readys) {
            var allReady = _readys.All(a => a);
            if (allReady) {
                SetGameState(GameState.GameState_SelectingDivineSkill);
                BattleManager.Instance.StartSelectDivineSkill();
            }
        }

        /// <summary>
        /// 收到神祉技能選擇封包, 如果雙方的資料都收到就開始遊戲
        /// </summary>
        public void ReceiveDivineSkill(SETDIVINESKILL_TOCLIENT _setDivineSkillToClient) {
            if (_setDivineSkillToClient == null) return;
            //更新介面神祉技能卡牌
            BattleSceneUI.Instance?.SetDivineSkillData(_setDivineSkillToClient.JsonSkillIDs);
            //關閉神祇技能選擇介面(做完演出後才去執行後續動作)
            DivineSelectUI.Instance?.CloseUI(() => {
            });
        }
        /// <summary>
        /// 收到戰鬥階段設定封包
        /// </summary>
        public void ReceiveGameState(GameState_TOCLIENT _gameState) {
            PackGameState gameState;
            if (MyEnum.TryParseEnum(_gameState.State, out gameState)) {
                WriteLog.LogColor($"SERVER狀態: {gameState}", WriteLog.LogType.Connection);
                switch (gameState) {
                    case PackGameState.GAMESTATE_FIGHTING:
                        BattleManager.Instance.StartGame();
                        break;
                }
            }
        }

        /// <summary>
        /// 收到戰鬥資訊封包, 存儲封包資料
        /// </summary>
        public void ReceiveGladiatorStates(long _packID, GLADIATORSTATES_TOCLIENT _gladiatorStats) {
            if (BattleManager.Instance != null) BattleController.Instance.UpdateGladiatorsState(_packID, _gladiatorStats.Time, _gladiatorStats.MyState, _gladiatorStats.OpponentState);
        }
        /// <summary>
        /// 收到肉搏封包, 存儲封包資料
        /// </summary>
        public void ReceiveMelee(MELEE_TOCLIENT _melee) {
            if (BattleManager.Instance == null) return;
            BattleManager.Instance.Melee(_melee);
            BattleController.Instance.UpdateMySkills(_melee.MyHandSkillIDs, 0);
            BattleSceneUI.Instance.SetSkillDatas(_melee.MyHandSkillIDs, 0);
            TestTool.Instance.UpdateSkills(_melee.MyHandSkillIDs, 0);
        }
        /// <summary>
        /// 收到角鬥士血量更新
        /// </summary>
        public void ReceiveGladiatorHP(Hp_TOCLIENT _hpPack) {
            if (BattleManager.Instance != null) {
                BattleSceneUI.Instance.UpdateGladiatorHP(_hpPack.PlayerID, _hpPack.HPChange);
            }
        }

        public void ReceiveSkill(int[] _skills, int _skillOnID) {
            if (BattleController.Instance != null) BattleController.Instance.UpdateMySkills(_skills, _skillOnID);
            BattleSceneUI.Instance.SetSkillDatas(_skills, _skillOnID);
            TestTool.Instance.UpdateSkills(_skills, _skillOnID);
        }

    }
}
