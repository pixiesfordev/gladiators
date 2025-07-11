using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Gladiators.Main;
using UnityEngine.AddressableAssets;

namespace Scoz.Func {
    public class PopupEventSpawner : ItemSpawner<PopupEventItem> {
        public static PopupEventSpawner Instance { get; private set; }
        public override void RefreshText() {
        }

        protected override void SetInstance() {
            Instance = this;
        }
    }
    public partial class PopupUI : MonoBehaviour {
        public static PopupUI Instance;
        public Canvas MyCanvas { get; private set; }



        //[HeaderAttribute("==============基本設定==============")]

        public void Init() {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MyCanvas = GetComponent<Canvas>();
            MyCanvas.worldCamera = UICam.Instance.GetComponent<Camera>();// GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            MyCanvas.sortingLayerName = "UI";
            InitGameInfo();
            InitLoading();
            InitPopupEvent();
            InitClickCancel();
            InitAttribueUI();
            InitConfirmCancel();
            InitInput();
            //InitScreenEffect();
            InitTipUI();
            InitGainSkillUI();

        }
        void Start() {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;

        }

        void OnDestroy() {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }
        private void Update() {
            if (ConfirmCancel_ConfirmBtnTimer != null) ConfirmCancel_ConfirmBtnTimer.RunTimer();
        }
        void OnLevelFinishedLoading(Scene _scene, LoadSceneMode _mode) {
            MyCanvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            MyCanvas.sortingLayerName = "UI";
        }





        [HeaderAttribute("==============Tip彈窗==============")]
        [SerializeField] TipUI MyTipUI;
        void InitTipUI() {
            if (MyTipUI == null) return;
            MyTipUI.Init();
        }

        [HeaderAttribute("==============說明彈窗==============")]
        [SerializeField] GameObject GameInfo_GO = null;
        [SerializeField] Text GameInfo_Content = null;
        [SerializeField] ContentSizeFitter GameInfo_ContentSizeFitter = null;
        void InitGameInfo() {
            if (GameInfo_GO == null) return;
            GameInfo_GO.SetActive(false);
        }
        public static void ShowGameInfo(string _infoUIStringID) {
            if (Instance.GameInfo_GO == null) return;
            string content = JsonString.GetUIString(_infoUIStringID);
            Instance.GameInfo_Content.text = content;
            CoroutineJob.Instance.StartNewAction(() => { Instance.GameInfo_ContentSizeFitter.Update(); }, 0.01f);
            Instance.GameInfo_GO.SetActive(true);
        }
        public void OnCloseInfoClick() {
            GameInfo_GO.SetActive(false);
        }



        [HeaderAttribute("==============讀取彈窗==============")]
        [SerializeField] GameObject LoadingGo = null;
        [SerializeField] Text LoadingText = null;
        static int LoadingCoroutineID;
        static string LoadingTimeOutStr;

        void InitLoading() {
            LoadingGo.SetActive(false);
        }
        /// <summary>
        /// 顯示Loading介面, _maxLoadingTime不傳入預設是看GameSetting表(預設999)
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_maxLoadingTime"></param>
        /// <param name="_loadingTimeOutStr"></param>
        public static void ShowLoading(string _text, float _maxLoadingTime = 999, string _loadingTimeOutStr = "") {
            WriteLog.Log("ShowLoading");
            if (!Instance)
                return;
            GameManager.UnloadUnusedAssets();//趁Loading時偷偷將null資源釋放
            Instance.LoadingGo.SetActive(true);
            Instance.LoadingText.text = _text;
            LoadingTimeOutStr = _loadingTimeOutStr;

            CoroutineJob.Instance.StopCoroutine(LoadingCoroutineID);
            if (_maxLoadingTime > 0) {
                LoadingCoroutineID = CoroutineJob.Instance.StartNewAction(() => {
                    HideLoading();
                    if (!string.IsNullOrEmpty(_loadingTimeOutStr))
                        ShowClickCancel(LoadingTimeOutStr, null);
                }, _maxLoadingTime);
            }
        }
        public static void HideLoading() {
            WriteLog.Log("HideLoading");
            if (!Instance)
                return;
            CoroutineJob.Instance.StopCoroutine(LoadingCoroutineID);
            Instance.LoadingGo.SetActive(false);
        }

        [HeaderAttribute("==============點擊關閉彈窗==============")]
        [SerializeField]
        GameObject ClickCancelGo = null;
        [SerializeField]
        Text ClickCancelText = null;
        Action ClickCancelAction = null;
        Action<object> ClickCancelActionWithParam = null;
        object ClickCancelParam;

