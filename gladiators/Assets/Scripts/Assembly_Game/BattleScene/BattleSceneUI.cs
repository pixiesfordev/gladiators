using Gladiators.Battle;
using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Gladiators.Main;
using Cysharp.Threading.Tasks;
using Gladiators.Socket.Matchgame;
using Unity.Entities.UniversalDelegates;
using System.Linq;
using Castle.Core.Internal;
using System;

public class BattleSceneUI : BaseUI {
    [HeaderAttribute("==============AddressableAssets==============")]
    [SerializeField] AssetReference BattleManagerAsset;

    [HeaderAttribute("==============UI==============")]
    [SerializeField] BattleStaminaObj MyBattleStaminaObj;
    [SerializeField] DivineSelectUI MyDivineSelectUI;
    //TODO:考慮是否之後改腳本產生物件
    //上方角鬥士資訊
    public BattleGladiatorInfo PlayerGladiatorInfo;
    public BattleGladiatorInfo EnemyGladiatorInfo;

    [SerializeField] Text PlayerGoldText;//玩家持有金錢

    [SerializeField] MyTextPro BattleLeftTime;//戰鬥剩餘時間


    //技能手牌
    [SerializeField] BattleSkillButton[] SkillBtns;
    [SerializeField] NextBattleSkillButton NextSkillBtn;

    [SerializeField] BattleDivineSkill[] DivineSkills;//神址卡牌

    //衝刺按鈕(名稱是Button00 在SprintBtnPart底下 不要改這個物件的名字 因為美術演出直接在這個物件上拉 改名演出會失效)
    [SerializeField] BattleSprintButton SprintBtn;

    [SerializeField] GameObject SettingBtn;//設定按鈕

    [SerializeField] BattleFIGHT BattleFightObj;//開場演出
    [SerializeField] Animator BattleKO;//KO演出

    [SerializeField] BattleMoney MoneyObj;//戰鬥金幣物件(UI)

    /// <summary>
    /// 是否能施放立即技能
    /// </summary>
    public bool CanSpellInstantSkill {
        get {
            bool isLock = spellLocker.Any(pair => pair.Value);
            //if (isLock) WriteLog.Log(getSpellLockStr);
            return isLock;
        }
    }
    string getSpellLockStr {
        get {
            var keys = spellLocker.Where(pair => pair.Value).Select(pair => pair.Key);
            return $"技能鎖住項目:  {string.Join(", ", keys)}";
        }
    }
    public enum SpellLock {
        Casting, // 施放中
        Effect, // 狀態效果影響
        InMeleeRange, // 在肉搏限定距離內(與對手角鬥士的距離過近時禁止施放立即技能)
    }
    Dictionary<SpellLock, bool> spellLocker = new Dictionary<SpellLock, bool>();
    /// <summary>
    /// 設定立即技能鎖住清單(有任一項SpellLock被鎖住就不能施放立即技能)
    /// </summary>
    public void SetInstantSkillLocker(SpellLock _type, bool _lock) {
        //WriteLog.Log($"技能鎖({_type}: {_lock})");
        spellLocker[_type] = _lock;
        bool canSpellInstantSkill = CanSpellInstantSkill;
        foreach (var btn in SkillBtns) {
            btn.SetLockerIcon(canSpellInstantSkill);
        }
    }



    int CastingSkillPos; //正在施放技能的按鈕位置

    //[HeaderAttribute("==============Test==============")]
    //[SerializeField] bool TriggerCastMeleeSkill = false;
    //int[] fakeSkills = new int[] { 1001, 1003, 1004, 1005, 1006 };
    //int fakeIndex = 0;

    [Header("Settings")]
    private bool _isSpellTest;

    public static BattleSceneUI Instance;

    // 收到SetPlayer封包後先暫存資料，因為此時可能還沒跑Init不能使用BattleUI.Instance
    static PackGladiator myGladiator;
    static PackGladiator opponentGladiator;
    static int[] handSKillIDs;
    public static void InitPlayerData(PackGladiator _myGladiator, PackGladiator _opponentGladiator, int[] _handSKillIDs) {
        myGladiator = _myGladiator;
        opponentGladiator = _opponentGladiator;
        handSKillIDs = _handSKillIDs;
    }

    /*
    void Update() {
        if (TriggerCastMeleeSkill) {
            TriggerCastMeleeSkill = false;
            CastMeleeSkill(fakeSkills[fakeIndex]);
            fakeIndex = fakeIndex < fakeSkills.Length - 1 ? fakeIndex++ : 0;
        }
    }
    */

