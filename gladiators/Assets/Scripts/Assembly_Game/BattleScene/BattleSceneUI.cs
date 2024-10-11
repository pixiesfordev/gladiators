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

public class BattleSceneUI : BaseUI {
    [HeaderAttribute("==============AddressableAssets==============")]
    [SerializeField] AssetReference BattleManagerAsset;

    [HeaderAttribute("==============UI==============")]
    [SerializeField] BattleStaminaObj MyBattleStaminaObj;
    [SerializeField] DivineSelectUI MyDivineSelectUI;
    //TODO:考慮是否之後改腳本產生物件
    //上方角鬥士資訊
    [SerializeField] BattleGladiatorInfo PlayerGladiatorInfo;
    [SerializeField] BattleGladiatorInfo EnemyGladiatorInfo;

    [SerializeField] Text PlayerGoldText;//玩家持有金錢

    [SerializeField] MyTextPro BattleLeftTime;//戰鬥剩餘時間


    //技能手牌
    [SerializeField] BattleSkillButton[] SkillBtns;

    [SerializeField] BattleDivineSkill[] DivineSkills;//神址卡牌

    [SerializeField] BattleSprintButton SprintBtn;//衝刺按鈕

    [SerializeField] GameObject SettingBtn;//設定按鈕



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

    private void Start() {
        Init();
    }
    public override void Init() {
        base.Init();
        MyDivineSelectUI.Init();
        SpawnBattleManager();
        Instance = this;
        InitGladiator(true, myGladiator.MaxHP, myGladiator.CurHp, myGladiator.JsonID);
        InitGladiator(false, opponentGladiator.MaxHP, opponentGladiator.CurHp, opponentGladiator.JsonID);
        SetSkillDatas(handSKillIDs, 0);
        CheckVigor(0f);
    }

    /// <summary>
    /// 更新技能資料
    /// </summary>
    /// <param name="_handSKillIDs">手牌技能</param>
    /// <param name="_handOnID">選中技能(近戰啟動中)</param>
    public void SetSkillDatas(int[] _handSKillIDs, int _skillOnID) {
        if (_handSKillIDs.Length != SkillBtns.Length) {
            WriteLog.LogError("_handSKillIDs封包格式錯誤");
            return;
        }
        //接收封包並設定按鈕的技能資料
        for (int i = 0; i < SkillBtns.Length; i++) {
            var jsonSkill = GameDictionary.GetJsonData<JsonSkill>(_handSKillIDs[i]);
            SkillBtns[i].SetData(jsonSkill);
            SkillBtns[i].SetSkillOn(_handSKillIDs[i] == _skillOnID);
        }
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
        var playerDoc = GamePlayer.Instance.GetDBPlayerDoc<DBPlayer>();
        WriteLog.LogError("_playerID=" + _playerID + " playerDoc.ID=" + playerDoc.ID + "  _addHP=" + _addHP);
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

    public void CheckVigor(float val) {
        SprintBtn.CheckVigor(val);
    }
}
