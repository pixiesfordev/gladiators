using Cysharp.Threading.Tasks;
using Gladiators.Main;
using Scoz.Func;
using System;
using UnityEngine;

namespace Gladiators.Socket {
    public partial class GameConnector : MonoBehaviour {


        bool IsConnectomg = false;
        Action OnConnToMatchgame;//連線Matchgame callback
        Action OnJoinGameFail;//連線Matchgame失敗callback
        Action OnMatchgameDisconnected;//與Matchgame 斷線callback

        /// <summary>
        /// 確認DBMatchgame表被建立後會跳BattleScene並開始跑ConnToMatchgame開始連線到Matchgame
        /// </summary>
        public void ConnToMatchgame(Action _onConnected, Action _onJoinGameFail, Action _onDisconnected) {
            if (AllocatedRoom.Instance.CurGameState != AllocatedRoom.GameState.GameState_NotInGame) return;
            AllocatedRoom.Instance.SetGameState(AllocatedRoom.GameState.GameState_UnAuth);
            if (IsConnectomg) return;
            OnConnToMatchgame = _onConnected;
            OnJoinGameFail = _onJoinGameFail;
            OnMatchgameDisconnected = _onDisconnected;

            WriteLog.LogColor("DBMatchgame已建立好, 開始連線到Matchgame", WriteLog.LogType.Connection);

            UniTask.Void(async () => {
                // 這裡要補從Lobby收到的Matchgame資料
                //var dbMatchgame = await APIManager.GameState()
                //if (dbMatchgame == null) {
                //    WriteLog.LogError("JoinMatchgame失敗，dbMatchgame is null");
                //    OnJoinGameFail?.Invoke();
                //    return;
                //}
                IsConnectomg = true;
                joinMatchgame(); //開始連線到Matchgame                                          
            });
        }
        /// <summary>
        /// 個人測試模式(不使用Agones服務, 不會透過Matchmaker分配房間再把ip回傳給client, 而是直接讓client去連資料庫matchgame的ip)
        /// </summary>
        public void ConnectToMatchgameTestVer(Action _onConnnectedAC, Action _onJoinGameFail, Action _onDisconnected) {
            // 建立房間成功
            WriteLog.LogColor("個人測試模式連線Matchgame: ", WriteLog.LogType.Connection);
            var gameState = GamePlayer.Instance.GetDBData<DBGameState>();
            //設定玩家目前所在遊戲房間的資料
            UniTask.Void(async () => {
                await AllocatedRoom.Instance.SetRoom_TestvVer("System", new string[2], gameState.MatchgameTestverMapID, gameState.MatchgameTestverRoomName, gameState.MatchgameTestverTcpIp, "", gameState.MatchgameTestverPort, "");
                ConnToMatchgame(_onConnnectedAC, _onJoinGameFail, _onDisconnected);
            });
        }
        /// <summary>
        /// 加入Matchmage
        /// </summary>
        void joinMatchgame() {
            var dbPlayer = GamePlayer.Instance.GetDBData<DBPlayer>();
            if (dbPlayer == null) return;
            Socket.JoinMatchgame(GameDisconnected, dbPlayer.ConnToken, AllocatedRoom.Instance.TcpIP, AllocatedRoom.Instance.UdpIP, AllocatedRoom.Instance.Port);
        }

        void JoinGameSuccess() {
            IsConnectomg = false;
            OnConnToMatchgame?.Invoke();
            WriteLog.LogColor("JoinMatchgame success!", WriteLog.LogType.Connection);
        }

        void JoinGameFailed(Exception ex) {
            IsConnectomg = false;
            Debug.Log("JoinMatghgame failed: " + ex);
            OnJoinGameFail?.Invoke();
        }

        void GameDisconnected() {
            IsConnectomg = false;
            OnMatchgameDisconnected?.Invoke();
        }

        public void SendTCP<T>(SocketCMD<T> cmd) where T : SocketContent {
            Socket.TCPSend(cmd);
        }
        public void SendUDP<T>(SocketCMD<T> cmd) where T : SocketContent {
            Socket.UDPSend(cmd);
        }
    }
}
