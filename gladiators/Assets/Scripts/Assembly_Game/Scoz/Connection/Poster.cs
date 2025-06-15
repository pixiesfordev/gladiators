using System;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Scoz.Func {
    public class Poster : MonoBehaviour {
        /// <summary>
        /// 執行 HTTP POST，回傳 (responseContent, true) 或 (null, false)
        /// </summary>
        public static async UniTask<(string content, bool isSuccess)> Post(string _url, string _token, string _bodyJson) {
            WriteLog.LogColorFormat("Post URL: {0}", WriteLog.LogType.ServerAPI, _url);
            try {
                using (UnityWebRequest request = new UnityWebRequest(_url, "POST")) {
                    // 設定授權
                    if (!string.IsNullOrEmpty(_token)) {
                        request.SetRequestHeader("Authorization", $"Bearer {_token}");
                    }

                    // 設定 Content-Type 與 body
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(_bodyJson);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");

                    await request.SendWebRequest();

                    // 檢查錯誤狀態
                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                        request.result == UnityWebRequest.Result.ProtocolError ||
                        request.result == UnityWebRequest.Result.DataProcessingError) {
                        WriteLog.LogError($"Poster Post 錯誤: {request.error}");
                        return (null, false);
                    }

                    // 成功
                    string responseContent = request.downloadHandler.text;
                    WriteLog.LogColor($"Response Code: {request.responseCode}", WriteLog.LogType.ServerAPI);
                    WriteLog.LogColor($"Content: {responseContent}", WriteLog.LogType.ServerAPI);
                    return (responseContent, true);
                }
            } catch (Exception ex) {
                WriteLog.LogError($"Poster Post 例外: {ex.Message}");
                return (null, false);
            }
        }

        /// <summary>
        /// 執行 HTTP GET，回傳 (responseContent, true) 或 (null, false)
        /// </summary>
        public static async UniTask<(string content, bool isSuccess)> Get(string _url, string _token) {
            WriteLog.LogColorFormat("Get URL: {0}", WriteLog.LogType.ServerAPI, _url);

            try {
                using (UnityWebRequest request = UnityWebRequest.Get(_url)) {
                    if (!string.IsNullOrEmpty(_token)) {
                        request.SetRequestHeader("Authorization", $"Bearer {_token}");
                    }
                    request.timeout = 30;

                    await request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                        request.result == UnityWebRequest.Result.ProtocolError ||
                        request.result == UnityWebRequest.Result.DataProcessingError) {
                        WriteLog.LogError($"Poster Get 錯誤: {request.error}");
                        return (null, false);
                    }

                    string responseContent = request.downloadHandler.text;
                    WriteLog.LogColor($"Response Code: {request.responseCode}", WriteLog.LogType.ServerAPI);
                    WriteLog.LogColor($"Response: {responseContent}", WriteLog.LogType.ServerAPI);
                    return (responseContent, true);
                }
            } catch (Exception ex) {
                WriteLog.LogError($"Poster Get 例外: {ex.Message}");
                return (null, false);
            }
        }

    }
}
