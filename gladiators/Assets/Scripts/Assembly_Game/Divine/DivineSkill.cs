using System;
using Gladiators.Main;
using UnityEngine;
using UnityEngine.UI;
using Scoz.Func;

namespace Gladiators.Battle {
    public class DivineSkill : MonoBehaviour
    {
        [SerializeField] Image SkillIcon;
        [SerializeField] Text Cost;
        [SerializeField] Text Info;
        [SerializeField] Image BG;
        [SerializeField] Button Btn;
        [SerializeField] Animator FloatingAni; //浮動動畫 常態演出
        [SerializeField] Animator EffectAni; //其他演出動畫

        JsonSkill SkillData;

        /*
        加入動畫演出
        Controller調整 >> SkillArea砍掉 改用DivineSkillAni(還未編輯 需要修改裡面的Clip) 浮動用DivineSkillFloat(還未編輯 需要修改裡面的Clip)
        註1:一個GameObject上只能掛一個Animator 所以可能需要調整 看是要多加一層GameObject還是怎樣
        註2:Animator的Clip可能都要重拉 因為新富是直接拉最上面那層 我技能命名有分1 2 3 4 這樣除了2以外其他三個都吃不到 因為名稱不同
            其實可以上層加一個GameObject就好 但這樣長久來看不OK 因為命名會混淆 所以還是得修正起來
            屆時製作的時候先複製一個副本 確保新的Animator都跟舊的演出一樣後再把舊的砍掉(建立新的Controller Clip也重新拉)

        Relics_ Normal >> 一般狀態 這個應該不會使用到 這只是還原原本狀態
        Relics_floating >> 神址牌飄動 這個要獨立拉一個Animator 然後設定一個隨機起始值讓撥放看起來錯落有致 動畫本身得是循環撥放
        Relics_Click >> 點擊撥放此動畫(其實就跟單純變色一樣)
        Relics_bigger >> 點完確定可以選之後跳此演出
        Relics_bigger Normal >> 放大演出完之後設定為此狀態
        Relics_Cancelled >> 已選取卡牌再點一次撥放 表示取消選擇
        ruins_decision_selection >> 點下決定按鈕後 被選上者撥放此動畫
        Ruins_Decision_Not Selected >> 點下決定按鈕後 未被選上者撥放此動畫
        */

        public void SetData(JsonSkill _skill)
        {
            //設定技能資料
            SkillData = _skill;
            
            if (SkillData != null) {
                //設定金額(cost)
                Cost.text = SkillData.Cost.ToString();
                //設定技能說明(之後補)
                if (!string.IsNullOrEmpty(SkillData.Ref)) {
                    SkillIcon.gameObject.SetActive(true);
                    //設定SkillIcon
                    AssetGet.GetSpriteFromAtlas("SkillIcon", SkillData.Ref, (sprite) => {
                        SkillIcon.sprite = sprite;
                    });
                } else {
                    SkillIcon.gameObject.SetActive(false);
                }
            }
            Debug.Log("技能物件: " + gameObject.name + " 設定技能資料! 技能ID: " + SkillData.ID);
        }

        public void ClickBtn()
        {
            //TODO:改成撥放點擊動畫
            //選擇技能
            if (DivineSelectUI.Instance != null)
            {
                bool selected = DivineSelectUI.Instance.SelectDivineSkill(SkillData);
                //先以背景變色代表選上/未選上
                BG.color = selected ? Color.yellow : Color.white;
            }
        }

        //TODO:撰寫動畫撥放公開接口讓外部呼叫
    }
}
