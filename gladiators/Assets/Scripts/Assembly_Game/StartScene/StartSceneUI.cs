using UnityEngine;
using Scoz.Func;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using static Gladiators.Main.AllocatedRoom;
using Codice.Client.Common;

namespace Gladiators.Main {
    public class StartSceneUI : BaseUI {
        [SerializeField] Toggle TermsOfUseToggle;
        [SerializeField] Text VersionText;
        [SerializeField] Button Btn_Continue, Btn_Logout, Btn_DeleteAC;
        [SerializeField] Button Btn_GuestLogin, Btn_GoogleLogin, Btn_AppleLogin;

        /// <summary>
        /// 是否第一次執行遊戲，第一次執行遊戲後會自動進大廳，之後透過從大廳的設定中點回到主介面就不會又自動進大廳了
        /// </summary>
        public static bool FirstTimeLaunchGame { get; private set; } = true;

        public static StartSceneUI Instance { get; private set; }

        protected override void SetInstance() {
            Instance = this;
        }
        public override void Init() {
            base.Init();
        }
        public override void RefreshText() {
        }

        private async void Start() {
            Init();
            // Apple登入要打開
#if UNITY_IOS
            Btn_AppleLogin.gameObject.SetActive(true);
#else
            Btn_AppleLogin.gameObject.SetActive(false);
#endif

            UniTask.Void(async () => {
                showInfo();//顯示資訊
                showBtnsByState(false);
                // 取得遊戲狀態
                var (_, successGetGameState) = await GamePlayer.Instance.UpdateGameState();
                if (successGetGameState == false) {
                    WriteLog.LogError("getGameState失敗");
                    return;
                }

                if (GamePlayer.Instance.CurSignInState == GamePlayer.SignInState.GotPlayerID) {
                    // 登入
                    var (_, successSignin) = await GamePlayer.Instance.Signin();
                    if (successSignin == false) {
                        WriteLog.LogError("signin失敗");
                        return;
                    }
                    showInfo();//顯示資訊
                    bool successConnectToLobby = await GamePlayer.Instance.ConnectToLobby(); // 開始連線大廳
                    if (successConnectToLobby == false) {
                        WriteLog.LogError("successConnectToLobby失敗");
                        return;
                    }
                }

#if UNITY_EDITOR
                showBtnsByState(true);
#else
            goLobby(); 
#endif

            });
        }






