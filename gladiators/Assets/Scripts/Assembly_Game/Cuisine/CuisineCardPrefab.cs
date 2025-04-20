using Scoz.Func;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Gladiators.Cuisine {

    public class CuisineCardPrefab : MonoBehaviour, IItem {
        [SerializeField] Image Img;
        [SerializeField] Sprite Sprite_Back;

        public CusineCard MyData { get; private set; }
        public bool IsActive { get; set; }

        Action<CusineCard> clickAc;

        public void Set(CusineCard _data, Action<CusineCard> _ac) {
            MyData = _data;
            clickAc = _ac;
            Refresh();
        }
        public async void Refresh() {
            if (MyData == null) return;
            if (!MyData.IsFaceUp) {
                Img.sprite = Sprite_Back;
            } else {
                Img.sprite = await MyData.GetSprite();
            }
        }
        public void OnClick() {
            clickAc?.Invoke(MyData);
        }

    }
}