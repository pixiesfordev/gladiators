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

    [HeaderAttribute("==============TEST==============")]
    [Tooltip("使用技能外移位置")][SerializeField] Vector3 UsedSkillMoveOutPosition;
    [Tooltip("使用技能外移所需時間")][SerializeField] float UsedSkillMoveOutTime = 1f;
    //[Tooltip("")][SerializeField] ;

    JsonSkill SkillData;
    public bool SkillSelected { get; private set; } = false; //技能被選上 等待觸發 碰撞觸發技能使用

    //TODO:
    //1.技能分類 直接釋放/碰撞釋放(enum的BattleSkillType)
    //2.碰撞釋放牌點一次ON 再點一次off ON的情況下要顯示預扣除體力與發光外框淡入淡出
    //3.直接釋放牌點了就直接演出
    //4.補牌演出

    //TODO:演出詳細流程 >> 新富說可能演出還會改 我先去串資料好了?
    //1.使用掉的牌:往外位移同時淡出(可調整參數 外移最終位置 外移所需時間 補牌起始移入位置 補牌移入所需時間)
    //2.補牌:補到使用掉的牌的位置(可調整參數 等待多久後開始位移補入 淡出所需時間 這裡注意不要真的改物件 因為補充的牌不會掛按鈕)
    //3.肉搏牌ON/OFF演出(外框加金框的圖做淡入淡出)

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