using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gladiators.Socket.Matchgame;
using Scoz.Func;
using Gladiators.Main;

public class BattleDivineSkill : MonoBehaviour
{
    //TODO:這裡介面尚未確定要怎麼排 先暫時用選擇時一樣的白版介面拚上去
    [SerializeField] Image SkillIcon;
    [SerializeField] Text Cost;
    [SerializeField] Text Info;
    [SerializeField] Image BG;
    [SerializeField] Button Btn;

    PackBribeSkill PackData;//同步資料

    void Start() { }

    public void SetData(PackBribeSkill _skill)
    {
        if (_skill == null)
        {
            gameObject.SetActive(false);
            WriteLog.LogErrorFormat("空的神祉技能");
        }
        else
        {
            PackData = _skill;
            WriteLog.LogFormat("神祉技能設定. ID:{0} ", PackData.JsonID);

            JsonSkill SkillData = GameDictionary.GetJsonData<JsonSkill>(PackData.JsonID);
            if (SkillData != null && SkillData.MySkillType == SkillType.Divine) {
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
            } else {
                gameObject.SetActive(false);
                WriteLog.LogErrorFormat("神址技能不存在或者不是神址類技能.");
            }
        }
    }

    public void ClickBtn()
    {
        //TODO:使用神祇技能
        WriteLog.LogFormat("使用神祇技能. ID:{0}", PackData.JsonID);
    } 
}
