using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Gladiators.Main;
using Scoz.Func;

namespace Gladiators.Cuisine {
    public class SkillCardPrefab : MonoBehaviour,
        IBeginDragHandler, IDragHandler, IEndDragHandler {

        [SerializeField] Image Img_SkillIcon;
        [SerializeField] Image Img_HighlightFrame;
        [SerializeField] Image Img_LockCover;
        [SerializeField] CanvasGroup CG_CanvasGroup;

        public JsonSkill MyData { get; private set; }

        bool isLockedOrHidden = false;

        public enum CardState {
            Normal,      // 一般
            Dragging,    // 拖曳中（半透明、不可互動）
            Lock,        // 鎖住（壓黑遮罩）
            Hightlight,  // 高亮
            Empty        // 隱藏
        }

        public CardState CurState = CardState.Normal;

        void Awake() {
            if (Img_HighlightFrame != null) Img_HighlightFrame.enabled = false;
            if (Img_LockCover != null) Img_LockCover.enabled = false;
        }

        public void Set(JsonSkill _data) {
            MyData = _data;
            AddressablesLoader.GetSpriteAtlas("SpellIcon", _atlas => {
                Img_SkillIcon.sprite = _atlas.GetSprite(MyData.ID.ToString());
            });
            SetState(CardState.Normal);
        }

        public void SetUsed() {
            SetState(CardState.Empty);
        }

        public void SetState(CardState _state) {
            CurState = _state;
            isLockedOrHidden = (_state == CardState.Lock || _state == CardState.Empty);

            if (Img_HighlightFrame != null) Img_HighlightFrame.enabled = false;
            if (Img_LockCover != null) Img_LockCover.enabled = false;

            switch (_state) {
                case CardState.Normal:
                    gameObject.SetActive(true);
                    CG_CanvasGroup.alpha = 1f;
                    CG_CanvasGroup.blocksRaycasts = true;
                    break;

                case CardState.Dragging:
                    gameObject.SetActive(true);
                    CG_CanvasGroup.alpha = 0.5f;
                    CG_CanvasGroup.blocksRaycasts = false;
                    break;

                case CardState.Lock:
                    gameObject.SetActive(true);
                    CG_CanvasGroup.alpha = 1f;
                    CG_CanvasGroup.blocksRaycasts = false;
                    if (Img_LockCover != null) Img_LockCover.enabled = true;
                    break;

                case CardState.Hightlight:
                    gameObject.SetActive(true);
                    CG_CanvasGroup.alpha = 1f;
                    CG_CanvasGroup.blocksRaycasts = true;
                    if (Img_HighlightFrame != null) Img_HighlightFrame.enabled = true;
                    break;

                case CardState.Empty:
                    gameObject.SetActive(false);
                    break;
            }
        }

        public void OnBeginDrag(PointerEventData _eventData) {
            if (isLockedOrHidden) {
                _eventData.pointerDrag = null;
                return;
            }
            SetState(CardState.Dragging);
            GainSkillUI.Instance.OnSkillBeginDrag(this, _eventData);
        }

        public void OnDrag(PointerEventData _eventData) {
            if (isLockedOrHidden) return;
            GainSkillUI.Instance.OnSkillDragging(this, _eventData);
        }

        public void OnEndDrag(PointerEventData _eventData) {
            if (isLockedOrHidden) return;
            GainSkillUI.Instance.OnSkillEndDrag(this, _eventData);
            if (CurState != CardState.Empty)
                SetState(CardState.Normal);
        }
    }
}
