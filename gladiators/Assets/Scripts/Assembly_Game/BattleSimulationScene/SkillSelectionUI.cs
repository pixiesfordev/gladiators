using Gladiators.Main;
using Scoz.Func;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace Gladiators.BattleSimulation {

    public class SkillSelectionUI : ItemSpawner_Remote<Selection_Skill> {

        public static SkillSelectionUI Instance { get; private set; }
        public override void RefreshText() {
        }

        protected override void SetInstance() {
            Instance = this;
        }

        public override void Init() {
            base.Init();
            LoadItemAsset();
        }

        public void ShowUI(List<JsonSkill> _jsons, int[] _curSkillIDs) {
            SetActive(true);
            if (!LoadItemFinished) {
                WriteLog.LogError("ItemAsset尚未載入完成");
                return;
            }
            InActiveAllItem();
            if (_jsons == null || _jsons.Count == 0) {
                WriteLog.LogError("傳入的_jsons為空或長度為0");
                return;
            }
            for (int i = 0; i < _jsons.Count; i++) {
                if (i < ItemList.Count) {
                    ItemList[i].SetItem(_jsons[i]);
                    ItemList[i].IsActive = true;
                    ItemList[i].gameObject.SetActive(true);

                } else {
                    var item = Spawn();
                    item.SetItem(_jsons[i]);
                }
                // 已經有的技能不能重複選
                ItemList[i].Btn.interactable = !_curSkillIDs.Contains(_jsons[i].ID);
                // 加入按鈕事件
                var idx = i;
                ItemList[i].Btn.onClick.AddListener(() => {
                    SimulationUI.Instance.UpdateSkill(_jsons[idx]);
                    SetActive(false);
                });
            }
        }

    }
}
