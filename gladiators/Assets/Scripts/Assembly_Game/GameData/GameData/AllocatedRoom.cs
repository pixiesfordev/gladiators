using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Gladiators.Socket;
using Gladiators.Socket.Matchgame;
using Newtonsoft.Json.Linq;
using Scoz.Func;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

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
        public async UniTask SetRoom(string _createID, string[] _playerIDs, string _dbMapID, string _dbMatchgameID, string _ip, int _port, string _podName) {
            CreaterID = _createID;
            PlayerIDs = _playerIDs;
            DBMapID = _dbMapID;
            DBMatchgameID = _dbMatchgameID;
            TcpIP = _ip;
            UdpIP = _ip;
            Port = _port;
            PodName = _podName;
            WriteLog.LogColorFormat("設定被Matchmaker分配到的房間資料: {0}", WriteLog.LogType.Debug, DebugUtils.ObjToStr(Instance));

            var dbPlayer = GamePlayer.Instance.GetDBPlayerDoc<DBPlayer>();
            if (dbPlayer == null) return;
            await dbPlayer.SetInMatchgameID(DBMatchgameID);
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

            var dbPlayer = GamePlayer.Instance.GetDBPlayerDoc<DBPlayer>();
            if (dbPlayer == null) return;
            await dbPlayer.SetInMatchgameID(DBMatchgameID);
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

            var dbPlayer = GamePlayer.Instance.GetDBPlayerDoc<DBPlayer>();
            if (dbPlayer == null) return;
            dbPlayer.SetInMatchgameID(null).Forget();
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
        /// 通知Server此玩家已經進入BattleScene
        /// </summary>
        public void BattleState() {
            var cmd = new SocketCMD<BATTLESTATE>();
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
        /// Matchgame驗證完成時執行
        /// </summary>
        public void ReceiveAuth() {
            SetGameState(GameState.GameState_WaitingPlayersData);
            SetPlayer("660926d4d0b8e0936ddc6afe");
        }
        /// <summary>
        /// 收到雙方玩家資料後, 將目前狀態設定為GotEnemy並通知BattleScene送Ready
        /// </summary>
        public void ReceiveSetPlayer(PackPlayer _myPlayer, PackPlayer _opponentPlayer) {
            MyPackPlayer = _myPlayer;
            OpponentPackPlayer = _opponentPlayer;
            //收到雙方玩家資料
            if (MyPackPlayer != null && OpponentPackPlayer != null && !string.IsNullOrEmpty(MyPackPlayer.DBID) && !string.IsNullOrEmpty(OpponentPackPlayer.DBID)) {
                SetGameState(GameState.GameState_WaitingPlayersReady);
                if (SceneManager.GetActiveScene().name != MyScene.BattleScene.ToString())//跳轉到BattleScene
                    PopupUI.CallSceneTransition(MyScene.BattleScene);
            }
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
        public void ReceiveDivineSkill(PackPlayerState _myPlayerState, PackPlayerState _opponentPlayerState) {
            if (_myPlayerState == null || _myPlayerState.DBID == null) return;
            //更新介面神祉技能卡牌
            BattleSceneUI.Instance?.SetDivineSkillData(_myPlayerState.DivineSkills);
            //關閉神祇技能選擇介面(做完演出後才去執行後續動作)
            DivineSelectUI.Instance?.CloseUI(() => {
            });
        }
        /// <summary>
        /// 收到戰鬥開始封包
        /// </summary>
        public void ReceiveStartFighting() {
            BattleManager.Instance.StartGame();
        }

        /// <summary>
        /// 收到戰鬥資訊封包, 存儲封包資料
        /// </summary>
        public void ReceiveBattleState(BATTLESTATE_TOCLIENT _battleState) {
            if (BattleManager.Instance != null) BattleManager.Instance.SetBattleState(_battleState);
        }
        /// <summary>
        /// 收到肉搏封包, 存儲封包資料
        /// </summary>
        public void ReceiveMelee(MELEE_TOCLIENT _melee) {
            if (BattleManager.Instance != null) BattleManager.Instance.Melee(_melee);
        }

        public void ReceivePing() { //回送Server(如果X秒Server都沒收到Ping會認為玩家斷線了)
            var cmd = new SocketCMD<PING>(new PING());
            GameConnector.Instance.SendTCP(cmd);
        }

        public void ReceiveRush(string _playerID, bool _rush) {
            if (BattleModelController.Instance != null) BattleModelController.Instance.Run(_playerID, _rush);
        }

        public void ReceiveSkill(string _playerID, int _skillID, bool _on) {
            if (BattleModelController.Instance != null) BattleModelController.Instance.Skill(_playerID, _skillID, _on);
        }

    }
}
