using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Gladiators.Cuisine;
using UnityEngine.AddressableAssets;
using Scoz.Func;
using Cysharp.Threading.Tasks;
using System;

namespace Gladiators.Main {
    public class GainSkillUI : BaseUI {
        public static GainSkillUI Instance { get; private set; }
        Canvas mainCanvas;
        [SerializeField] SkillCardPrefab CardPrefab;
        [SerializeField] Transform TopCardParent;
        List<SkillCardPrefab> topSkillCards = new List<SkillCardPrefab>();
        [SerializeField] List<SkillCardPrefab> botSkillCards;
        [SerializeField] SkillCardPrefab DraggingCard; // 拖曳中的技能

        [SerializeField] GameObject circlePrefab;
        [SerializeField] float circleSpacing = 20f;
        List<GameObject> dragCircles = new List<GameObject>();
        Vector3 dragStartPos;

        public override void Init() {
            base.Init();
            mainCanvas = UICanvas.Instance.MyCanvas;
        }


        public void ShowUI(List<JsonSkill> gainSkills, List<JsonSkill> playerSkills) {
            // 設定可以選擇的技能
            foreach (var card in topSkillCards) {
                Destroy(card);
            }
            foreach (var json in gainSkills) {
                var cardPrefab = Instantiate(CardPrefab, TopCardParent);
                botSkillCards.Add(cardPrefab);
                cardPrefab.Set(json);
            }

            // 設定玩家技能
            for (int i = 0; i < playerSkills.Count; i++) {
                botSkillCards[i].Set(playerSkills[i]);
            }

            DraggingCard.gameObject.SetActive(false);
            SetActive(true);

        }


        public void OnSkillBeginDrag(SkillCardPrefab card, PointerEventData eventData) {
            Vector3 mousePos = Input.mousePosition;
            DraggingCard.transform.position = mousePos;
            DraggingCard.Set(card.MyData);
            dragStartPos = card.transform.position;
            DraggingCard.gameObject.SetActive(true);
        }

        public void OnSkillDragging(SkillCardPrefab card, PointerEventData eventData) {

            Vector2 screenPos = eventData.position;
            RectTransform canvasRect = mainCanvas.GetComponent<RectTransform>();

            Vector2 localPoint;
            bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                                canvasRect,
                                screenPos,
                                mainCanvas.worldCamera,  // 或 null，視情況而定
                                out localPoint
                            );

            RectTransform draggingRect = DraggingCard.GetComponent<RectTransform>();
            draggingRect.anchoredPosition = localPoint;

            // 先清掉圓點
            ClearDragCircles();

            // 生成一系列的小圓點
            float distance = Vector3.Distance(dragStartPos, localPoint);
            int circleCount = Mathf.FloorToInt(distance / circleSpacing);

            for (int i = 0; i < circleCount; i++) {
                float t = (float)i / circleCount;
                Vector3 circlePos = Vector3.Lerp(dragStartPos, localPoint, t);
                GameObject circle = Instantiate(circlePrefab, canvasRect);
                circle.GetComponent<RectTransform>().anchoredPosition = circlePos;

                dragCircles.Add(circle);
            }
        }

        public void OnSkillEndDrag(SkillCardPrefab card, PointerEventData eventData) {
            Vector3 mousePos = Input.mousePosition;

            int targetSlotIndex = GetHoveredBottomSlotIndex(mousePos);
            if (targetSlotIndex != -1) {
                botSkillCards[targetSlotIndex].Set(card.MyData);
                card.SetUsed(true);
            }

            ClearDragCircles();
            DraggingCard.gameObject.SetActive(false);
        }

        private int GetHoveredBottomSlotIndex(Vector3 mousePosition) {
            for (int i = 0; i < botSkillCards.Count; i++) {
                RectTransform slotRect = botSkillCards[i].GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(slotRect, mousePosition)) {
                    return i;
                }
            }
            return -1;
        }

        void ClearDragCircles() {
            foreach (var c in dragCircles) {
                Destroy(c);
            }
            dragCircles.Clear();
        }


        public void OnSkipClicked() {
            WriteLog.Log("跳過");
        }

        public override void RefreshText() {
            Instance.RefreshText();
        }

        protected override void SetInstance() {
            Instance = this;
        }
    }
}

