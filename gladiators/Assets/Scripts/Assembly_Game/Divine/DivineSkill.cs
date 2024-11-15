using System;
using Gladiators.Main;
using UnityEngine;
using UnityEngine.UI;
using Scoz.Func;
using Cysharp.Threading.Tasks;

namespace Gladiators.Battle {
    public class DivineSkill : MonoBehaviour {
        [SerializeField] Image RelicSkill;
        [SerializeField] Text Cost;
        [SerializeField] Text Info;
        [SerializeField] Button Btn;
        [SerializeField] Animator FloatingAni; //浮動動畫 常態演出
        [SerializeField] Animator EffectAni; //其他演出動畫

        JsonSkill SkillData;

        //TODO:
        //RelicsSkill日後要掛回材質球(位置:Asset/Resource/Atlas/DivineUI) Relics skill 

        /*
        加入動畫演出
        Controller調整 >> SkillArea砍掉 改用DivineSkillAni 浮動用DivineSkillFloat
        註1:一個GameObject上只能掛一個Animator 需要多加一層GameObject
        註2:僅供紀錄 Animator的Clip都重拉 因為一開始是直接拉最上面那層 技能命名有分1 2 3 4 這樣除了2以外其他三個都吃不到
            Animator是認路徑名稱的 而且不能修改 一旦修改物件架構就必須重新拉過

        Relics_Normal >> 一般狀態 這個應該不會使用到 這只是還原原本狀態
        Relics_floating >> 神址牌飄動 這個要獨立拉一個Animator 然後設定一個隨機起始值讓撥放看起來錯落有致 動畫本身得是循環撥放
        Relics_Press >> 壓下按鈕(變暗色)
        Relics_Released >> 放開按鈕(變亮色)
        Relics_bigger >> 點完確定可以選之後跳此演出
        Relics_bigger_Normal >> 放大演出完之後設定為此狀態
        Relics_Cancelled >> 已選取卡牌再點一次撥放 表示取消選擇
        Ruins_Decision_Selection >> 點下決定按鈕後 被選上者撥放此動畫
        Ruins_Decision_Not_Selected >> 點下決定按鈕後 未被選上者撥放此動畫
        */

        private void Start () {
            //播放浮動動畫 起始點為隨機 作出錯落有致的效果
            FloatingAni.Play("Relics_floating", -1, UnityEngine.Random.Range(0f, 1f));
        }

        /// <summary>
        /// 設定資料
        /// </summary>
        /// <param name="_skill"></param>
        public void SetData(JsonSkill _skill) {
            //設定技能資料
            SkillData = _skill;

            if (SkillData != null) {
                //設定金額(cost)
                Cost.text = SkillData.Cost.ToString();
                //設定技能說明(之後補)
                RelicSkill.gameObject.SetActive(true);
                //設定Icon
                AssetGet.GetSpriteFromAtlas("Relics skillicon", SkillData.ID.ToString(), (sprite) => {
                    if (sprite != null)
                        RelicSkill.sprite = sprite;
                    else {
                        AssetGet.GetSpriteFromAtlas("Relics skillicon", "Relics_Click_10110", (sprite) => { 
                        RelicSkill.sprite = sprite; 
                        WriteLog.LogWarningFormat("圖片缺少! 用Relics_Click_10110代替顯示! ID: {0}", SkillData.Ref);
                        } );
                    }
                });
            }
            Debug.Log("技能物件: " + gameObject.name + " 設定技能資料! 技能ID: " + SkillData.ID);
        }

        /// <summary>
        /// 按下按鈕事件
        /// </summary>
        public void PressSkill() {
            EffectAni.Play("Relics_Press", -1, 0f);
            //WriteLog.LogErrorFormat("壓下按鈕");
        }

        /// <summary>
        /// 放開按鈕事件
        /// </summary>
        public void ReleasedSkill() {
            EffectAni.Play("Relics_Released", -1, 0f);
            //WriteLog.LogErrorFormat("放開按鈕");
        }

        /// <summary>
        /// 點擊事件
        /// </summary>
        public void ClickSkill() {
            SelectSkill().Forget();
        }

        async UniTaskVoid SelectSkill() {
            await UniTask.WaitForSeconds(0.03f);
            //選擇技能
            if (DivineSelectUI.Instance != null) {
                bool selected = DivineSelectUI.Instance.SelectDivineSkill(SkillData);
                if (selected) {
                    EffectAni.Play("Relics_bigger", -1, 0f);
                    //WriteLog.LogErrorFormat("被選上! 按鈕: {0}", name);
                } else {
                    EffectAni.Play("Relics_Cancelled", -1, 0f);
                    //WriteLog.LogErrorFormat("取消選上! 按鈕: {0}", name);
                }
            }
        }

        /// <summary>
        /// 播放確定按鈕點擊後被選上動畫
        /// </summary>
        public void PlayDecisionSelected() {
            EffectAni.Play("Ruins_Decision_Selection", -1, 0f);
            //WriteLog.LogErrorFormat("播放確定選擇動畫! 按鈕: {0}", name);
        }

        /// <summary>
        /// 播放確定按鈕點擊後未被選上動畫
        /// </summary>
        public void PlayDecisionNotSelected() {
            EffectAni.Play("Ruins_Decision_Not_Selected", -1, 0f);
            //WriteLog.LogErrorFormat("播放確定未被選擇動畫! 按鈕: {0}", name);
        }

        /// <summary>
        /// 動畫設定為未被選上普通狀態
        /// </summary>
        public void SetNormalEvent() {
            //call after Relics_Cancelled
            EffectAni.Play("Relics_Normal", -1, 0f);
            //WriteLog.LogErrorFormat("播放普通狀態. 按鈕: {0}", name);
        }

        /// <summary>
        /// 動畫設定為被選上普通狀態
        /// </summary>
        public void SetBigNormalEvent() {
            //call after Relics_bigger
            EffectAni.Play("Relics_bigger_Normal", -1, 0f);
            //WriteLog.LogErrorFormat("播放放大普通狀態. 按鈕: {0}", name);
        }

        /// <summary>
        /// 取得技能ID
        /// </summary>
        /// <returns>技能ID</returns>
        public int GetSkillDataID() {
            return SkillData != null ? SkillData.ID : 0;
        }
    }
}
