using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scoz.Func {

    public class SpriteNumber : MonoBehaviour {

        [SerializeField] Transform SpriteParent;
        Image[] Imgs_Number;

        void getImgs() {
            Imgs_Number = SpriteParent.GetComponentsInChildren<Image>();
        }

        public void SetImg(int _num) {
            if (Imgs_Number == null) getImgs();
            string numStr = _num.ToString();
            int length = numStr.Length;
            if (length > Imgs_Number.Length) {
                WriteLog.LogError("Imgs_Number 長度不足，要加一下prefab");
                return;
            }

            AddressablesLoader.GetSpriteAtlas("Number", atlas => {
                for (int i = 0; i < Imgs_Number.Length; i++) {
                    if (i < length) {
                        char digitChar = numStr[i];
                        Imgs_Number[i].sprite = atlas.GetSprite(digitChar.ToString());
                        Imgs_Number[i].gameObject.SetActive(true);
                    } else {
                        Imgs_Number[i].gameObject.SetActive(false);
                    }
                }
            });
        }
    }
}