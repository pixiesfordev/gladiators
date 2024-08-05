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
    public class DivineSelectUI : BaseUI
    { 

        [SerializeField] Button ConfirmBtn;
        [SerializeField] Text PlayerMoney;
        [SerializeField] GameObject[] Candles;
        [SerializeField] DivineSkill[] DivineSkills;
        [SerializeField] Transform DropCoinTrans;
        [SerializeField] Text SureBtnText;

        [HeaderAttribute("==============TEST==============")]
        [Tooltip("每根蠟燭倒數時間")][SerializeField] float PerCandleCountDownTime = 1f;
        [Tooltip("測試蠟燭倒數演出")][SerializeField] bool PerformCandleCountDown = false;

        public static DivineSelectUI Instance;

        JsonSkill[] SelectedDivineSkills;
        CancellationTokenSource CandleCountDownCTS; //用來控制中斷蠟燭倒數
        bool lockSureBtn = false; //鎖定確認按鈕 避免重複發送

        enum DivineSkillSelectState : short
        {
            Choose,//選擇
            Cancel,//取消選擇
            CannotSelect,//不可選擇
            GoldNotEnough,//金錢不足
        }

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        public override void Init() {
            base.Init();

            Instance = this;

            SelectedDivineSkills = new JsonSkill[2];

            //更新玩家持有金幣
            UpdatePlayerGold();

            //按鈕文字設定
            SureBtnText.text = JsonString.GetUIString("Confirm");

            //讀取技能並設定技能進來 先用預設的技能
            DivineSkills[0].SetData(GameDictionary.GetJsonData<JsonSkill>(10001));
            DivineSkills[1].SetData(GameDictionary.GetJsonData<JsonSkill>(10101));
            DivineSkills[2].SetData(GameDictionary.GetJsonData<JsonSkill>(10201));
            DivineSkills[3].SetData(GameDictionary.GetJsonData<JsonSkill>(10301));

            //倒數蠟燭
            CountDownCandleTime();
        }

        // Update is called once per frame
        void Update()
        {
            if (PerformCandleCountDown)
            {
                CountDownCandleTime();
                PerformCandleCountDown = false;
            }
        }

        void UpdatePlayerGold()
        {
            var playerDB = GamePlayer.Instance.GetDBPlayerDoc<DBPlayer>();
            var playerGold = playerDB != null ? playerDB.Gold : 0;
            PlayerMoney.text = playerGold.ToString();
        }

        void CountDownCandleTime()
        {
            UniTask.Void(async () => {
                if (CandleCountDownCTS != null)
                    CandleCountDownCTS.Cancel();
                CandleCountDownCTS = new CancellationTokenSource();
                PlayCountDownCandleTime(CandleCountDownCTS).Forget();
            });
        }

        //停止蠟燭倒數
        void EndCandleCountDown()
        {
            if (CandleCountDownCTS != null)
                CandleCountDownCTS.Cancel();
            foreach(var c in Candles)
                c.gameObject.SetActive(false);
        }

        //重置蠟燭
        void ResetCandles()
        {
            foreach(var c in Candles)
                c.gameObject.SetActive(true);
            lockSureBtn = false;
        }

        //倒數蠟燭熄滅(用PerCandleCountDownTime設定每根蠟燭倒數時間)
        async UniTaskVoid PlayCountDownCandleTime(CancellationTokenSource ctk) {
            ResetCandles();
            int CandleNum = Candles.Length;
            while (CandleNum > 0)
            {
                await UniTask.WaitForSeconds(PerCandleCountDownTime, cancellationToken:ctk.Token);
                Candles[CandleNum - 1].gameObject.SetActive(false);
                CandleNum -= 1;
            }
            //時間到直接發送封包 先鎖定按鈕 等待一禎再發送 避免重複發送封包
            lockSureBtn = true;
            await UniTask.Yield();
            SendBribe();
        }

        //卡牌選擇判斷
        public bool SelectDivineSkill(JsonSkill _skill)
        {
            int pos = 0;
            DivineSkillSelectState result = CheckSelectDivineSkill(pos, _skill);
            if (result == DivineSkillSelectState.CannotSelect)
            {
                pos = 1;
                result = CheckSelectDivineSkill(pos, _skill);
            }

            switch (result)
            {
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
        DivineSkillSelectState CheckSelectDivineSkill(int _skillPos, JsonSkill _selectSkill)
        {
            if (_skillPos < 0 || _skillPos > 1)
                return DivineSkillSelectState.CannotSelect;
            JsonSkill _selectedSkill = SelectedDivineSkills[_skillPos];
            //新選擇技能為空的 理論上不會發生 一旦發生回傳不可選擇
            if (_selectSkill == null)
                return DivineSkillSelectState.CannotSelect;
            if (_selectedSkill != null)
            {
                //兩個技能一樣則表示取消選擇
                if (_selectedSkill.ID == _selectSkill.ID)
                    return DivineSkillSelectState.Cancel;
                //該位置已經有技能且兩個技能不一樣則不可以替代 回傳不可選擇
                if (_selectSkill.ID != _selectedSkill.ID)
                    return DivineSkillSelectState.CannotSelect;
            }  
            //判斷玩家金錢是否足夠可以選擇
            var playerDB = GamePlayer.Instance.GetDBPlayerDoc<DBPlayer>();
            if (playerDB != null)
            {
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

        void ChooseSkill(int pos, JsonSkill _skill)
        {
            if (pos > 1 || pos < 0)
                return;
            SelectedDivineSkills[pos] = _skill;
        }

        void CancelSkill(int pos)
        {
            if (pos > 1 || pos < 0)
                return;
            SelectedDivineSkills[pos] = null;
        }

        public void ClickSure()
        {
            if (lockSureBtn) return;
            lockSureBtn = true;
            SendBribe();
        }

        void SendBribe()
        {
            //掉落金幣演出(先用白板落下位移演出)
            CoinDrop();
            //TODO:扣錢(前端先扣 之後後端實際接上扣錢回傳這段應該去掉 直接後端回接更新就好)
            DeductionCoin();
            //按鈕文字設定(等待玩家)
            SureBtnText.text = JsonString.GetUIString("WaitPlayer");
            int selectedSkillID1 = SelectedDivineSkills[0] != null ? SelectedDivineSkills[0].ID : 0;
            int selectedSkillID2 = SelectedDivineSkills[1] != null ? SelectedDivineSkills[1].ID : 0;
            //發送Socket
            GameConnector.Instance.SetDivineSkills(new int[] { selectedSkillID1, selectedSkillID2 });
        }

        void CoinDrop()
        {
            //重置位置
            DropCoinTrans.localPosition = Vector3.zero;
            //掉落演出
            DropCoinTrans.DOLocalMove(new Vector3(0f, -430f, 0f), 2f, true);
        }

        void DeductionCoin()
        {
            int costGold = SelectedDivineSkills[0] != null ? SelectedDivineSkills[0].Cost : 0;
            costGold += SelectedDivineSkills[1] != null ? SelectedDivineSkills[1].Cost : 0;
            var playerDB = GamePlayer.Instance.GetDBPlayerDoc<DBPlayer>();
            var playerGold = playerDB != null ? playerDB.Gold : 0;
            PlayerMoney.text = (playerGold - costGold).ToString();
        }

        //接回傳關閉介面(寫在這裡比較方便追蹤 不然不知道誰會在哪裡關閉 且重設介面邏輯寫在此)
        public void CloseUI(Action _afterCloseAct)
        {
            WriteLog.Log("接到回傳 準備進入戰鬥!");
            UniTask.Void(async () => {
                DoCloseAni(_afterCloseAct).Forget();
            });
        }

        async UniTaskVoid DoCloseAni(Action _afterCloseAct)
        {
            //停止倒數 隱藏全部蠟燭 準備進入戰鬥
            EndCandleCountDown();
            //更新玩家金錢(之後要實際接封包 目前先重新抓一次資料)
            UpdatePlayerGold();
            //等待演出結束 先設定暴力等待三秒 之後有完整演出改等待正確演出時間
            await UniTask.WaitForSeconds(3f);
            WriteLog.Log("關閉介面 進入戰鬥!");
            base.SetActive(false);
            ResetCandles();
            _afterCloseAct();
        }

        //敵方資料來源 >> AllocatedRoom的ReceiveSetPlayer下的GotOpponent

        public override void RefreshText() {

        }
    }
}
