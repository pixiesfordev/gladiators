using Scoz.Func;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gladiators.Main {
    public class TotemSymbolSpawner : MonoBehaviour {
        [SerializeField] GameObject[] Prefab_TotemSymbols; // 不同的圖騰Prefab
        [SerializeField] Transform Trans_Parent;
        [SerializeField] float Radius = 1f;  // 圖騰柱半徑
        [SerializeField] float StartAngle = 180f; // 起始角度(控制第一個圖騰位置)
        Dictionary<int, TotemSymbolPrefab> Totems = new Dictionary<int, TotemSymbolPrefab>(); // key就代表位置 

        public void SetTotems(Dictionary<int, Totem> _totems) {
            foreach (var item in Totems.Values) {
                if (item != null) Destroy(item.gameObject);
            }
            foreach (var key in _totems.Keys) {
                var dir = getTotemDir(key);
                var pos = getTotemPos(dir);
                Quaternion quaternion = Quaternion.LookRotation(-dir, Vector3.up);
                var totem = spwanNewTotem(_totems[key], pos, quaternion);
                Totems[key] = totem;
            }
        }
        public void ChangeToken(int _idx, Totem _totem) {
            Destroy(Totems[_idx]);
            var dir = getTotemDir(_idx);
            var pos = getTotemPos(dir);
            Quaternion quaternion = Quaternion.LookRotation(-dir, Vector3.up);
            var totem = spwanNewTotem(_totem, pos, quaternion);
            Totems[_idx] = totem;
        }
        Vector3 getTotemDir(int _idx) {
            if (_idx < 0 || _idx > 5) {
                WriteLog.LogError($"傳入 _idx: {_idx} 錯誤");
                return Vector3.zero;
            }
            float startAngleRad = Mathf.Deg2Rad * StartAngle;
            float angleRad = startAngleRad + Mathf.PI / 3f * _idx;
            Vector3 dir = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
            return dir;
        }
        Vector3 getTotemPos(Vector3 _dir) {
            return _dir * Radius;
        }
        GameObject getTotemPrefab(Totem _totem) {
            if ((int)_totem >= Prefab_TotemSymbols.Length) {
                WriteLog.LogError("getToeknPrefab 錯誤");
                return null;
            }
            var go = Prefab_TotemSymbols[(int)_totem];
            if (go == null) {
                WriteLog.LogError($"Prefab_TotemSymbols索引 {(int)_totem} 目標為null");
                return null;
            }
            return go;
        }
        TotemSymbolPrefab spwanNewTotem(Totem _totem, Vector3 _pos, Quaternion _quat) {
            var prefab = getTotemPrefab(_totem);
            var go = Instantiate<GameObject>(prefab);
            go.transform.SetParent(Trans_Parent, false);
            go.transform.localPosition = _pos;
            go.transform.localRotation = _quat;
            var totem = go.GetComponent<TotemSymbolPrefab>();
            return totem;
        }


    }
}