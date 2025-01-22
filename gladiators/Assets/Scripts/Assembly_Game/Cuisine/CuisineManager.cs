using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using Scoz.Func;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using GridFramework.Grids;
using Unity.Entities.UniversalDelegates;
using UnityEngine.UIElements;
using System.Linq;


namespace Gladiators.Cuisine {
    public class CuisineManager : MonoBehaviour {
        public static CuisineManager Instance;
        [SerializeField] Camera MyCam;
        [SerializeField] bool MobileControl;
        [SerializeField] CuisineCardPrefab cardPrefab;
        [SerializeField] Transform CardParent;
        [SerializeField] Vector2 RawColumn;
        [SerializeField] RectGrid rectGrid;
        [SerializeField] int CardFlipBackMiliSecs;

        bool playing = false;
        int curLeftTime;
        public int StartCountDownSec { get; private set; }
        CuisineGame myGame;
        Dictionary<int, CuisineCardPrefab> cards = new Dictionary<int, CuisineCardPrefab>();
        bool canFlip = false;

        public void Init() {
            Instance = this;
            setCam();//設定攝影機模式
            initGame();
#if !UNITY_EDITOR // 輸出版本要根據平台判斷操控方式
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) MobileControl = true;
            else MobileControl = false;
#endif

            CuisineSceneUI.Instance.ShowCountingdown(false);
        }

        public void SetCanFlip(bool _canFlip) {
            canFlip = _canFlip;
        }

        void initGame() {
            SetCanFlip(true);
            myGame = new CuisineGame(18);
            cards.Clear();
            var cardDatas = myGame.Cards.Values.ToList();
            cardDatas.Shuffle();
            for (int i = 0; i < cardDatas.Count; i++) {
                var go = Instantiate(cardPrefab, CardParent);
                var card = go.GetComponent<CuisineCardPrefab>();
                card.Set(cardDatas[i], OnCardFlip);
                cards.Add(cardDatas[i].Idx, card);

                // 設定座標
                int row = (int)(i / RawColumn.x);
                int col = (int)(i % RawColumn.y);
                Vector3 gridCoords = new Vector3(col, row, 0);
                Vector3 worldPos = rectGrid.GridToWorld(gridCoords);
                go.transform.position = worldPos;
            }

        }
        public void ResetGame() {
            SetCanFlip(true);
            myGame.ResetGame();
            cards.Clear();
            var cardDatas = new List<CusineCard>(myGame.Cards.Values);
            cardDatas.Shuffle();
            for (int i = 0; i < cardDatas.Count; i++) {
                cards[i].Set(cardDatas[i], OnCardFlip);
            }
        }
        public void OnCardFlip(CusineCard _card) {
            if (canFlip == false) return;
            var (match, status) = myGame.FlipCard(_card.Idx);

            if (status == -1) return;
            cards[_card.Idx].Refresh();
            if (status == 1) {
                SetCanFlip(false);
                UniTask.Void(async () => {
                    await UniTask.Delay(CardFlipBackMiliSecs);
                    myGame.FlipBack();
                    refreshCards();
                    SetCanFlip(true);
                });
            }


        }
        void refreshCards() {
            foreach (var card in cards.Values) {
                card.Refresh();
            }
        }

        void setCam() {
            //因為戰鬥場景的攝影機有分為場景與UI, 要把場景攝影機設定為Base, UI設定為Overlay, 並在BaseCamera中加入Camera stack
            UICam.Instance.SetRendererMode(CameraRenderType.Overlay);
            addCamStack(UICam.Instance.MyCam);
        }
        /// <summary>
        /// 將指定camera加入到MyCam的CameraStack中
        /// </summary>
        void addCamStack(Camera _cam) {
            if (_cam == null) return;
            var cameraData = MyCam.GetUniversalAdditionalCameraData();
            if (cameraData == null) return;
            cameraData.cameraStack.Add(_cam);
        }
        void restartGame() {
            CuisineSceneUI.Instance.ShowCountingdown(false);
            StartGame();
        }
        public void StartGame() {
            playing = true;
            // 開始倒數計時
            UniTask.Void(async () => {
                curLeftTime = StartCountDownSec;
                CuisineSceneUI.Instance.SetCountdownImg(curLeftTime);
                while (curLeftTime > 0) {
                    if (!playing) break;
                    await UniTask.Delay(1000);
                    curLeftTime--;
                    CuisineSceneUI.Instance.SetCountdownImg(curLeftTime);
                }
                if (playing) endGame();
            });
        }
        void endGame() {
            playing = false;
            //PopupUI.ShowAttributeUI($"體力回復增加{addVigorGen}/s", restartGame);
        }




    }
}