        void InitClickCancel() {
            ClickCancelGo.SetActive(false);
        }

        public static void ShowClickCancel(string _text, Action _clickCancelAction) {
            if (!Instance)
                return;
            Instance.ClickCancelGo.SetActive(true);
            Instance.ClickCancelText.text = _text;
            Instance.ClickCancelAction = _clickCancelAction;
        }
        public static void ShowClickCancel(string _text, Action<object> _clickCancelAction, object _param) {
            if (!Instance)
                return;
            Instance.ClickCancelGo.SetActive(true);
            Instance.ClickCancelText.text = _text;
            Instance.ClickCancelActionWithParam = _clickCancelAction;
            Instance.ClickCancelParam = _param;
        }
        public void OnClickCancelClick() {
            if (!Instance)
                return;
            Instance.ClickCancelGo.SetActive(false);
            ClickCancelAction?.Invoke();
            ClickCancelActionWithParam?.Invoke(ClickCancelParam);
        }


        [HeaderAttribute("==============獲得道具彈窗==============")]
        [SerializeField]
        GainPropsUI MyGainPropsUI = null;

        void InitAttribueUI() {
            MyGainPropsUI.SetActive(false);
            MyGainPropsUI.Init();
        }


        [HeaderAttribute("==============確認/取消彈窗==============")]
        [SerializeField] GameObject ConfirmCancelGo = null;
        [SerializeField] Text ConfirmCancelText = null;
        [SerializeField] Text ConfirmCancel_ConfirmBtnText = null;
        [SerializeField] Text ConfirmCancel_CancelBtnText = null;
        [SerializeField] Button ConfirmCancel_ConfirmBtn;
        Action ConfirmCancelAction_Click = null;
        Action ConfirmCancelAction_Cancel = null;
        Action<object> ConfirmCancelAction_Click_WithParam = null;
        Action<object> ConfirmCancelAction_Cancel_WithParam = null;
        object ConfirmCancel_ConfirmParam;
        object ConfirmCancel_CancelParam;
        MyTimer ConfirmCancel_ConfirmBtnTimer;
        int ConfirmCanClickCoundownSecs;
        void InitConfirmCancel() {
            ConfirmCancelGo.SetActive(false);
        }
        public static void ShowConfirmCancel(string _text, Action _confirmAction, Action _cancelAction) {
            if (!Instance)
                return;
            Instance.ConfirmCancelGo.SetActive(true);
            Instance.ConfirmCancelText.text = _text;
            Instance.ConfirmCancelAction_Click = _confirmAction;
            Instance.ConfirmCancelAction_Cancel = _cancelAction;
            Instance.ConfirmCancelAction_Click_WithParam = null;
            Instance.ConfirmCancelAction_Cancel_WithParam = null;
            Instance.ConfirmCancel_ConfirmBtnText.text = JsonString.GetUIString("Confirm");
            Instance.ConfirmCancel_CancelBtnText.text = JsonString.GetUIString("Cancel");
            Instance.ConfirmCancel_ConfirmBtnTimer = null;
            Instance.ConfirmCancel_ConfirmBtn.interactable = true;
        }
        /// <summary>
        /// 顯示確認取消視窗 且 確認按鈕有倒數 倒數完才能點確認
        /// </summary>
        public static void ShowConfirmCancel(string _text, int _confirmCanClickCoundownSecs, Action _confirmAction, Action _cancelAction) {
            if (!Instance)
                return;
            if (_confirmCanClickCoundownSecs > 0) {
                Instance.ConfirmCanClickCoundownSecs = _confirmCanClickCoundownSecs;
                Instance.ConfirmCancel_ConfirmBtn.interactable = false;
                SetConfirmBtnText();
                Instance.ConfirmCancel_ConfirmBtnTimer = new MyTimer(1, () => {
                    Instance.ConfirmCanClickCoundownSecs--;
                    SetConfirmBtnText();
                }, true, true);
            } else {
                Instance.ConfirmCancel_ConfirmBtnTimer = null;
                Instance.ConfirmCancel_ConfirmBtnText.text = JsonString.GetUIString("Confirm");
                Instance.ConfirmCancel_ConfirmBtn.interactable = true;
            }

            Instance.ConfirmCancelGo.SetActive(true);
            Instance.ConfirmCancelText.text = _text;
            Instance.ConfirmCancelAction_Click = _confirmAction;
            Instance.ConfirmCancelAction_Cancel = _cancelAction;
            Instance.ConfirmCancelAction_Click_WithParam = null;
            Instance.ConfirmCancelAction_Cancel_WithParam = null;
            Instance.ConfirmCancel_CancelBtnText.text = JsonString.GetUIString("Cancel");
        }
        static void SetConfirmBtnText() {
            if (Instance.ConfirmCanClickCoundownSecs > 0) {
                Instance.ConfirmCancel_ConfirmBtn.interactable = false;
                Instance.ConfirmCancel_ConfirmBtnText.text = string.Format(JsonString.GetUIString("CowndownSec"), Instance.ConfirmCanClickCoundownSecs.ToString());
            } else {
                Instance.ConfirmCancel_ConfirmBtnTimer = null;
                Instance.ConfirmCancel_ConfirmBtn.interactable = true;
                Instance.ConfirmCancel_ConfirmBtnText.text = JsonString.GetUIString("Confirm");
            }
        }
        public static void ShowConfirmCancel(string _text, string _confirmText, string _cancelText, Action _confirmAction, Action _cancelAction) {
            if (!Instance)
                return;
            Instance.ConfirmCancelGo.SetActive(true);
            Instance.ConfirmCancelText.text = _text;
            Instance.ConfirmCancelAction_Click = _confirmAction;
            Instance.ConfirmCancelAction_Cancel = _cancelAction;
            Instance.ConfirmCancelAction_Click_WithParam = null;
            Instance.ConfirmCancelAction_Cancel_WithParam = null;
            Instance.ConfirmCancel_ConfirmBtnText.text = JsonString.GetUIString("Confirm");
            Instance.ConfirmCancel_CancelBtnText.text = JsonString.GetUIString("Cancel");
            Instance.ConfirmCancel_ConfirmBtnText.text = _confirmText;
            Instance.ConfirmCancel_CancelBtnText.text = _cancelText;
            Instance.ConfirmCancel_ConfirmBtnTimer = null;
            Instance.ConfirmCancel_ConfirmBtn.interactable = true;
        }


