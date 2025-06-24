using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gladiators.Main {
    public enum Totem {
        Battle,
        Trial,
        Destiny,
        Camp,
        Boss,
    }
    public class TotemPole : MonoBehaviour {
        [SerializeField] GameObject[] Prefab_Totem;
        [SerializeField] Transform TotemParent;
        [SerializeField] Vector3 Pos_TotemPole; // 圖騰柱底部位置，圖騰產生位置是基於這個位置
        [SerializeField] float TotemHeight; // 圖騰高度
        [SerializeField] float DropHeight; // 掉落額外高度

        List<TotemPrefab> Totems = new List<TotemPrefab>();

        /// <summary>
        /// 新圖騰產生位置是基於圖騰柱底部+圖騰高度*目前圖騰數量+掉落額外高度
        /// </summary>
        Vector3 nextTotemPos {
            get {
                float addTotemHeight = TotemHeight * Totems.Count + DropHeight;
                Vector3 pos = new Vector3(Pos_TotemPole.x, Pos_TotemPole.y + addTotemHeight, Pos_TotemPole.z);
                return pos;
            }
        }

        GameObject getTotemPrefab(Totem _totme) {
            if ((int)_totme >= Prefab_Totem.Length) {
                WriteLog.LogError("getTotemPrefab 錯誤");
                return null;
            }
            return Prefab_Totem[(int)_totme];
        }



        public void SetTotems(List<Totem> _totmes) {
            Totems.Clear();
            for (int i = 0; i < _totmes.Count; i++) {
                AddTotem(_totmes[i]);
            }
        }
        public void AddTotem(Totem _totem) {
            var prefab = getTotemPrefab(_totem);
            if (prefab == null) {
                WriteLog.LogError("AddTotem 失敗");
                return;
            }
            var newTotemGO = Instantiate<GameObject>(prefab, nextTotemPos, Quaternion.identity, TotemParent);
            var totem = newTotemGO.GetComponent<TotemPrefab>();
            Totems.Add(totem);
        }

        /// <summary>
        /// 取得下一個圖騰並移除
        /// </summary>
        public Totem TakeAwayNextTotem() {
            var totem = Totems[0].TotemType;
            Destroy(Totems[0].gameObject);
            Totems.RemoveAt(0);
            return totem;
        }
    }
}