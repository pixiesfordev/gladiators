using Cysharp.Threading.Tasks;
using Gladiators.Battle;
using GridFramework.Grids;
using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
namespace Gladiators.Cuisine {
    public class CuisineSceneUI : ItemSpawner_Remote<CuisineCardPrefab> {
        [HeaderAttribute("==============AddressableAssets==============")]
        [SerializeField] Image[] Img_Coutingdown;
        [SerializeField] int CardFlipBackMiliSecs;
        [SerializeField] Animator StartCountingDownAni;
        [SerializeField] int ChanceCount = 5; // 幾次翻牌機會
        [SerializeField] Text Txt_RemainChance;
        public static CuisineSceneUI Instance { get; private set; }
        //int curLeftTime;
        public int StartCountDownSec { get; private set; }
        CuisineGame myGame;
        Dictionary<int, CuisineCardPrefab> cards = new Dictionary<int, CuisineCardPrefab>();
        bool canFlip = false;


        private void Start() {
            ShowCountingdown(false);
            LoadItemAsset(Init);
        }


        public override void Init() {
            base.Init();
            StartGame();
        }
        public void StartGame() {
            initGame();
            StartCountingDownAni.gameObject.SetActive(true);
            StartCountingDownAni.Play(0);
            //// 開始倒數計時
            //UniTask.Void(async () => {
            //    curLeftTime = StartCountDownSec;
            //    CuisineSceneUI.Instance.SetCountdownImg(curLeftTime);
            //    while (curLeftTime > 0) {
            //        if (!playing) break;
            //        await UniTask.Delay(1000);
            //        curLeftTime--;
            //        CuisineSceneUI.Instance.SetCountdownImg(curLeftTime);
            //    }
            //    if (playing) endGame();
            //});
        }
        /// <summary>
        /// 動畫倒數完呼叫
        /// </summary>
        public void OnStartCountingDownEnd() {
            SetCanFlip(true);
            StartCountingDownAni.gameObject.SetActive(false);
        }
        void endGame() {
            SetCanFlip(false);
        }
        void initGame() {
            myGame = new CuisineGame(6, ChanceCount);
            RefreshText();
            cards.Clear();
            var cardDatas = myGame.Cards.Values.ToList();
            cardDatas.Shuffle();

            if (!LoadItemFinished) {
                WriteLog.LogError("ItemAsset尚未載入完成");
                return;
            }
            InActiveAllItem();
            if (cardDatas == null || cardDatas.Count == 0) {
                WriteLog.LogError("傳入的_jsons為空或長度為0");
                return;
            }
            for (int i = 0; i < cardDatas.Count; i++) {
                if (i < ItemList.Count) {
                    ItemList[i].Set(cardDatas[i], OnCardFlip);
                    ItemList[i].IsActive = true;
                    ItemList[i].gameObject.SetActive(true);

                } else {
                    var item = Spawn();
                    item.Set(cardDatas[i], OnCardFlip);
                }
                cards.Add(cardDatas[i].Idx, ItemList[i]);
            }
        }
        public void OnCardFlip(CusineCard _card) {
            if (canFlip == false) return;
            var (match, status) = myGame.FlipCard(_card.Idx);
            RefreshText();
            if (status == -1) return;
            cards[_card.Idx].Refresh();
            if (status == 1) {
                SetCanFlip(false);
                if (myGame.CurChanceCount > 0) { // 還有翻牌次數
                    UniTask.Void(async () => {
                        await UniTask.Delay(CardFlipBackMiliSecs);
                        myGame.FlipBack();
                        refreshCards();
                        SetCanFlip(true);
                    });
                } else { // 翻牌次數用盡
                    endGame();
                }
            }
        }
        void refreshCards() {
            foreach (var card in cards.Values) {
                card.Refresh();
            }
        }
        public void SetCanFlip(bool _canFlip) {
            canFlip = _canFlip;
        }
        public override void RefreshText() {
            Txt_RemainChance.text = $"剩餘次數: {myGame.CurChanceCount}";
        }
        protected override void SetInstance() {
            Instance = this;
        }

        public void ShowCountingdown(bool _show) {
            for (int i = 0; i < Img_Coutingdown.Length; i++) Img_Coutingdown[i].gameObject.SetActive(_show);
        }

        public void SetCountdownImg(int _num) {

            if (_num < 0) return;
            string numStr = _num.ToString();
            int length = numStr.Length;
            if (length > Img_Coutingdown.Length) {
                Debug.LogError("Img_Coutingdown 長度不足，要加一下prefab");
                return;
            }

            AddressablesLoader.GetSpriteAtlas("Number", atlas => {
                for (int i = 0; i < Img_Coutingdown.Length; i++) {
                    if (i < length) {
                        char digitChar = numStr[i];
                        Img_Coutingdown[i].sprite = atlas.GetSprite(digitChar.ToString());
                        Img_Coutingdown[i].gameObject.SetActive(true);
                    } else {
                        Img_Coutingdown[i].gameObject.SetActive(false);
                    }
                }
            });
        }



    }
}