    private void Start() {
        Init();
    }

    protected override void SetInstance() {
        Instance = this;
    }
    public override void Init() {
        base.Init();
        MyDivineSelectUI.Init();
        SpawnBattleManager();
        InitGladiator(true, myGladiator.MaxHP, myGladiator.CurHp, myGladiator.JsonID);
        InitGladiator(false, opponentGladiator.MaxHP, opponentGladiator.CurHp, opponentGladiator.JsonID);
        SetSkillDatas(handSKillIDs, 0);
        CheckVigor(0f);
        BattleKO.gameObject.SetActive(false);
    }

    /// <summary>
    /// 更新技能資料
    /// </summary>
    /// <param name="_handSKillIDs">手牌技能</param>
    /// <param name="_skillOnID">選中技能(近戰啟動中)</param>
    public void SetSkillDatas(int[] _handSKillIDs, int _skillOnID) {
        //這邊要+1是因為有一個是nextSkill
        if (_handSKillIDs.Length != SkillBtns.Length + 1) {
            WriteLog.LogError("_handSKillIDs封包格式錯誤");
            return;
        }
        //接收封包並設定按鈕的技能資料
        for (int i = 0; i < SkillBtns.Length; i++) {
            var jsonSkill = GameDictionary.GetJsonData<JsonSkill>(_handSKillIDs[i]);
            SkillBtns[i].SetData(jsonSkill, true);
            SkillBtns[i].SetSkillOn(_handSKillIDs[i] == _skillOnID);
            MyBattleStaminaObj.SetSkillVigorVal(i, jsonSkill != null ? jsonSkill.Vigor : 0);
            //TODO:目前體力預設值是5 這裡先判斷一次預設值 但日後應該添加一個判斷是否為初始化的旗標 包含技能起始狀態都要判斷
            MyBattleStaminaObj.SetVigorMaskBrightness(i, jsonSkill != null ? jsonSkill.Vigor < 5 : false);
        }
        var nextJsonSkill = GameDictionary.GetJsonData<JsonSkill>(_handSKillIDs[_handSKillIDs.Length - 1]);
        NextSkillBtn.SetData(nextJsonSkill);
        SetNextSkillVigorCost(nextJsonSkill != null ? nextJsonSkill.Vigor : 0);
    }

    public int GetHandSkillId(BattleSkillButton _btn) {
        for (int i = 0; i < SkillBtns.Length; i++) {
            if (SkillBtns[i] == _btn)
                return handSKillIDs[i];
        }
        return 0;
    }


    /// <summary>
    /// 更新玩家金錢
    /// </summary>
    /// <param name="gold">金錢</param>
    public void UpdatePlayerGold(int gold) {
        PlayerGoldText.text = gold.ToString();
    }

    /// <summary>
    /// 更新角鬥士資訊
    /// </summary>
    /// <param name="self">是否為己方角鬥士</param>
    /// <param name="maxHP">最大血量</param>
    /// <param name="curHP">目前血量</param>
    /// <param name="heroID">英雄ID</param>
    public void InitGladiator(bool self, int maxHP, int curHP, int heroID) {
        BattleGladiatorInfo target = self ? PlayerGladiatorInfo : EnemyGladiatorInfo;
        target.Init(maxHP, curHP, heroID);
    }

    /// <summary>
    /// 更新角鬥士血量
    /// </summary>
    /// <param name="self">是否為己方角鬥士</param>
    /// <param name="_addHP">目前血量</param>
    public void UpdateGladiatorHP(string _playerID, int _addHP) {
        var playerDoc = GamePlayer.Instance.GetDBData<DBPlayer>();
        //WriteLog.LogError("_playerID=" + _playerID + " playerDoc.ID=" + playerDoc.ID + "  _addHP=" + _addHP);
        if (_playerID == playerDoc.ID) PlayerGladiatorInfo.AddHP(_addHP);
        else EnemyGladiatorInfo.AddHP(_addHP);
    }

    void SpawnBattleManager() {
        AddressablesLoader.GetPrefabByRef(BattleManagerAsset, (battleManagerPrefab, handle) => {
            GameObject go = Instantiate(battleManagerPrefab);
            var battleMaanger = go.GetComponent<BattleManager>();
            battleMaanger.Init().Forget();
        });
    }

    public override void RefreshText() {

    }