        public static void ShowConfirmCancel(string _text, Action<object> _confirmAction, object _confirmParam, Action<object> _cancelAction, object _cancelParam) {
            if (!Instance)
                return;
            Instance.ConfirmCancelGo.SetActive(true);
            Instance.ConfirmCancelText.text = _text;
            Instance.ConfirmCancelAction_Click = null;
            Instance.ConfirmCancelAction_Cancel = null;
            Instance.ConfirmCancelAction_Click_WithParam = _confirmAction;
            Instance.ConfirmCancelAction_Cancel_WithParam = _cancelAction;
            Instance.ConfirmCancel_ConfirmParam = _confirmParam;
            Instance.ConfirmCancel_CancelParam = _cancelParam;
            Instance.ConfirmCancel_ConfirmBtnText.text = JsonString.GetUIString("Confirm");
            Instance.ConfirmCancel_CancelBtnText.text = JsonString.GetUIString("Cancel");
            Instance.ConfirmCancel_ConfirmBtnTimer = null;
            Instance.ConfirmCancel_ConfirmBtn.interactable = true;
        }

        public void OnConfirmCancel_ConfirmClick() {
            if (!Instance)
                return;
            Instance.ConfirmCancelGo.SetActive(false);
            ConfirmCancelAction_Click?.Invoke();
            ConfirmCancelAction_Click_WithParam?.Invoke(ConfirmCancel_ConfirmParam);
        }

        public void OnConfirmCancel_CancelClick() {
            if (!Instance)
                return;
            Instance.ConfirmCancelGo.SetActive(false);
            ConfirmCancelAction_Cancel?.Invoke();
            ConfirmCancelAction_Cancel_WithParam?.Invoke(ConfirmCancel_CancelParam);
        }


        [HeaderAttribute("==============輸入彈窗==============")]
        [SerializeField]
        GameObject InputGo = null;
        [SerializeField]
        Text InputTitleText = null;
        [SerializeField]
        InputField MyInputField = null;
        Action<string> InputAction_Click_WithParam = null;
        Action InputAction_Cancel = null;
        void InitInput() {
            InputGo.SetActive(false);
        }


        public static void ShowInput(string _titleText, string _placeholder, string _text, Action<string> _confirmAction, Action _cancelAction) {
            if (!Instance)
                return;
            Instance.InputGo.SetActive(true);
            Instance.InputTitleText.text = _titleText;
            Instance.MyInputField.text = _text;
            Instance.MyInputField.placeholder.GetComponent<Text>().text = _placeholder;
            Instance.InputAction_Click_WithParam = _confirmAction;
            Instance.InputAction_Cancel = _cancelAction;
        }