        void showBtnsByState(bool _show) {
            if (_show == false) {
                Btn_Continue.gameObject.SetActive(false);
                Btn_DeleteAC.gameObject.SetActive(false);
                Btn_GuestLogin.gameObject.SetActive(false);
                Btn_AppleLogin.gameObject.SetActive(false);
                Btn_GoogleLogin.gameObject.SetActive(false);
                Btn_Logout.gameObject.SetActive(false);
                return;
            }
            switch (GamePlayer.Instance.CurSignInState) {
                case GamePlayer.SignInState.DontHavePlayerID:
                    Btn_Continue.gameObject.SetActive(false);
                    Btn_DeleteAC.gameObject.SetActive(false);
                    Btn_GuestLogin.gameObject.SetActive(true);
                    Btn_AppleLogin.gameObject.SetActive(true);
                    Btn_GoogleLogin.gameObject.SetActive(true);
                    Btn_Logout.gameObject.SetActive(false);
                    break;
                case GamePlayer.SignInState.LoginedIn:
                    Btn_Continue.gameObject.SetActive(true);
                    Btn_DeleteAC.gameObject.SetActive(true);
                    Btn_GuestLogin.gameObject.SetActive(false);
                    Btn_AppleLogin.gameObject.SetActive(false);
                    Btn_GoogleLogin.gameObject.SetActive(false);
                    Btn_Logout.gameObject.SetActive(true);
                    break;
            }
        }
        /// <summary>
        /// 1. (玩家尚未登入) 顯示版本
        /// 2. (玩家登入) 顯示版本+玩家ID
        /// </summary>
        public void showInfo() {
            string playerID = "尚未登入";
            var dbPlayer = GamePlayer.Instance.GetDBData<DBPlayer>();
            if (dbPlayer != null) playerID = dbPlayer.ID;
            VersionText.text = $"版本: {GameManager.GameFullVersion} 玩家: {playerID}";
        }
        /// <summary>
        /// 登入按鈕按下
        /// </summary>
        public void OnSignupClick(string _authTypeStr) {

            if (!TermsOfUseToggle.isOn) {//沒有勾選同意使用者條款的話會跳彈窗並返回
                PopupUI.ShowClickCancel(JsonString.GetUIString("NeedToAgreeTersOfUse"), null);
                return;
            }

            //錯誤的登入類型就返回
            AuthType authType = AuthType.GUEST;
            if (!MyEnum.TryParseEnum(_authTypeStr, out authType)) { WriteLog.LogError("錯誤的登入類型: " + _authTypeStr); return; }
            var dbPlayer = GamePlayer.Instance.GetDBData<DBPlayer>();
            if (dbPlayer != null) {
                WriteLog.LogError("本來就有登入，代表是UI顯示錯誤(登入中的玩家不該點的到訪客註冊)");
                return;
            }
            signup(authType).Forget();
        }
        /// <summary>
        /// 初始化玩家資料
        /// </summary>
        async UniTask signup(AuthType _authType) {
            WriteLog.LogColor($"{_authType} 註冊玩家資料", WriteLog.LogType.Player);
            string deviceUID = DeviceManager.GenerateDeviceUID();
            switch (_authType) {
                case AuthType.GUEST:
                    var (dbPlayer, result) = await APIManager.Signup(_authType.ToString(), deviceUID, Application.platform.ToString(), deviceUID);
                    if (result == false) {
                        WriteLog.LogError("GUEST登入失敗");
                        return;
                    }
                    GamePlayer.Instance.SigninSetPlayerData(dbPlayer, true);
                    break;
                default:
                    WriteLog.LogError($"尚未實作此AuthType: {_authType}");
                    return;
            }
        }

        public void OnContinueClick() {
            goLobby();
        }
        public void goLobby() {
            showBtnsByState(false);
            //繞過正式流程
            FirstTimeLaunchGame = false;
            PopupUI.InitSceneTransitionProgress(0);
            PopupUI.CallSceneTransition(MyScene.LobbyScene);
            return;

            /// 根據是否能進行遊戲來執行各種狀態
            /// 1. 判斷玩家版本，若版本低於最低遊戲版本則會跳強制更新
            /// 2. 判斷玩家版本，若版本低於目前遊戲版本則會跳更新建議
            /// 3. 判斷Maintain是否為true，若為true則不在MaintainExemptPlayerUIDs中的玩家都會跳維護中
            /// 4. 判斷該玩家是否被Ban，不是才能進遊戲
            GameStateManager.Instance.CheckCanPlayGame(() => {
                FirstTimeLaunchGame = false;
                PopupUI.InitSceneTransitionProgress(0, "LobbyUILoaded");
                PopupUI.CallSceneTransition(MyScene.LobbyScene);
            });
        }
        public void OnLogoutClick() {
        }
        void logout() {
        }
        public void OnDeleteACClick() {
        }
        void deleteAC() {
            //UnlinkAllPlatfromsAndLogout
        }
        /// <summary>
        /// 遞迴解綁所有平台並登出帳戶
        /// </summary>
        void unlinkAllPlatfromsAndLogout() {
            //if (FirebaseManager.IsLinkingAnyThirdPart) {
            //    if (FirebaseManager.IsLinkingThrdPart(ThirdPartLink.Facebook)) {//還沒綁定就進行綁定
            //        PopupUI.ShowLoading(StringData.GetUIString("UnLinkingFB"));
            //        FirebaseManager.UnlinkFacebook(result => {
            //            PopupUI.HideLoading();
            //            UnlinkAllPlatfromsAndLogout();
            //        });
            //        return;
            //    }
            //    if (FirebaseManager.IsLinkingThrdPart(ThirdPartLink.Apple)) {//還沒綁定就進行綁定
            //        PopupUI.ShowLoading(StringData.GetUIString("UnLinkingApple"));
            //        FirebaseManager.UnlinkApple(result => {
            //            PopupUI.HideLoading();
            //            UnlinkAllPlatfromsAndLogout();
            //        });
            //        return;
            //    }
            //    if (FirebaseManager.IsLinkingThrdPart(ThirdPartLink.Google)) {//還沒綁定就進行綁定
            //        PopupUI.ShowLoading(StringData.GetUIString("UnLinkingGoogle"));
            //        FirebaseManager.UnlinkGoogle(result => {
            //            PopupUI.HideLoading();
            //            UnlinkAllPlatfromsAndLogout();
            //        });
            //        return;
            //    }
            //} else {
            //    PopupUI.ShowLoading(StringData.GetUIString("Loading"));
            //    StartCoroutine(FirebaseManager.Logout(() => {//登出
            //        PopupUI.HideLoading();
            //        ShowUI(Condietion.BackFromLobby_ShowLoginBtn);
            //        PlayerPrefs.DeleteAll();//清除所有本機資料
            //    }));
            //}
        }
        void fbAuth() {
            //PopupUI.ShowLoading(string.Format("Loading"));
            //FirebaseManager.SignInWithFacebook(result => {
            //    PopupUI.HideLoading();
            //    if (!result) {
            //        PopupUI.ShowClickCancel(StringData.GetUIString("SigninFail"), null);
            //    } else {
            //        OnThirdPartAuthFinished(AuthType.Facebook);
            //    }
            //});
        }

