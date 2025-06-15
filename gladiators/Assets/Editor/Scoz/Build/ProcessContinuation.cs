using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Scoz.Editor {
    [InitializeOnLoad]
    public static class ProcessContinuation {
        private const string PREFS_KEY = "ProcessContinuation_ProcessKeys";
        private const char SEPARATOR = '|';
        private const string NAMESPACE_PREFIX = "Scoz.Editor.";
        private const string PREFS_PARAMS = "ProcessContinuation_ProcessParams";
        // key為方法名稱 value為string引數(逗號隔開)
        private static Dictionary<string, string> processMethodParameters = new Dictionary<string, string>();

        [Serializable]
        private class ProcessParamsContainer {
            public List<string> keys = new List<string>();
            public List<string> values = new List<string>();
        }

        static ProcessContinuation() {
            // 重載時讀取持久化的引數
            processMethodParameters = LoadPersistedProcessParameters();
            // 編譯完成後呼叫
            EditorApplication.delayCall += ContinueProcess;
        }

        /// <summary>
        /// 傳入方法名(格式: "ClassName.MethodName") 與引數string(逗號隔開)
        /// </summary>
        public static void SetProcess(string processKey, string parameters) {
            if (string.IsNullOrEmpty(processKey))
                return;
            processMethodParameters[processKey] = parameters;
            SavePersistedProcessParameters(processMethodParameters);
            List<string> keys = GetPersistedProcessKeys();
            if (!keys.Contains(processKey)) {
                keys.Add(processKey);
                SavePersistedProcessKeys(keys);
            }
        }

        public static void RunProcess(string processKey) {
            if (string.IsNullOrEmpty(processKey))
                return;
            InvokeMethod(processKey);
            RemoveProcess(processKey);
        }

        private static void ContinueProcess() {
            List<string> keys = GetPersistedProcessKeys();
            foreach (string key in new List<string>(keys)) {
                Debug.Log("執行流程: " + key);
                InvokeMethod(key);
                RemoveProcess(key);
            }
        }

        /// <summary>
        /// 利用反射在指定 Assembly 中跑靜態方法
        /// </summary>
        private static void InvokeMethod(string processKey) {
            try {
                if (!processKey.Contains(".")) {
                    Debug.LogError("方法Key格式錯誤，必須為 ClassName.MethodName 格式：" + processKey);
                    return;
                }
                string[] parts = processKey.Split(new char[] { '.' }, 2);
                string className = parts[0];
                string methodName = parts[1];
                string fullTypeName = NAMESPACE_PREFIX + className;
                Assembly asm = Assembly.GetAssembly(typeof(ProcessContinuation));
                Type targetType = asm.GetType(fullTypeName);
                if (targetType == null) {
                    Debug.LogError("找不到類型：" + fullTypeName);
                    return;
                }
                // 取得對應的參數字串
                string paramString = "";
                processMethodParameters.TryGetValue(processKey, out paramString);
                object[] parameters;
                if (string.IsNullOrEmpty(paramString)) {
                    parameters = new object[0];
                } else {
                    string[] paramParts = paramString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    // Trim each parameter
                    for (int i = 0; i < paramParts.Length; i++) {
                        paramParts[i] = paramParts[i].Trim();
                    }
                    parameters = new object[paramParts.Length];
                    for (int i = 0; i < paramParts.Length; i++) {
                        parameters[i] = paramParts[i];
                    }
                }
                // 建立參數類型陣列(統一為string)
                Type[] paramTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++) {
                    paramTypes[i] = typeof(string);
                }
                MethodInfo method = targetType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, paramTypes, null);
                if (method == null) {
                    Debug.LogError("找不到方法：" + methodName + " 在類型：" + fullTypeName + "，接受 " + parameters.Length + " 個 string 參數");
                    return;
                }
                method.Invoke(null, parameters);
            } catch (Exception ex) {
                Debug.LogError("呼叫方法 " + processKey + " 時發生錯誤：" + ex);
            }
        }

        private static List<string> GetPersistedProcessKeys() {
            string keysStr = EditorPrefs.GetString(PREFS_KEY, "");
            if (string.IsNullOrEmpty(keysStr))
                return new List<string>();
            return new List<string>(keysStr.Split(new char[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries));
        }

        private static void SavePersistedProcessKeys(List<string> keys) {
            string keysStr = string.Join(SEPARATOR.ToString(), keys);
            EditorPrefs.SetString(PREFS_KEY, keysStr);
        }

        private static void RemoveProcess(string processKey) {
            if (processMethodParameters.ContainsKey(processKey))
                processMethodParameters.Remove(processKey);
            SavePersistedProcessParameters(processMethodParameters);
            List<string> keys = GetPersistedProcessKeys();
            if (keys.Contains(processKey)) {
                keys.Remove(processKey);
                SavePersistedProcessKeys(keys);
            }
        }

        // 儲存 processMethodParameters 到 EditorPrefs (JSON 格式)
        private static void SavePersistedProcessParameters(Dictionary<string, string> dict) {
            ProcessParamsContainer container = new ProcessParamsContainer();
            foreach (var kvp in dict) {
                container.keys.Add(kvp.Key);
                container.values.Add(kvp.Value);
            }
            string json = JsonUtility.ToJson(container);
            EditorPrefs.SetString(PREFS_PARAMS, json);
        }

        // 新增：從 EditorPrefs 讀取 processMethodParameters
        private static Dictionary<string, string> LoadPersistedProcessParameters() {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string json = EditorPrefs.GetString(PREFS_PARAMS, "");
            if (!string.IsNullOrEmpty(json)) {
                try {
                    ProcessParamsContainer container = JsonUtility.FromJson<ProcessParamsContainer>(json);
                    for (int i = 0; i < container.keys.Count; i++) {
                        dict[container.keys[i]] = container.values[i];
                    }
                } catch (Exception ex) {
                    Debug.LogError("讀取持久化參數錯誤：" + ex);
                }
            }
            return dict;
        }
    }
}