        public void OnInput_ConfirmClick() {
            if (!Instance)
                return;
            Instance.InputGo.SetActive(false);
            InputAction_Click_WithParam?.Invoke(MyInputField.text);
        }

        public void OnInput_CancelClick() {
            if (!Instance)
                return;
            Instance.InputGo.SetActive(false);
            InputAction_Cancel?.Invoke();
        }

        //[HeaderAttribute("==============螢幕效果==============")]
        //[SerializeField]
        //Transform ScreenEffectTrans;
        //void InitScreenEffect() {
        //    ScreenEffectTrans.gameObject.SetActive(false);
        //}
        //public static void CallScreenEffect(string _name) {
        //    Instance.ScreenEffectTrans.gameObject.SetActive(true);
        //    Instance.ScreenEffectTrans.Find(_name).gameObject.SetActive(true);
        //}
        //public void OnScreenEffectEnd() {
        //    foreach (Transform trans in ScreenEffectTrans) {
        //        trans.gameObject.SetActive(false);
        //    }
        //}





        [HeaderAttribute("==============事件短暫彈窗==============")]
        [SerializeField]
        PopupEventItem PopupItemPrefab = null;
        [SerializeField]
        Transform PopupEventParent = null;
        PopupEventSpawner MyPopupEventSpawner;
        public static bool CanShowEvent { get; set; } = true;

        void InitPopupEvent() {
            PopupEventParent.gameObject.SetActive(true);
            MyPopupEventSpawner = gameObject.AddComponent<PopupEventSpawner>();
            MyPopupEventSpawner.ParentTrans = PopupEventParent;
            MyPopupEventSpawner.ItemPrefab = PopupItemPrefab;
        }

        public static void ShowPopupEvent(string _text) {
            if (!Instance)
                return;
            if (!CanShowEvent)
                return;
            //Vibrator.Vibrate(GameSettingJsonData.GetInt(GameSetting.PopupEventVibrationMilliSecs));//手機震動
            PopupEventItem item = Instance.GetAvailableItem();
            if (item == null)
                item = Instance.SpawnNewItem();
            item.Init(_text, null);
        }
        public static void ShowPopupEvent(string _title, Action<object[]> _action, params object[] _objects) {
            if (!Instance)
                return;
            if (!CanShowEvent)
                return;
            //Vibrator.Vibrate(GameSettingJsonData.GetInt(GameSetting.PopupEventVibrationMilliSecs));//手機震動
            PopupEventItem item = Instance.GetAvailableItem();
            if (item == null)
                item = Instance.SpawnNewItem();
            item.Init(_title, _action, _objects);
        }
        PopupEventItem SpawnNewItem() {
            return MyPopupEventSpawner.Spawn<PopupEventItem>();
        }
        PopupEventItem GetAvailableItem() {
            for (int i = 0; i < MyPopupEventSpawner.ItemList.Count; i++) {
                if (!MyPopupEventSpawner.ItemList[i].IsActive)
                    return MyPopupEventSpawner.ItemList[i];
            }
            return null;
        }

        [HeaderAttribute("==============設定視窗==============")]

        //進遊戲不先初始化，等到要用時才初始化UI
        [SerializeField] Transform SettingUIParent;
        [SerializeField] AssetReference SettingUIAsset;
        Gladiators.Main.SettingUI MySettingUI;

        void InitSettingUI(Action _ac) {
            PopupUI.ShowLoading(JsonString.GetUIString("WaitForLoadingUI"));
            //初始化UI
            AddressablesLoader.GetPrefabByRef(Instance.SettingUIAsset, (prefab, handle) => {
                PopupUI.HideLoading();
                GameObject go = Instantiate(prefab);
                go.transform.SetParent(Instance.SettingUIParent);
                go.transform.localPosition = prefab.transform.localPosition;
                go.transform.localScale = prefab.transform.localScale;
                //RectTransform rect = go.GetComponent<RectTransform>();
                //rect.offsetMin = Vector2.zero;//Left、Bottom
                //rect.offsetMax = Vector2.zero;//Right、Top
                Instance.MySettingUI = go.GetComponent<Gladiators.Main.SettingUI>();
                Instance.MySettingUI.Init();
                Instance.MySettingUI.SetActive(false);
                _ac?.Invoke();
            }, () => { WriteLog.LogError("載入GameSettingUIAsset失敗"); });
        }
        public static void CallSettingUI() {
            if (!Instance)
                return;
            //判斷是否已經載入過此UI，若還沒載過就跳讀取中並開始載入
            if (Instance.MySettingUI != null) {
                Instance.MySettingUI.SetActive(true);
            } else {
                Instance.InitSettingUI(() => {
                    Instance.MySettingUI.SetActive(true);
                });
            }
        }



