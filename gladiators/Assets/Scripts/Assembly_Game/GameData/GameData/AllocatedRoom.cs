using Castle.Core.Internal;
using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Gladiators.Socket;
using Gladiators.Socket.Matchgame;
using Scoz.Func;
using Service.Realms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        public DBPlayer EnemyPlayer { get; private set; }
        public DBGladiator EnemyGladiator { get; private set; }

        public enum GameState {
            GameState_NotInGame,// 不在遊戲中
            GameState_UnAuth,//已經從Matchmaker收到配對房間但還沒從Matchgame收到Auth回傳true
            GameState_WaitingPlayers,//已經從Matchgame收到Auth驗證, 等待雙方玩家入場
            GameState_SelectingDivineSkill,//雙方玩家已入場, 選擇神祉技能中
            GameState_CountingDown,//已完成神祉技能選擇, 戰鬥倒數中
            GameState_Fighting,//戰鬥中
            GameState_End,//戰鬥結束
        }
        public GameState CurGameState { get; private set; } = GameState.GameState_NotInGame;
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
        public void SetGameState(GameState _value) {
            CurGameState = _value;
            WriteLog.Log("遊戲狀態切換為:" + _value);
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
        /// Matchgame驗證完成時執行
        /// </summary>
        public void ReceiveAuth() {
            SetGameState(GameState.GameState_WaitingPlayers);
            GameConnector.Instance.SetPlayer("660926d4d0b8e0936ddc6afe");
        }
        /// <summary>
        /// 收到雙方玩家資料後, 將目前狀態設定為GotEnemy並通知BattleScene送Ready
        /// </summary>
        public void ReceiveSetPlayer(PackPlayer _myPlayer, PackPlayer _opponentPlayer) {
            SetGameState(GameState.GameState_SelectingDivineSkill);
            //設定資料
            BattleManager.Instance.CreateTerrainAndChar(_myPlayer, _opponentPlayer).Forget();
            BattleManager.Instance.GotOpponent();
        }
        /// <summary>
        /// 收到準備完成, 收到雙方準備完成 就會進入賄賂階段
        /// </summary>
        public void ReceiveReady(bool[] _readys) {
            var allReady = _readys.All(a => a);
            if (allReady) BattleManager.Instance.GoBribe();
        }

        /// <summary>
        /// 收到賄賂封包, 如果雙方的資料就收到就開始遊戲
        /// </summary>
        public void ReceiveBribe(PackPlayerState[] _playerStates) {
            if (_playerStates == null) return;
            var playerDB = GamePlayer.Instance.GetDBPlayerDoc<DBPlayer>();
            PackPlayerState playerState = null;
            int playerCount = 0;
            for (int i = 0; i < _playerStates.Length; i++) {
                if (_playerStates[i] == null) continue;
                if (playerDB != null && _playerStates[i].DBID == playerDB.ID) playerState = _playerStates[i];
                WriteLog.LogErrorFormat("收到角鬥士{0} 的資料", i);
                WriteLog.WriteObj(_playerStates[i]);
                playerCount++;
            }
            if (playerCount == 2) BattleManager.Instance.StartGame(playerState);
        }

        /// <summary>
        /// 收到戰鬥資訊封包, 存儲封包資料
        /// </summary>
        public void ReceiveBattleState(PackPlayerState[][] _playerStates, double[] GameTime) { //行為處理
            if (_playerStates == null) return;

            BattleManager.Instance.StartBattleTimer();
            if (_playerStates.Length == 1) {
                BattleManager.Instance.setBattleState(_playerStates[0], GameTime[0], BattleManager.BattleStateType.Move);
            } else {
                BattleManager.Instance.setBattleState(_playerStates[0], GameTime[0], BattleManager.BattleStateType.MoveAttack);
                BattleManager.Instance.setBattleState(_playerStates[1], GameTime[1], BattleManager.BattleStateType.Knockback);
            }
        }

        /// <summary>
        /// 收到玩家行為封包, 存儲封包資料
        /// </summary>
        public void ReceivePlayerAction(string _actionType, object _actionContent, PackPlayerState[][] _playerStates, double[] GameTime) { //行為處理
            Debug.Log($"ActionType : {_actionType}");
            switch (_actionType) {
                case "PLAYERACTION_RUSH":
                    if (_playerStates.Length == 1) {
                        BattleManager.Instance.setBattleState_byAction(_playerStates[0], GameTime[0], BattleManager.BattleStateType.Move);
                    } else {
                        BattleManager.Instance.setBattleState_byAction(_playerStates[0], GameTime[0], BattleManager.BattleStateType.MoveAttack);
                        BattleManager.Instance.setBattleState(_playerStates[1], GameTime[1], BattleManager.BattleStateType.Knockback);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