        void appleAuth() {
            //PopupUI.ShowLoading(string.Format("Loading"));
            //FirebaseManager.SignInWithApple(result => {
            //    PopupUI.HideLoading();
            //    if (!result) {
            //        PopupUI.ShowClickCancel(StringData.GetUIString("SigninFail"), null);
            //    } else {
            //        OnThirdPartAuthFinished(AuthType.Apple);
            //    }
            //});
        }
        void googleAuth() {
            //PopupUI.ShowLoading(string.Format("Loading"));
            //FirebaseManager.SignInWithGoogle(result => {
            //    PopupUI.HideLoading();
            //    if (!result) {
            //        PopupUI.ShowClickCancel(StringData.GetUIString("SigninFail"), null);
            //    } else {
            //        OnThirdPartAuthFinished(AuthType.Google);
            //    }
            //});
        }



        /// <summary>
        /// 三方登入驗證完成跑這裡
        /// </summary>
        void onThirdPartAuthFinished(AuthType _authType) {
            // 通知分析註冊完成事件
            GameManager.Instance.OnAuthFinished(_authType);
        }


        /// <summary>
        /// 使用者條款
        /// </summary>
        void onTermsOfUseClick() {
            //PopupUI.ShowLoading(StringData.GetUIString("Loading"));
            //FirebaseManager.GetDataByDocID(ColEnum.GameSetting, "BackendURL", (col, dic) => {
            //    PopupUI.HideLoading();
            //    string backendAddress = dic[BackendURLType.BackendAddress.ToString()].ToString();
            //    string userContractURL = dic[BackendURLType.UserContractURL.ToString()].ToString();
            //    string showUrl = string.Concat(backendAddress, userContractURL);
            //    WriteLog.Log($"[OnTermsOfUseClick] showUrl={showUrl}");
            //    Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            //    WebViewManager.Inst.ShowWebview(showUrl, rect);
            //});
        }

        /// <summary>
        /// 隱私權條款
        /// </summary>
        public void onProtectionPolicyClick() {
            //PopupUI.ShowLoading(StringData.GetUIString("Loading"));
            //FirebaseManager.GetDataByDocID(ColEnum.GameSetting, "BackendURL", (col, dic) => {
            //    PopupUI.HideLoading();
            //    string backendAddress = dic[BackendURLType.BackendAddress.ToString()].ToString();
            //    string protectionPolicyURL = dic[BackendURLType.ProtectionPolicyURL.ToString()].ToString();
            //    string showUrl = string.Concat(backendAddress, protectionPolicyURL);
            //    WriteLog.Log($"[OnProtectionPolicyClick] showUrl={showUrl}");
            //    Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            //    WebViewManager.Inst.ShowWebview(showUrl, rect);
            //});
        }

        public void OnClearBundleClick() {
            AddressableManage.Instance.ReDownload();
        }
    }
}