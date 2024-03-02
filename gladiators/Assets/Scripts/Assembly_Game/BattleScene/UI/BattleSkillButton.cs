using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戰鬥技能按鈕
/// </summary>
public class BattleSkillButton : MonoBehaviour {
    
    [SerializeField] Image SkillIcon;
    [SerializeField] Button Btn;
    

    //TODO:
    //1.技能分類 遠/近/治療
    //2.近戰牌點一次ON 再點一次off ON的情況下要顯示預扣除體力
    //3.遠程/治療點了直接釋放
    //4.用完牌後補牌的演出
}