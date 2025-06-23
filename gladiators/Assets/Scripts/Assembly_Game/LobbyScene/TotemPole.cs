using Cysharp.Threading.Tasks;
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
    }
    public class TotemPole : MonoBehaviour {
        [SerializeField] GameObject[] Prefab_Totem;
        [SerializeField] Transform TotemParent;
        [SerializeField] Transform DefaultTotme;
        [SerializeField] float TotemHeight; // 一個圖騰的高度
        [SerializeField] Vector3 DropPos; // 新圖騰掉落位置
        [SerializeField] float DefaultTotmeMoveDuration; // 初始圖騰向下移動速度

        List<TotemPrefab> Totems = new List<TotemPrefab>();
        Vector3 defaultTotmePos;

        public void Init() {
            defaultTotmePos = DefaultTotme.localPosition;
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
            var newTotemGO = Instantiate<GameObject>(prefab, DropPos, Quaternion.identity, TotemParent);
            var totem = newTotemGO.GetComponent<TotemPrefab>();
            Totems.Add(totem);
            //moveDefaultTotem(Totems.Count);
        }
        //UniTask moveDefaultTotem(int _pos) {
        //    // 將 DefaultTotme Y軸從 defaultTotmePos-(_pos-1)*TotemHeight 移動到 defaultTotmePos-(_pos)*TotemHeight 的位置 並在DefaultTotmeMoveDuration秒數內完成移動
        //}
    }
}