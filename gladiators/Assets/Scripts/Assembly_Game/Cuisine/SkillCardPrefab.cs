using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Gladiators.Main;
using Scoz.Func;

namespace Gladiators.Cuisine {
    public class SkillCardPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        public Image skillIcon;
        public Image highlightFrame; // 拖曳到可以替換範圍時，高亮用
        public CanvasGroup canvasGroup; // 拖曳時改變透明度用

        public JsonSkill MyData;

        // 是否已被使用(拖曳過)的狀態
        private bool isUsed = false;

        public void Set(JsonSkill _data) {
            MyData = _data;
            AddressablesLoader.GetSpriteAtlas("SpellIcon", atlas => {
                skillIcon.sprite = atlas.GetSprite(MyData.ID.ToString());
            });
            highlightFrame.enabled = false;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            // 如果該技能卡已被用過，就不允許再拖
            if (isUsed) {
                eventData.pointerDrag = null; // 取消拖曳
                return;
            }

            // 開始拖曳時，告知 GainSkillUI
            GainSkillUI.Instance.OnSkillBeginDrag(this, eventData);

            // 略微淡出(看起來有被選取拖曳感)
            if (canvasGroup != null)
                canvasGroup.alpha = 0.6f;
        }

        public void OnDrag(PointerEventData eventData) {
            if (isUsed) return;

            // 拖曳過程，更新拖曳路徑效果
            GainSkillUI.Instance.OnSkillDragging(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (isUsed) return;

            // 結束拖曳
            GainSkillUI.Instance.OnSkillEndDrag(this, eventData);

            // 還原顯示
            if (canvasGroup != null)
                canvasGroup.alpha = 1f;
        }

        public void SetUsed(bool used) {
            isUsed = used;
        }
    }

}