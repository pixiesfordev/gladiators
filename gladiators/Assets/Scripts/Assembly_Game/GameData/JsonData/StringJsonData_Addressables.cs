using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine.AddressableAssets;
using System.Text;

namespace Scoz.Func {
    public partial class StringJsonData {
        public static void SetStringDic_Remote(Action<Dictionary<string, StringJsonData>> _cb) {
            string dataName = "String";
            Dictionary<string, StringJsonData> dic = new Dictionary<string, StringJsonData>();
            Addressables.LoadAssetAsync<TextAsset>(string.Format("Assets/AddressableAssets/Jsons/{0}.json", dataName)).Completed += handle => {
                string jsonStr = handle.Result.text;
                JsonData jd = JsonMapper.ToObject(jsonStr);
                JsonData items = jd[dataName];
                for (int i = 0; i < items.Count; i++) {
                    StringJsonData data = new StringJsonData(items[i], dataName);
                    string name = GetValue<string>(items[i]["ID"].ToString());
                    if (dic.ContainsKey(name)) {
                        WriteLog.LogWarning(string.Format("{0}表的欄位({1})重複", dataName, name));
                        break;
                    }
                    dic.Add(name, data);
                }
                Addressables.Release(handle);
                if (ShowLoadTime) {
                    WriteLog.LogFormat("<color=#398000>[Json] {0}.json載入完成</color>", dataName);
                }
                _cb?.Invoke(dic);
            };
        }
    }
}
