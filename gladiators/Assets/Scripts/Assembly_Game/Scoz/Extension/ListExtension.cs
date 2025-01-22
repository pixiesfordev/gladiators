using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Scoz.Func {
    public static class ListExtension {
        public static int GetFirstItemOrderIndex<T>(this List<T> _list, T _t) {
            for (int i = 0; i < _list.Count; i++) {
                if (_t.Equals(_list[i])) {
                    return i;
                }
            }
            WriteLog.LogError("傳入的item不再list中");
            return 0;
        }
        public static List<float> ToFloatList(this List<object> _objList) {
            List<float> floatList = _objList.OfType<IConvertible>()
                .Select(item => Convert.ToSingle(item)).ToList();
            return floatList;
        }

        /// <summary>
        /// 以 Fisher–Yates 演算法對清單進行就地洗牌 (in-place shuffle)，使用 Unity 的 Random。
        /// </summary>
        public static void Shuffle<T>(this List<T> list) {
            for (int i = 0; i < list.Count; i++) {
                // 產生範圍在 [i, list.Count) 的隨機索引
                int swapIndex = UnityEngine.Random.Range(i, list.Count);

                // 交換位置 i 和 swapIndex 的元素
                T temp = list[i];
                list[i] = list[swapIndex];
                list[swapIndex] = temp;
            }
        }
    }
}