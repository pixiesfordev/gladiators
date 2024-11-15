using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using Scoz.Func;
using Gladiators.Main;
using UnityEngine.AddressableAssets;
using DG.Tweening;
using Gladiators.Socket;
using UniRx;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Gladiators.Battle {
    public class DivineSelectUI : BaseUI {

        [SerializeField] Image BGFore;
        [SerializeField] Button ConfirmBtn;
        [SerializeField] Text PlayerMoney;
        [SerializeField] DivineSkill[] DivineSkills;
        [SerializeField] Transform DropCoinTrans;
        [SerializeField] Text SureBtnText;
        [SerializeField] Image ApertureImage;
        [SerializeField] RectTransform ApertureRT;
        [SerializeField] RectTransform LeftArrowRT;
        [SerializeField] RectTransform RightArrowRT;
        [SerializeField] RectTransform BtnBGMaskRT;
        [SerializeField] Image BtnBGDecoration;
        [SerializeField] Material GrayMaterial;

        [SerializeField] Transform[] BGMoveWithCameraTiers; //會跟著鏡頭移動的分層 拆五層 五層移動量會不一樣 做出深度感
        [SerializeField] DivineCandle[] CandleObjs; //蠟燭動畫

        [HeaderAttribute("==============TEST==============")]
        [Tooltip("每根蠟燭倒數時間")][SerializeField] float PerCandleCountDownTime = 1f;
        [Tooltip("測試蠟燭倒數演出")][SerializeField] bool PerformCandleCountDown = false;
        [Tooltip("光圈演出模式 打勾為逐漸縮小 否則為一步一步縮小")][SerializeField] bool ApertureModeDigital = false;
        [Tooltip("光圈縮至最小的倍率 即蠟燭全熄滅時 光圈相對於原本大小的倍率 必須大於0")][SerializeField] float ApertureMinSize = 0.3f;
        [Tooltip("背景變至最暗時的亮度 值為0~1")][SerializeField] float BGDarkestBrightness = 0.3f;
        //[Tooltip("")][SerializeField]

        //TODO:
        //1.卡片被選中要有光圈(先上光圈圖)
        //2.鏡頭左右偏移效果
        // 1.PC版本
        // 2.手機版本

        /*
        1.神址卡牌演出調用
        Relics_ Normal >> 神址常態性
        Relics_floating >> 神址牌飄動
        Relics_Click >> 神址點選
        Relics_bigger >> 神址放大
        Relics_bigger Normal >> 神址放大_常態
        Relics_Cancelled >> 神址取消
        ruins_decision_selection >> 神址_決定_選取
        Ruins_Decision_Not Selected >> 神址_決定_未選取
        */

        public static DivineSelectUI Instance;

        JsonSkill[] SelectedDivineSkills;
        CancellationTokenSource CandleCountDownCTS; //用來控制中斷蠟燭倒數
        bool Confirmed = false; //鎖定確認按鈕 避免重複發送
        Tweener ApertureScaleTween; //光圈動畫控件
        Tweener BtnSizeTween; //按鈕延伸放大動畫控件
        Tweener BtnMaskSizeTween; //按鈕遮罩放大動畫控件
        Tweener LeftArrowPosTween; //按鈕左邊箭頭位移動畫控件
        Tweener RightArrowPosTween; //按鈕右邊箭頭位移動畫控件
        float BtnAniTime = 2f; //按鈕動畫時間

        enum DivineSkillSelectState : short {
            Choose,//選擇
            Cancel,//取消選擇
            CannotSelect,//不可選擇
            GoldNotEnough,//金錢不足
        }

        public override void RefreshText() {

        }

        protected override void SetInstance() {
            Instance = this;
        }

        public override void Init() {
            base.Init();

            SelectedDivineSkills = new JsonSkill[2];

            //更新玩家持有金幣
            UpdatePlayerGold();

            //按鈕文字設定
            SureBtnText.text = JsonString.GetUIString("Confirm");

            //讀取技能並設定技能進來 先用預設的技能
            DivineSkills[0].SetData(GameDictionary.GetJsonData<JsonSkill>(10001));
            DivineSkills[1].SetData(GameDictionary.GetJsonData<JsonSkill>(10110));
            DivineSkills[2].SetData(GameDictionary.GetJsonData<JsonSkill>(10201));
            DivineSkills[3].SetData(GameDictionary.GetJsonData<JsonSkill>(10203));

            //倒數蠟燭
            CountDownCandleTime();
        }

        // Update is called once per frame
        void Update() {
            if (PerformCandleCountDown) {
                CountDownCandleTime();
                PerformCandleCountDown = false;
            }
        }

        void UpdatePlayerGold() {
            var playerDB = GamePlayer.Instance.GetDBData<DBPlayer>();
            var playerGold = playerDB != null ? playerDB.Gold : 0;
            PlayerMoney.text = playerGold.ToString();
        }

        void CountDownCandleTime() {
            CandleCountDownCTS?.Cancel();
            StopApertureDoScale();
            CandleCountDownCTS = new CancellationTokenSource();
            PlayCountDownCandleTime(CandleCountDownCTS).Forget();
        }

        //停止蠟燭倒數
        void EndCandleCountDown() {
            //停止所有倒數演出
            CandleCountDownCTS?.Cancel();
            StopApertureDoScale();

            //全蠟燭熄滅
            foreach(var c in CandleObjs) {
                if (c.IsCombusting())
                    c.GoOutCandle();
            }

            //設定光圈&亮度(變至最小&最暗)
            ApertureRT.localScale = new Vector3(ApertureMinSize, ApertureMinSize, 1f);
            BGFore.color = new Color(BGDarkestBrightness, BGDarkestBrightness, BGDarkestBrightness);
            ApertureImage.color = new Color(BGDarkestBrightness, BGDarkestBrightness, BGDarkestBrightness);
        }

        //重置蠟燭
        void ResetCandles() {
            foreach (var c in CandleObjs) {
                c.ResetCandle();
            }
            Confirmed = false;
        }

        //重設背景亮度
        void ResetBGFore() {
            BGFore.color = Color.white;
        }

        //重設光圈大小
        void ResetApeture() {
            ApertureRT.localScale = Vector3.one;
            ApertureImage.color = Color.white;
        }

        //中止光圈演出
        void StopApertureDoScale() {
            if (ApertureScaleTween != null) {
                ApertureScaleTween.Pause();
                ApertureScaleTween.Kill();
            }
        }

        void ResetButton() {
            //設定按鈕(短版色彩)
            SetSureBtnState(false);
        }

        //倒數蠟燭熄滅(用PerCandleCountDownTime設定每根蠟燭倒數時間)
        async UniTaskVoid PlayCountDownCandleTime(CancellationTokenSource ctk) {
            //重置演出相關物件
            ResetCandles();
            ResetBGFore();
            ResetApeture();

            int CandleNum = CandleObjs.Length;

            //背景色彩相關參數(變暗演出)
            float bgColorDelta = (1f - BGDarkestBrightness) / CandleNum; //每次亮度變化量
            float curBgColor = 1f; //目前亮度值

            //光圈演出相關參數
            float apertureDigitalDelta = (1f - ApertureMinSize) / CandleNum; //光圈數位演出方式每次差異值(一秒變一次)
            float curApertureSize = 1f; //光圈目前尺寸

            //光圈類比式演出(逐漸縮小) 演出時間會+1秒是因為比較早開始演出
            if (!ApertureModeDigital)
                ApertureScaleTween = ApertureRT.DOScale(new Vector3(ApertureMinSize, ApertureMinSize, 1f), CandleNum + 1f);

            while (CandleNum > 0) {
                await UniTask.WaitForSeconds(PerCandleCountDownTime, cancellationToken: ctk.Token);
                //熄滅蠟燭
                CandleObjs[CandleNum - 1].GoOutCandle();
                CandleNum -= 1;
                //光圈大小調整(類比式 一秒變一次)
                curApertureSize -= apertureDigitalDelta;
                if (ApertureModeDigital)
                    ApertureRT.localScale = new Vector3(curApertureSize, curApertureSize, 1f);
                //背景亮度調整
                curBgColor -= bgColorDelta;
                BGFore.color = new Color(curBgColor, curBgColor, curBgColor);
                ApertureImage.color = new Color(curBgColor, curBgColor, curBgColor);
            }

            //時間到直接發送封包 先鎖定按鈕 等待一禎再發送 避免重複發送封包
            Confirmed = true;
            await UniTask.Yield();
            //TODO:如果要測試演出效果這裡就註解 這樣倒數結束也不會送出封包 可以看演出效果
            SendDivineSkill();
        }

        //卡牌選擇判斷
        public bool SelectDivineSkill(JsonSkill _skill) {
            int pos = 0;
            DivineSkillSelectState result = CheckSelectDivineSkill(pos, _skill);
            if (result == DivineSkillSelectState.CannotSelect) {
                pos = 1;
                result = CheckSelectDivineSkill(pos, _skill);
            }

            switch (result) {
                case DivineSkillSelectState.Choose:
                    ChooseSkill(pos, _skill);
                    return true;
                case DivineSkillSelectState.Cancel:
                    CancelSkill(pos);
                    return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 檢查選擇技能
        /// </summary>
        /// <param name="_selectedSkill">舊的已選擇的技能</param>
        /// <param name="_selectSkill">新選擇的技能</param>
        /// <returns>選擇狀態</returns>
        DivineSkillSelectState CheckSelectDivineSkill(int _skillPos, JsonSkill _selectSkill) {
            if (_skillPos < 0 || _skillPos > 1)
                return DivineSkillSelectState.CannotSelect;
            JsonSkill _selectedSkill = SelectedDivineSkills[_skillPos];
            //新選擇技能為空的 理論上不會發生 一旦發生回傳不可選擇
            if (_selectSkill == null)
                return DivineSkillSelectState.CannotSelect;
            if (_selectedSkill != null) {
                //兩個技能一樣則表示取消選擇
                if (_selectedSkill.ID == _selectSkill.ID)
                    return DivineSkillSelectState.Cancel;
                //該位置已經有技能且兩個技能不一樣則不可以替代 回傳不可選擇
                if (_selectSkill.ID != _selectedSkill.ID)
                    return DivineSkillSelectState.CannotSelect;
            }
            //判斷玩家金錢是否足夠可以選擇
            var playerDB = GamePlayer.Instance.GetDBData<DBPlayer>();
            if (playerDB != null) {
                int costGold = _selectSkill.Cost;
                JsonSkill anotherSkill = _skillPos == 0 ? SelectedDivineSkills[1] : SelectedDivineSkills[0];
                if (anotherSkill != null)
                    costGold += anotherSkill.Cost;
                if (playerDB.Gold < costGold)
                    return DivineSkillSelectState.GoldNotEnough;
            }
            //此位置還沒有技能 選擇
            return DivineSkillSelectState.Choose;
        }

        void ChooseSkill(int pos, JsonSkill _skill) {
            if (pos > 1 || pos < 0)
                return;
            SelectedDivineSkills[pos] = _skill;
        }

        void CancelSkill(int pos) {
            if (pos > 1 || pos < 0)
                return;
            SelectedDivineSkills[pos] = null;
        }

        public void ClickSure() {
            if (Confirmed) return;
            Confirmed = true;
            SendDivineSkill();
        }

        void SendDivineSkill() {
            //掉落金幣演出(先用白板落下位移演出) >> 先隱藏 目前沒要求掉落演出
            //CoinDrop();
            //TODO:扣錢(前端先扣 之後後端實際接上扣錢回傳這段應該去掉 直接後端回接更新就好)
            DeductionCoin();
            //設定按鈕(打灰拉長)
            SetSureBtnState(true);
            //按鈕文字設定(等待玩家)
            SureBtnText.text = JsonString.GetUIString("WaitPlayer");
            //讀取選中技能
            int selectedSkillID1 = SelectedDivineSkills[0] != null ? SelectedDivineSkills[0].ID : 0;
            int selectedSkillID2 = SelectedDivineSkills[1] != null ? SelectedDivineSkills[1].ID : 0;

            //播放神祇技能選擇動畫
            int checkId = 0;
            for (int i = 0; i < DivineSkills.Length; i++) {
                checkId = DivineSkills[i].GetSkillDataID();
                if (checkId != 0 && (checkId == selectedSkillID1 || checkId == selectedSkillID2)) {
                    DivineSkills[i].PlayDecisionSelected();
                } else {
                    DivineSkills[i].PlayDecisionNotSelected();
                }
            }

            //發送Socket
            AllocatedRoom.Instance.SetDivineSkills(new int[] { selectedSkillID1, selectedSkillID2 });
        }

        void SetSureBtnState(bool bGray) {
            if (BtnSizeTween != null) {
                BtnSizeTween.Pause();
                BtnSizeTween.Kill();
            }
            if (BtnMaskSizeTween != null) {
                BtnMaskSizeTween.Pause();
                BtnMaskSizeTween.Kill();
            }
            if (LeftArrowPosTween != null) {
                LeftArrowPosTween.Pause();
                LeftArrowPosTween.Kill();
            }
            if (RightArrowPosTween != null) {
                RightArrowPosTween.Pause();
                RightArrowPosTween.Kill();
            }

            //按鈕相關演出
            RectTransform btnRT = ConfirmBtn.GetComponent<RectTransform>();
            Image btnImage = btnRT.GetComponent<Image>();
            Image maskImage = BtnBGMaskRT.GetComponent<Image>();
            Image LeftArrowImage = LeftArrowRT.GetComponent<Image>();
            Image RightArrowImage = RightArrowRT.GetComponent<Image>();
            if (bGray) {
                //按鈕圖片要逐步變長
                if (btnRT != null)
                    BtnSizeTween = btnRT.DOSizeDelta(new Vector2(630f, 135f), BtnAniTime);
                //按鈕要打灰
                if (btnImage != null)
                    btnImage.material = GrayMaterial;
                //按鈕遮罩要逐步變長
                BtnMaskSizeTween = BtnBGMaskRT.DOSizeDelta(new Vector2(600f, 102f), BtnAniTime);
                //按鈕遮罩要打灰
                if (maskImage != null)
                    maskImage.material = GrayMaterial;
                //裝飾箭頭要逐漸移動位置
                LeftArrowPosTween = LeftArrowRT.DOAnchorPos3D(new Vector3(292f, 0f, 0f), BtnAniTime);
                if (LeftArrowImage != null)
                    LeftArrowImage.material = GrayMaterial;
                RightArrowPosTween = RightArrowRT.DOAnchorPos3D(new Vector3(-292f, 0f, 0f), BtnAniTime);
                if (RightArrowImage != null)
                    RightArrowImage.material = GrayMaterial;
                BtnBGDecoration.material = GrayMaterial;
            } else {
                //還原所有按鈕相關UI
                if (btnRT != null)
                    btnRT.sizeDelta = new Vector2(367f, 135f);
                if (btnImage != null)
                    btnImage.material = null;
                BtnBGMaskRT.sizeDelta = new Vector2(308f, 94f);
                if (maskImage != null)
                    maskImage.material = null;
                LeftArrowRT.anchoredPosition = new Vector2(154f, 0f);
                if (LeftArrowImage != null)
                    LeftArrowImage.material = null;
                RightArrowRT.anchoredPosition = new Vector2(-154f, 0f);
                if (RightArrowImage != null)
                    RightArrowImage.material = null;
                BtnBGDecoration.material = null;
            }
        }

        void CoinDrop() {
            //重置位置
            DropCoinTrans.localPosition = Vector3.zero;
            //掉落演出
            DropCoinTrans.DOLocalMove(new Vector3(-24f, -430f, 0f), BtnAniTime, true);
        }

        void DeductionCoin() {
            int costGold = SelectedDivineSkills[0] != null ? SelectedDivineSkills[0].Cost : 0;
            costGold += SelectedDivineSkills[1] != null ? SelectedDivineSkills[1].Cost : 0;
            var playerDB = GamePlayer.Instance.GetDBData<DBPlayer>();
            var playerGold = playerDB != null ? playerDB.Gold : 0;
            PlayerMoney.text = (playerGold - costGold).ToString();
        }

        //接回傳關閉介面(寫在這裡比較方便追蹤 不然不知道誰會在哪裡關閉 且重設介面邏輯寫在此)
        public void CloseUI(Action _afterCloseAct) {
            WriteLog.Log("接到回傳 準備進入戰鬥!");
            UniTask.Void(async () => {
                DoCloseAni(_afterCloseAct).Forget();
            });
        }

        async UniTaskVoid DoCloseAni(Action _afterCloseAct) {
            //停止倒數 隱藏全部蠟燭 準備進入戰鬥
            EndCandleCountDown();
            //更新玩家金錢(之後要實際接封包 目前先重新抓一次資料)
            UpdatePlayerGold();
            //等待演出結束 先設定等待三秒 之後有完整演出改等待正確演出時間
            await UniTask.WaitForSeconds(3f);
            WriteLog.Log("關閉介面 進入戰鬥!");
            base.SetActive(false);
            ResetCandles();
            ResetBGFore();
            ResetApeture();
            ResetButton();
            _afterCloseAct();
        }

        //敵方資料來源 >> AllocatedRoom的ReceiveSetPlayer下的GotOpponent


    }
}