    //TODO:
    //1.秒數倒數 數到0送出戰鬥結束(已完成)
    //2.體力條扣除與自動恢復數值演出
    //3.衝刺操作與演出 做兩種 一種為長壓操作方式 另一種為ON/OFF方式 設參數切換模式(已完成)
    //4.設定按鈕點擊開啟設定介面製作
    //5.技能點擊演出 設參數供調整測試
    //6.血條演出 設參數供調整測試(已完成)

    //更新剩餘秒數
    public void SetTimeText(int num) {
        BattleLeftTime.text = num.ToString();
    }

    /*舊版純前端邏輯 預留當時純前端演出 前端發生碰撞就從這裡直接要求作演出 保留當作參考 by 瑞榮2024.9.18
    public void CastMeleeSkill(JsonSkill _skill) {
        //UI作演出 並把技能使用掉 更新下一個技能上來
        if (SkillBtn1.SkillSelected)
            SkillBtn1.CastMeleeSkill(_skill);
        else if (SkillBtn2.SkillSelected)
            SkillBtn2.CastMeleeSkill(_skill);
        else if (SkillBtn3.SkillSelected)
            SkillBtn3.CastMeleeSkill(_skill);
    }
    */

    public void SetDivineSkillData(int[] _skillIDs) {
        if (_skillIDs == null || _skillIDs.Length != 2) { WriteLog.Log("神祇技能資料遺失!"); return; }
        DivineSkills[0].SetData(_skillIDs[0]);
        DivineSkills[1].SetData(_skillIDs[1]);
    }

    /// <summary>
    /// 初始化體力值
    /// </summary>
    /// <param name="curVal">目前數值</param>
    /// <param name="maxVal">最大值</param>
    public void InitVigor(float curVal, float maxVal) {
        MyBattleStaminaObj.InitVigor(curVal, maxVal);
        CheckVigor(curVal);
    }

    /// <summary>
    /// 同步體力數值
    /// </summary>
    /// <param name="val">數值</param>
    public void SetVigor(float val) {
        MyBattleStaminaObj.SetVigor(val);
        CheckVigor(val);
    }

    /// <summary>
    /// 檢查是否能量足夠
    /// </summary>
    /// <param name="val">能量值</param>
    public void CheckVigor(float val) {
        SprintBtn.CheckVigor(val);
        for (int i = 0; i < SkillBtns.Length; i++) {
            SkillBtns[i].SetEnergy(val);
        }
    }

    /// <summary>
    /// 戰鬥技能按鈕設定體力消耗值能量狀態(能量足夠/不足)
    /// </summary>
    /// <param name="_btn">技能按鈕</param>
    /// <param name="energyEnough">能量足夠為True 否則為false</param>
    public void BattleSkillBtnSetVigorEnergyState(BattleSkillButton _btn, bool energyEnough) {
        for (int i = 0; i < SkillBtns.Length; i++) {
            if (_btn == SkillBtns[i]) {
                MyBattleStaminaObj.SetVigorMaskBrightness(i, energyEnough);
                break;
            }
        }
    }

    /// <summary>
    /// 戰鬥技能按鈕能量足夠其他相關演出
    /// </summary>
    /// <param name="_btn">要演出的技能按鈕</param>
    public void BattleSkillBtnAvailable(BattleSkillButton _btn) {
        //BattleSkillButton的AniState變成Available時呼叫
        for (int i = 0; i < SkillBtns.Length; i++) {
            if (_btn == SkillBtns[i]) {
                MyBattleStaminaObj.BrigtenMask(i);
                break;
            }
        }
    }

    /// <summary>
    /// 戰鬥技能按鈕釋放技能體力值消耗演出
    /// </summary>
    /// <param name="_consumeVigor"></param>
    public void BattleSkillBtnCastStaminaConsume(int _consumeVigor) {
        MyBattleStaminaObj.ConsumeVigorBySkill(_consumeVigor);
    }

    /// <summary>
    /// 戰鬥技能按鈕釋放技能隱藏對應技能位置的體力消耗值
    /// </summary>
    /// <param name="_btn">要演出的技能按鈕</param>
    public void BattleSkillBtnCastHideVigorVal(BattleSkillButton _btn) {
        //BattleSkillButton的ModelCastSkill呼叫開始做演出
        for (int i = 0; i < SkillBtns.Length; i++) {
            if (_btn == SkillBtns[i]) {
                MyBattleStaminaObj.FadeOutSkillVigorVal(i);
                break;
            }
        }
    }