        //進遊戲不先初始化，等到要用時才初始化UI
        [HeaderAttribute("==============轉場UI==============")]
        [SerializeField] AssetReference UITransitionAsset;
        [SerializeField] Transform UITransitionParent;
        UITransition MyUITransition = null;
        Action OnUITransitionAssetLoadFinishedAC;//載完Asset後要執行的Action
        static bool IsLoadingUITransitionAsset = false;//是否載入UI中

        void InitUITransition() {
            if (IsLoadingUITransitionAsset)
                return;
            IsLoadingUITransitionAsset = true;
            PopupUI.ShowLoading(JsonString.GetUIString("Loading"));
            //初始化UI
            AddressablesLoader.GetPrefabByRef(Instance.UITransitionAsset, (prefab, handle) => {
                IsLoadingUITransitionAsset = false;
                PopupUI.HideLoading();
                GameObject go = Instantiate(prefab);
                go.transform.SetParent(Instance.UITransitionParent);
                go.transform.localPosition = prefab.transform.localPosition;
                go.transform.localScale = prefab.transform.localScale;
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.offsetMin = Vector2.zero;//Left、Bottom
                rect.offsetMax = Vector2.zero;//Right、Top
                go.transform.SetAsLastSibling();
                Instance.MyUITransition = go.GetComponent<UITransition>();
                Instance.MyUITransition.gameObject.SetActive(true);
                Instance.MyUITransition.InitTransition();
                Instance.OnUITransitionAssetLoadFinishedAC?.Invoke();
            }, () => { WriteLog.LogError("載入UITransitionAsset失敗"); });
        }
        public static void InitUITransitionProgress(params string[] _keys) {
            if (Instance == null)
                return;

            //判斷是否已經載入過此UI，若還沒載過就跳讀取中並開始載入
            if (Instance.MyUITransition != null) {
                Instance.MyUITransition.SetTransitionProgress(_keys);
            } else {
                Instance.OnUITransitionAssetLoadFinishedAC += () => { Instance.MyUITransition.SetTransitionProgress(_keys); };
                Instance.InitUITransition();
            }
        }

        public static void FinishUITransitionProgress(string _key) {
            if (Instance == null)
                return;

            //判斷是否已經載入過此UI，若還沒載過就跳讀取中並開始載入
            if (Instance.MyUITransition != null) {
                Instance.MyUITransition.FinishTransitionProgress(_key);
            } else {
                Instance.OnUITransitionAssetLoadFinishedAC += () => { Instance.MyUITransition.FinishTransitionProgress(_key); };
                Instance.InitUITransition();
            }

        }
        public static void CallUITransition(Sprite _sprite, string _text, float _minWaitSec, Action _ac = null) {
            if (Instance == null)
                return;

            //判斷是否已經載入過此UI，若還沒載過就跳讀取中並開始載入
            if (Instance.MyUITransition != null) {
                Instance.MyUITransition.CallTransition(_sprite, _text, _minWaitSec, _ac);
            } else {
                Instance.OnUITransitionAssetLoadFinishedAC += () => { Instance.MyUITransition.CallTransition(_sprite, _text, _minWaitSec, _ac); };
                Instance.InitUITransition();
            }
        }

        [HeaderAttribute("==============設定視窗==============")]

        //進遊戲不先初始化，等到要用時才初始化UI
        [SerializeField] Transform GainSkillUIParent;
        [SerializeField] AssetReference GainSkillUIAsset;

        void InitGainSkillUI() {
            //初始化UI
            AddressablesLoader.GetPrefabByRef(Instance.GainSkillUIAsset, (prefab, handle) => {
                GameObject go = Instantiate(prefab);
                go.transform.SetParent(Instance.GainSkillUIParent);
                go.transform.localPosition = prefab.transform.localPosition;
                go.transform.localScale = prefab.transform.localScale;
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.offsetMin = Vector2.zero;//Left、Bottom
                rect.offsetMax = Vector2.zero;//Right、Top
                var ui = go.GetComponent<GainSkillUI>();
                ui.Init();
                ui.SetActive(false);
            }, () => { WriteLog.LogError("載入GameGainSkillUIAsset失敗"); });
        }


    }
}
