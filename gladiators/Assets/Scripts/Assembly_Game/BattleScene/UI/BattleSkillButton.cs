using System;
using Gladiators.Battle;
using Gladiators.Main;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戰鬥技能按鈕
/// </summary>
public class BattleSkillButton : MonoBehaviour {

    [SerializeField] Image SkillIcon;
    [SerializeField] Button Btn;
    [SerializeField] Material SKillIconMaterial;
    [SerializeField] Animator BtnAni;

    [HeaderAttribute("==============TEST==============")]
    [Tooltip("使用技能外移位置")][SerializeField] Vector3 UsedSkillMoveOutPosition;
    [Tooltip("使用技能外移所需時間")][SerializeField] float UsedSkillMoveOutTime = 1f;
    //[Tooltip("")][SerializeField] ;

    JsonSkill SkillData;
    public bool SkillSelected { get; private set; } = false; //技能被選上 等待觸發 碰撞觸發技能使用

    //TODO:之後有足夠時間改設計成陣列 根據演出需求動態添加材質球 不能發生材質球共用的情況 >> 先用簡單寫死的方式 看有幾個物件會用到就建立幾個
    Material _cloneMaterial;

    //TODO:
    //1.技能分類 直接釋放/碰撞釋放(enum的BattleSkillType)
    //2.碰撞釋放牌點一次ON 再點一次off ON的情況下要顯示預扣除體力與發光外框淡入淡出
    //3.直接釋放牌點了就直接演出
    //4.補牌演出

    //TODO:演出詳細流程 >> 新富說可能演出還會改 我先去串資料好了?
    //1.使用掉的牌:往外位移同時淡出(可調整參數 外移最終位置 外移所需時間 補牌起始移入位置 補牌移入所需時間)
    //2.補牌:補到使用掉的牌的位置(可調整參數 等待多久後開始位移補入 淡出所需時間 這裡注意不要真的改物件 因為補充的牌不會掛按鈕)
    //3.肉搏牌ON/OFF演出(外框加金框的圖做淡入淡出)

    /*
    材質球相關
    Insufficient Energy_Normal >> 材質球掛在Icon上
    Insufficient energy_rotation >> 程式製作 動畫只是一個示意圖 要去加白色底圖 這樣才能做轉圈fill方式
    Insufficient energy_click >> 材質球額外掛一個圖層物件 點擊就打開顯示 不要掛Icon 這樣狀態太複雜了 一定會亂
    available >> 材質球掛在Icon上
    */

    /*
    技能演出動畫相關
    1.要用成狀態機器(動畫本來就是用狀態機器控 但這裡沒有要搭配Transition 所以狀態直接寫在腳本內)
    2.狀態一覽:
      1.avaliable
      2.Change skills
      3.Enough energy_click
      4.Enough energy_Normal
      5.insufficient Energy_click
      6.insufficient Energy_Normal
      7.insufficient Energy_rotation
      8.Start_Cancel
      9.Start_cast
      10.Start_Scale
      11.Start_Vibrate
    3.合併完上傳後整理名稱統一大小寫+移除空格 讓狀態列舉可以直接對上名稱方便管理維護
    4.Insufficient Energy_Normal掛材質球處理
    5.Insufficient energy_rotation要額外處理 動畫不要使用 只是示意圖 要用程式去控制 黑色區域是Icon掛材質球 白色圖層要額外掛材質球
    6.Insufficient energy_click要額外做縮放 動畫不要使用 只有示意圖 要掛材質球(額外多掛一個Icon物件 不要用Icon去表演 會很難控)
    7.avaliable演出完狀態接到Enough_energy_Normal 要掛材質球 不過要考慮演出到一半又因為使用技能而跳回不能用轉圈的情況
    8.Start_Scale要掛材質球演出
    9.Start_Vibrate要掛材質球演出
    10.Start_Cancel要掛材質球演出
    11.Start_Cast演出完狀態接到change Skills 要掛材質球演出
    12.change Skills要掛材質球演出 演出完後根據下一張技能卡與目前能量值接回insufficient Energy_Normal或Enough energy_Normal
    13.SkillButton4(Next)要掛材質球 直接掛著就好 這個按鈕演出只有淡入淡出 在其他技能按鈕演出Change Skill的時候同時淡入淡出換圖
    14.設定測試情境 這個會相當複雜 功能完成後再進行測試與設想 可以配流程圖
    15.時間足夠的話 重新畫一個流程圖 目前文件上的流程還有一些缺稀(這部分可以畫在Animator)
    */

    void Start()
    {
        //TODO:初始化演出用材質球副本 避免每次演出一直產生新的object
        //_cloneMaterial = Instantiate(SKillIconMaterial);
    }

    /// <summary>
    /// 設定技能資料
    /// </summary>
    /// <param name="_skill">技能資料</param>
    public void SetData(JsonSkill _skill) {
        SkillData = _skill;
        Debug.LogFormat("技能物件:{0}設定技能資料! 技能ID: {1}", gameObject.name, SkillData.ID);
    }

    public void SetSkillOn(bool _on) {
        if (SkillData == null) return;
        SkillSelected = _on;
    }


    //點擊施放技能
    public void ClickBtn() {
        //判斷技能是否存在
        if (SkillData == null) {
            Debug.LogError("Skill Data is null!");
            return;
        }
        //送封包給後端
        AllocatedRoom.Instance.ActiveSkill(SkillData.ID, true);
        /* 舊版 先註解 確定完成正式邏輯後刪除 先保留做為參考
        //判斷技能類型
        if (SkillData.Activation.Equals(SkillActivation.Instant))
        {
            //直接觸發類
            BattleManager.Instance.CastInstantSKill(SkillData);
        }
        else if (SkillData.Activation.Equals(SkillActivation.Melee))
        {
            //碰撞觸發類 把技能傳給BattleManager存放
            SkillSelected = !SkillSelected;
            BattleManager.Instance.SetMeleeSkill(SkillData, SkillSelected);
        }
        else
        {
            Debug.LogErrorFormat("Unknown Battle Skill Type. String: " + SkillData.Activation);
            return;
        }
        */
    }

    public void CastMeleeSkill(JsonSkill _skill) {
        SkillSelected = false;
        //TODO:
        //作演出並替換下一個技能上來
    }
}