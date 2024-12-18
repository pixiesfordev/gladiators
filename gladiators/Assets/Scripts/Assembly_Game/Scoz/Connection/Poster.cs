using System;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Scoz.Func {
    public class Poster : MonoBehaviour {
        public static async UniTask<string> Post(string _url, string _token, string _bodyJson) {
            try {
                using (UnityWebRequest request = new UnityWebRequest(_url, "POST")) {
                    WriteLog.LogColorFormat("Post URL: {0}", WriteLog.LogType.ServerAPI, _url);
                    if (!string.IsNullOrEmpty(_token)) request.SetRequestHeader("Authorization", "Bearer " + _token);
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(_bodyJson);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.SetRequestHeader("Content-Type", "application/json");

                    await request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                        request.result == UnityWebRequest.Result.ProtocolError ||
                        request.result == UnityWebRequest.Result.DataProcessingError) {
                        WriteLog.LogError("Poster Post錯誤: " + request.error);
                        return null;
                    }
                    string responseContent = request.downloadHandler.text;
                    WriteLog.LogColor($"Response Code: {request.responseCode}  Result: {request.result}", WriteLog.LogType.ServerAPI);
                    WriteLog.LogColor($"Content: {responseContent}", WriteLog.LogType.ServerAPI);

                    return responseContent;
                }
            } catch (System.Exception _ex) {
                WriteLog.LogError($"Poster Post錯誤: {_ex.Message}");
                return null;
            }
        }

        public static async UniTask<string> Get(string _url, string _token) {
            try {
                using (UnityWebRequest request = UnityWebRequest.Get(_url)) {
                    WriteLog.LogColorFormat("Get URL: {0}", WriteLog.LogType.ServerAPI, _url);
                    if (!string.IsNullOrEmpty(_token)) request.SetRequestHeader("Authorization", "Bearer " + _token);
                    request.timeout = 30;
                    await request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                        request.result == UnityWebRequest.Result.ProtocolError ||
                        request.result == UnityWebRequest.Result.DataProcessingError) {
                        WriteLog.LogError($"Poster Get錯誤: {request.error}");
                        return null;
                    }

                    string responseContent = request.downloadHandler.text;
                    WriteLog.LogColor($"Response Code: {request.responseCode}", WriteLog.LogType.ServerAPI);
                    WriteLog.LogColor($"Response: {responseContent}", WriteLog.LogType.ServerAPI);
                    return responseContent;
                }
            } catch (Exception _ex) {
                WriteLog.LogError($"Poster Get錯誤: {_ex.Message}");
                return null;
            }
        }

    }
}
