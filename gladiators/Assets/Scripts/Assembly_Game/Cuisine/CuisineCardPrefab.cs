using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gladiators.Cuisine {

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))] // 或 CircleCollider2D 等
    public class CuisineCardPrefab : MonoBehaviour {
        [SerializeField] SpriteRenderer MyRenderer;
        [SerializeField] Sprite Sprite_Back;

        public CusineCard MyData { get; private set; }
        Action<CusineCard> clickAc;

        public void Set(CusineCard _data, Action<CusineCard> _ac) {
            MyData = _data;
            clickAc = _ac;
            Refresh();
        }
        public async void Refresh() {
            if (MyData == null) return;
            if (!MyData.IsFaceUp) {
                MyRenderer.sprite = Sprite_Back;
            } else {
                MyRenderer.sprite = await MyData.GetSprite();
            }
        }
        public void OnClick() {
            WriteLog.LogError("MyData=" + MyData.ID);
            clickAc?.Invoke(MyData);
        }

    }
}