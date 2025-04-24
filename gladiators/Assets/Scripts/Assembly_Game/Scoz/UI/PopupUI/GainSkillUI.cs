using Gladiators.Cuisine;
using Scoz.Func;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gladiators.Main {
    public class GainSkillUI : BaseUI {
        public static GainSkillUI Instance { get; private set; }

        [Header("Prefab & 容器")]
        [SerializeField] SkillCardPrefab Card_Prefab;
        [SerializeField] Transform Trans_TopCardParent;
        [SerializeField] List<SkillCardPrefab> BotSkillCards;

        [Header("拖曳顯示")]
        [SerializeField] SkillCardPrefab Card_Dragging;

        [Header("拖曳小球設定")]
        [SerializeField] GameObject Go_CirclePrefab;
        [SerializeField] float circleSpacing = 10f;
        [SerializeField] float baseArcHeight = 200f;
        [SerializeField] float maxArcDistance = 1000f;
        [SerializeField] int minDotCount = 4;

        [Header("文字")]
        [SerializeField] Text Txt_Title;
        [SerializeField] Text Txt_PlayerSkills;
        [SerializeField] Text Txt_Confirm;
        [SerializeField] Text Txt_Skip;
        [SerializeField] Text Txt_Reset;

        int pickableSkillCount;
        int pickCardCount;
        int replacedCount;
        HashSet<int> replacedSlots = new HashSet<int>();
        List<JsonSkill> originalPlayerSkills = new List<JsonSkill>();

        Canvas mainCanvas;
        Vector2 dragStartScreenPos;
        List<GameObject> dragCircles = new List<GameObject>();
        List<SkillCardPrefab> topSkillCards = new List<SkillCardPrefab>();

        int lastHighlightIdx = -1;

        void Awake() {
            Instance = this;
        }

        public override void Init() {
            base.Init();
            mainCanvas = GetComponentInParent<Canvas>();
            Card_Dragging.gameObject.SetActive(false);
        }

        public override void RefreshText() {
            Txt_Title.text = string.Format(JsonString.GetUIString("GainSkillUI_SelectSkillTitle"), pickableSkillCount, pickCardCount);
            Txt_PlayerSkills.text = JsonString.GetUIString("GainSkillUI_PlayerSkills");
            Txt_Confirm.text = JsonString.GetUIString("GainSkillUI_Confirm");
            Txt_Skip.text = JsonString.GetUIString("GainSkillUI_Skip");
            Txt_Reset.text = JsonString.GetUIString("GainSkillUI_Reset");
        }

        protected override void SetInstance() {
            Instance = this;
        }

        public void ShowUI(List<JsonSkill> _gainSkills, List<JsonSkill> _playerSkills, int _pickCount) {
            pickableSkillCount = _gainSkills.Count;
            pickCardCount = _pickCount;
            replacedCount = 0;
            replacedSlots.Clear();
            lastHighlightIdx = -1;
            originalPlayerSkills = new List<JsonSkill>(_playerSkills);

            foreach (var c in topSkillCards) Destroy(c.gameObject);
            topSkillCards.Clear();
            foreach (var js in _gainSkills) {
                var card = Instantiate(Card_Prefab, Trans_TopCardParent);
                card.Set(js);
                topSkillCards.Add(card);
            }

            for (int i = 0; i < BotSkillCards.Count; i++) {
                if (i < originalPlayerSkills.Count)
                    BotSkillCards[i].Set(originalPlayerSkills[i]);
                BotSkillCards[i].SetState(SkillCardPrefab.CardState.Normal);
            }

            Card_Dragging.gameObject.SetActive(false);
            RefreshText();
            SetActive(true);
        }

        public void OnResetBtnClick() {
            replacedCount = 0;
            replacedSlots.Clear();
            lastHighlightIdx = -1;
            clearDragCircles();
            Card_Dragging.gameObject.SetActive(false);

            foreach (var c in topSkillCards)
                c.SetState(SkillCardPrefab.CardState.Normal);

            for (int i = 0; i < BotSkillCards.Count; i++) {
                if (i < originalPlayerSkills.Count)
                    BotSkillCards[i].Set(originalPlayerSkills[i]);
                BotSkillCards[i].SetState(SkillCardPrefab.CardState.Normal);
            }
        }

        public void OnConfirmBtnClick() { }

        public void OnSkipBtnClick() { }

        public void OnSkillBeginDrag(SkillCardPrefab _card, PointerEventData _e) {
            var canvasRT = mainCanvas.GetComponent<RectTransform>();
            Camera cam = mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCanvas.worldCamera;

            dragStartScreenPos = RectTransformUtility.WorldToScreenPoint(
                cam, _card.GetComponent<RectTransform>().position
            );
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvasRT, dragStartScreenPos, cam, out Vector3 worldPos
            );
            var rt = Card_Dragging.GetComponent<RectTransform>();
            rt.SetParent(canvasRT, false);
            rt.position = worldPos;
            Card_Dragging.Set(_card.MyData);
            Card_Dragging.gameObject.SetActive(true);
        }

        public void OnSkillDragging(SkillCardPrefab _card, PointerEventData _e) {
            var canvasRT = mainCanvas.GetComponent<RectTransform>();
            Camera cam = mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCanvas.worldCamera;

            Vector2 curr = _e.position;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvasRT, curr, cam, out Vector3 worldCurr
            );
            Card_Dragging.GetComponent<RectTransform>().position = worldCurr;

            float d = Vector2.Distance(dragStartScreenPos, curr);
            float r = Mathf.Clamp01(d / maxArcDistance);
            float arc = baseArcHeight * (1f - r);

            clearDragCircles();
            int cnt = Mathf.Max(Mathf.FloorToInt(d / circleSpacing), minDotCount);
            for (int i = 0; i < cnt; i++) {
                float t = i / (float)cnt;
                Vector2 sp = Vector2.Lerp(dragStartScreenPos, curr, t)
                           + Vector2.up * (Mathf.Sin(Mathf.PI * t) * arc);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    canvasRT, sp, cam, out Vector3 wd
                );
                var dot = Instantiate(Go_CirclePrefab, canvasRT);
                dot.GetComponent<RectTransform>().position = wd;
                dragCircles.Add(dot);
            }

            int idx = getHoveredBottomSlotIndex(curr);
            if (idx != lastHighlightIdx) {
                if (lastHighlightIdx >= 0)
                    BotSkillCards[lastHighlightIdx].SetState(SkillCardPrefab.CardState.Normal);

                if (idx >= 0 && !replacedSlots.Contains(idx) && replacedCount < pickCardCount) {
                    BotSkillCards[idx].SetState(SkillCardPrefab.CardState.Hightlight);
                } else {
                    idx = -1;
                }
                lastHighlightIdx = idx;
            }

            Card_Dragging.GetComponent<RectTransform>().SetAsLastSibling();
        }

        public void OnSkillEndDrag(SkillCardPrefab _card, PointerEventData _e) {
            int idx = getHoveredBottomSlotIndex(_e.position);
            if (idx >= 0 && !replacedSlots.Contains(idx) && replacedCount < pickCardCount) {
                BotSkillCards[idx].Set(_card.MyData);
                BotSkillCards[idx].SetState(SkillCardPrefab.CardState.Lock);

                replacedCount++;
                replacedSlots.Add(idx);
                _card.SetUsed();

                if (replacedCount >= pickCardCount) {
                    foreach (var c in topSkillCards) {
                        if (c.CurState != SkillCardPrefab.CardState.Empty)
                            c.SetState(SkillCardPrefab.CardState.Lock);
                    }
                }
            } else {
                _card.SetState(SkillCardPrefab.CardState.Normal);
            }

            if (lastHighlightIdx >= 0) {
                BotSkillCards[lastHighlightIdx].SetState(SkillCardPrefab.CardState.Normal);
                lastHighlightIdx = -1;
            }

            clearDragCircles();
            Card_Dragging.gameObject.SetActive(false);
        }

        int getHoveredBottomSlotIndex(Vector2 _pos) {
            for (int i = 0; i < BotSkillCards.Count; i++) {
                var rt = BotSkillCards[i].GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(rt, _pos, mainCanvas.worldCamera))
                    return i;
            }
            return -1;
        }

        void clearDragCircles() {
            foreach (var d in dragCircles) Destroy(d);
            dragCircles.Clear();
        }
    }
}
