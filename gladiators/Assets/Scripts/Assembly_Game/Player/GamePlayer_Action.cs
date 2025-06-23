using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scoz.Func;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Gladiators.Main {

    public partial class GamePlayer : MyPlayer {
        public enum SignInState {
            DontHavePlayerID, // 沒有本地帳號紀錄
            GotPlayerID, // 本地有帳號紀錄但還沒登入
            LoginedIn,// 已登入
        }
        public SignInState CurSignInState { get; private set; }

        /// <summary>
        /// 取得遊戲狀態並寫入 GamePlayer
        /// </summary>
        public async UniTask<(DBGameState, bool)> UpdateGameState() {
            WriteLog.LogColor("開始 UpdateGameState", WriteLog.LogType.ServerAPI);
            var (dbGameState, result) = await APIManager.GameState();
            if (!result) {
                WriteLog.LogError("APIManager.GameState失敗");
                return (null, false);
            }

            SetDBData(dbGameState);
            WriteLog.LogColor("UpdateGameState 成功", WriteLog.LogType.ServerAPI);
            return (dbGameState, true);
        }



        public async UniTask<(DBPlayer, bool)> Signin() {
            WriteLog.LogColor("開始Signin", WriteLog.LogType.ServerAPI);
            if (string.IsNullOrEmpty(PlayerID) &&
                string.IsNullOrEmpty(MyAuthType) &&
                string.IsNullOrEmpty(DeviceUID)) {
                WriteLog.LogError($"signin傳入資料錯誤: PlayerID: {PlayerID}  MyAuthType: {MyAuthType}   DeviceUID: {DeviceUID}");
                return (null, false);
            }

            string authData = "";
            AuthType authType;
            if (!MyEnum.TryParseEnum(MyAuthType, out authType))
                return (null, false);

            switch (authType) {
                case AuthType.GUEST:
                    authData = DeviceUID;
                    break;
            }

            var (dbPlayer, result) = await APIManager.Signin(
                PlayerID,
                MyAuthType,
                authData,
                Application.platform.ToString(),
                DeviceUID);

            if (!result) {
                WriteLog.LogError("APIManager.Signin失敗");
                return (null, false);
            }

            SigninSetPlayerData(dbPlayer, false);
            updateSignInState();
            WriteLog.LogColor("Signin成功", WriteLog.LogType.ServerAPI);
            return (dbPlayer, true);
        }

        void updateSignInState() {
            SignInState state = SignInState.DontHavePlayerID;
            if (string.IsNullOrEmpty(PlayerID)) {
                state = SignInState.DontHavePlayerID;
            }
            if (!string.IsNullOrEmpty(PlayerID)) {
                if (GetDBData<DBPlayer>() == null) state = SignInState.GotPlayerID;
                else state = SignInState.LoginedIn;
            }
            CurSignInState = state;
            WriteLog.Log($"更新SignInState為: {CurSignInState}");
        }
        public async UniTask<bool> ConnectToLobby() {
            var state = GetDBData<DBGameState>();
            string serverName = "Lobby";
            return await GameConnector.NewConnector(serverName, state.LobbyIP, state.LobbyPort, () => {
                var connector = GameConnector.GetConnector(serverName);
                if (connector != null) {
                    AllocatedLobby.Instance.SetLobby(connector);
                    AllocatedLobby.Instance.Auth();
                }
            }, AllocatedLobby.Instance.Leave);
        }
    }
}