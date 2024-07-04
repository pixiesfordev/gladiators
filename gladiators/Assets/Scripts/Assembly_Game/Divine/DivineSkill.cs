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

        JsonSkill SkillData;

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
            //選擇技能
            if (DivineSelectUI.Instance != null)
            {
                bool selected = DivineSelectUI.Instance.SelectDivineSkill(SkillData);
                //先以背景變色代表選上/未選上
                BG.color = selected ? Color.yellow : Color.white;
            }
        }    
    }
}
