using Scoz.Func;
using System;
using System.Collections.Generic;

namespace Gladiators.Cuisine {
    public class CuisineGame {
        /// <summary>存放所有卡片</summary>
        public Dictionary<int, CusineCard> Cards { get; private set; }

        /// <summary>紀錄用來初始化卡片對數</summary>
        int pairCount;

        /// <summary>已翻開等待配對的第一張卡</summary>
        CusineCard firstCard;

        /// <summary>已翻開等待配對的第二張卡</summary>
        CusineCard secondCard;

        int maxChanceCount; // 可翻牌機會
        public int CurChanceCount { get; private set; }

        /// <summary>
        /// 初始化遊戲：給定「對數」，建立並洗牌卡片。
        /// 例如 pairCount=5，就會有10張卡（ID=[0,0,1,1,2,2,3,3,4,4]）。
        /// </summary>
        public CuisineGame(int pairCount, int chanceCount) {
            this.pairCount = pairCount;
            SetupCards(this.pairCount);
            this.maxChanceCount = chanceCount;
            CurChanceCount = chanceCount;
        }

        /// <summary>
        /// 重新設定遊戲牌組（可隨時呼叫以重置遊戲）。
        /// </summary>
        public void ResetGame() {
            firstCard = null;
            secondCard = null;
            CurChanceCount = maxChanceCount;
            SetupCards(pairCount);
        }

        /// <summary>
        /// 產生卡片並呼叫洗牌，最後生成 CusineCard 物件列表。
        /// </summary>
        private void SetupCards(int pairCount) {
            Cards = new Dictionary<int, CusineCard>();
            var cardList = new List<CusineCard>();
            for (int i = 0; i < pairCount; i++) {
                for (int j = 0; j < 2; j++) {
                    var card = new CusineCard(i * 2 + j, i + 1);
                    cardList.Add(card);
                }
            }
            foreach (var card in cardList) {
                Cards.Add(card.Idx, card);
            }
        }


        /// <summary>
        /// 翻牌，回傳是否配對成功與目前狀態ture為配對成功，-1為錯誤、0為第一張牌、1為第二張牌
        /// </summary>
        public (bool, int) FlipCard(int _idx) {

            if (CurChanceCount <= 0) {
                WriteLog.LogError("翻牌次數用完了");
                return (false, -1);
            }

            if (!Cards.ContainsKey(_idx)) {
                WriteLog.LogError($"錯誤索引: {_idx}");
                return (false, -1);
            }

            CusineCard card = Cards[_idx];

            // 如果這張卡已配對成功，或已經是正面朝上，就不動作
            if (card.IsMatched || card.IsFaceUp) {
                WriteLog.Log($"已經翻開此牌了: {_idx}");
                return (false, -1);
            }


            // 先把這張卡翻正面
            card.IsFaceUp = true;

            // 如果還沒選到第一張，就記錄為第一張
            if (firstCard == null) {
                firstCard = card;
                return (false, 0);
            } else {
                // 這是第二張 => 檢查是否配對
                secondCard = card;
                bool match = CheckMatch();
                return (match, 1);
            }
        }



        /// <summary>
        /// 檢查第一張與第二張是否配對；若失敗則翻回去。
        /// </summary>
        bool CheckMatch() {
            if (firstCard == null || secondCard == null)
                return false;

            // 判斷是否同 ID
            bool match = false;
            if (firstCard.ID == secondCard.ID) {
                // 配對成功
                firstCard.IsMatched = true;
                secondCard.IsMatched = true;
                match = true;
            }

            CurChanceCount--; // 翻開第二張牌後要減少次數

            return match;
        }
        public void FlipBack() {
            // 翻回背面
            if (!firstCard.IsMatched) firstCard.IsFaceUp = false;
            if (!secondCard.IsMatched) secondCard.IsFaceUp = false;
            // 清空翻開暫存
            firstCard = null;
            secondCard = null;
        }

        /// <summary>
        /// 判斷是否整副牌都已配對完成
        /// </summary>
        /// <returns>true 表示所有卡都 Matched</returns>
        public bool IsAllMatched() {
            foreach (var c in Cards.Values) {
                if (!c.IsMatched)
                    return false;
            }
            return true;
        }
    }
}
