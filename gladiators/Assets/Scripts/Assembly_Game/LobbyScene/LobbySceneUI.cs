using UnityEngine;
using Scoz.Func;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gladiators.Socket;
using Gladiators.Battle;

namespace Gladiators.Main {

    public class LobbySceneUI : BaseUI {
        [SerializeField] LobbyManager Prefab_LobbyManager;
        [SerializeField] CurrencyUI MyCurrencyUI;
        [SerializeField] RouletteController MyRouletteController;

        public static LobbySceneUI Instance { get; private set; }

        DBPlayer myDBPlayer;


        private void Start() {
            Init();
        }
        public override void RefreshText() {
        }
        protected override void SetInstance() {
            Instance = this;
        }

        public override void Init() {
            base.Init();
            UniTask.Void(async () => {
                if (await setDBPlayer() == false) {
                    WriteLog.LogError("玩家尚未登入");
                    return;
                }
                spawnLobbyManager();
                updateCurrencyUI();
                MyRouletteController.Init();
                MyRouletteController.SetRoulette(LobbyManager.Instance.MyRoulette);
            });
        }

        async UniTask<bool> setDBPlayer() {
            if (GamePlayer.Instance.CurSignInState == GamePlayer.SignInState.DontHavePlayerID) return false;
            if (GamePlayer.Instance.CurSignInState == GamePlayer.SignInState.LoginedIn) return true;

#if UNITY_EDITOR
            // 取得遊戲狀態
            var (_, successGetGameState) = await GamePlayer.Instance.UpdateGameState();
            if (successGetGameState == false) {
                WriteLog.LogError("getGameState失敗");
                return false;
            }
            // 登入
            var (_, successSignin) = await GamePlayer.Instance.Signin();
            if (successSignin == false) {
                WriteLog.LogError("signin失敗");
                return false;
            }
            myDBPlayer = GamePlayer.Instance.GetDBData<DBPlayer>();
            await GamePlayer.Instance.ConnectToLobby(); // 開始連線大廳
#else
            return false;
#endif
            return true;
        }

        void updateCurrencyUI() {
            MyCurrencyUI.SetImg(myDBPlayer.Gold);
        }

        void spawnLobbyManager() {
            var go = Instantiate(Prefab_LobbyManager);
            go.GetComponent<LobbyManager>().Init();
        }

    }
}