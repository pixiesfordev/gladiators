using Gladiators.Battle;
using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleSceneUI : BaseUI {
    [HeaderAttribute("==============AddressableAssets==============")]
    [SerializeField] AssetReference BattleManagerAsset;

    [HeaderAttribute("==============UI==============")]
    //TODO:考慮是否之後改腳本產生物件
    //上方角鬥士資訊
    [SerializeField] BattleGladiatorInfo PlayerGladiatorInfo;
    [SerializeField] BattleGladiatorInfo EnemyGladiatorInfo;

    [SerializeField] MyTextPro BattleLeftTime;//戰鬥剩餘時間

    //技能牌
    [SerializeField] BattleSkillButton SkillBtn1;
    [SerializeField] BattleSkillButton SkillBtn2;
    [SerializeField] BattleSkillButton SkillBtn3;
    [SerializeField] BattleSkillButton NextSkillBtn;

    [SerializeField] Image StaminaBar;//使用技能體力條

    [SerializeField] BattleSprintButton SprintBtn;//衝刺按鈕

    [SerializeField] GameObject SettingBtn;//設定按鈕


    [Header("Settings")]
    private bool _isSpellTest;

    public static BattleSceneUI Instance;

    private void Start() {
        Init();
    }
    public override void Init() {
        base.Init();
        SpawnBattleManager();
        Instance = this;
    }

    void SpawnBattleManager() {
        AddressablesLoader.GetPrefabByRef(BattleManagerAsset, (battleManagerPrefab, handle) => {
            GameObject go = Instantiate(battleManagerPrefab);
            var battleMaanger = go.GetComponent<BattleManager>();
            battleMaanger.Init();
        });
    }

    public override void RefreshText() {

    }

    //TODO:
    //1.秒數倒數 數到0送出戰鬥結束
    //2.體力條扣除與自動恢復數值與演出
    //3.衝刺操作與演出 做兩種 一種為長壓操作方式 另一種為ON/OFF方式 設參數切換模式
    //4.前端暫時計算邏輯預計先寫在BattleManager
    //5.設定按鈕點擊開啟設定介面製作

    //更新剩餘秒數
    public void SetTimeText(int num)
    {
        BattleLeftTime.text = num.ToString();
    }
}