    /// <summary>
    /// 更新技能消耗能量
    /// </summary>
    /// <param name="_btn">更換技能的按鈕物件</param>
    /// <param name="_val">能量消耗值</param>
    public void SetSKillVigorCost(BattleSkillButton _btn, int _val) {
        for (int i = 0; i < SkillBtns.Length; i++) {
            if (_btn == SkillBtns[i]) {
                MyBattleStaminaObj.SetSkillVigorVal(i, _val);
                MyBattleStaminaObj.FadeInSkillVigorVal(i);
                break;
            }
        }
    }

    /// <summary>
    /// 設定下一技能的能量消耗值
    /// </summary>
    /// <param name="_val"></param>
    public void SetNextSkillVigorCost(int _val) {
        MyBattleStaminaObj.SetSkillVigorVal(4, _val);
    }

    /// <summary>
    /// 施放技能
    /// </summary>
    /// <param name="_btn">發動技能的按鈕物件</param>
    /// <param name="_skillId">該按鈕的技能ID</param>
    public void CastingInstantSKill(BattleSkillButton _btn, int _skillId) {
        //施放技能後鎖定 不能連續施放技能
        SetInstantSkillLocker(SpellLock.Casting, true);
        //找出釋放技能的按鈕位置
        for (int i = 0; i < SkillBtns.Length; i++) {
            if (SkillBtns[i] == _btn) {
                CastingSkillPos = i;
                break;
            }
        }
        AllocatedRoom.Instance.ActiveSkill(_skillId, true);
        WriteLog.LogWarningFormat("施放立即技能! 正在施展技能中 上鎖! 要求按鈕: {0} 技能ID: {1}", _btn.name, _skillId);
    }

    /// <summary>
    /// 技能施放後要短暫鎖住X秒
    /// </summary>
    async UniTaskVoid castingTmpLock() {
        SetInstantSkillLocker(SpellLock.Casting, true);
        //鎖定0.5秒後才能放下一個技能
        await UniTask.WaitForSeconds(0.5f);
        SetInstantSkillLocker(SpellLock.Casting, false);
    }

    /// <summary>
    /// 確認是否有已選上的近戰技能
    /// </summary>
    /// <returns>True if any skill is selected.</returns>
    public bool CheckSelectedMeleeExist() {
        for (int i = 0; i < SkillBtns.Length; i++) {
            if (SkillBtns[i].SkillSelected)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 施展立即技能後接到Server回傳 開始施放技能
    /// </summary>
    /// <param name="_skillId">下一個技能ID</param>
    public void CastInstantSkill(int _skillId) {
        SkillBtns[CastingSkillPos].CastInstantSkill(NextSkillBtn.GetNextSkillId());
        NextSkillBtn.CacheSkillId(_skillId);
        castingTmpLock().Forget();
    }

    /// <summary>
    /// 釋放肉搏技能
    /// </summary>
    /// <param name="_skillId">下一個技能ID</param>
    public void CastMeleeSkill(int _skillId) {
        //遍歷技能按鈕找到現在On的按鈕
        for (int i = 0; i < SkillBtns.Length; i++) {
            if (SkillBtns[i].SkillSelected) {
                SkillBtns[i].CastMeleeSkill(NextSkillBtn.GetNextSkillId());
                break;
            }
        }
        NextSkillBtn.CacheSkillId(_skillId);
        castingTmpLock().Forget(); // 技能施放後要短暫鎖住X秒
    }

    /// <summary>
    /// 通知NextSkillBtn演出換技能
    /// </summary>
    public void NextSkillBtnChangeSkill() {
        NextSkillBtn.ChangeSkill();
    }

    public void PlayKO(Action afterKO) {
        DoKOAni(afterKO).Forget();
    }

    async UniTask DoKOAni(Action afterKO) {
        BattleKO.gameObject.SetActive(true);
        BattleKO.Play("KO Animation", -1, 0f);
        WriteLog.LogFormat("播放KO動畫 動畫時間長度: {0} 等待時間長度: {1}", 1.35f, 2f);
        await UniTask.WaitForSeconds(2f);
        afterKO();
    }

    /// <summary>
    /// 播放開場動畫
    /// </summary>
    public void StartBattle() {
        BattleFightObj.StartBattle();
    }

    /// <summary>
    /// 獲得金幣
    /// </summary>
    /// <param name="money">金幣數量</param>
    public void AddMoney(int money) {
        MoneyObj.AddMoney(money);
    }

}
