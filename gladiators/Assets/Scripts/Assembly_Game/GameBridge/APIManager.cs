using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Scoz.Func;
using System;
using System.Security.Policy;
using UnityEngine;


namespace Gladiators.Main {
    public class APIManager {
        static string domain;

        public static void Init() {
            if (!MyEnum.TryParseEnum("Domain_" + GameManager.CurVersion, out GameSetting settingKey)) {
                WriteLog.LogError("APIManager Init錯誤: 無法取得Domain");
                return;
            }
            domain = JsonGameSetting.GetStr(settingKey);
            WriteLog.LogColor($"domain: {domain}", WriteLog.LogType.ServerAPI);
        }
        static async UniTask<string> post(string _url, object? _data) {
            if (string.IsNullOrEmpty(_url)) {
                WriteLog.LogError($"post uri 為 null");
                return null;
            }
            if (_data == null) {
                WriteLog.LogError($"post _data 為 null");
                return null;
            }
            string jsonBody = JsonConvert.SerializeObject(_data);
            string res = await Poster.Post(_url, jsonBody);
            if (string.IsNullOrEmpty(res)) {
                WriteLog.LogError("post API 錯誤: " + _url);
                return null;
            }
            return res;
        }
        static T decode<T>(string _res) where T : class {
            var resp = JsonConvert.DeserializeObject<T>(_res);
            if (resp == null) {
                WriteLog.LogError("Decode 錯誤: " + _res);
                return null;
            }
            return resp;
        }
        public static async UniTask<System.DateTimeOffset?> GetServerTime() {
            string url = domain + "/game/servertime";
            string res = await post(url, null);
            if (string.IsNullOrEmpty(res)) {
                WriteLog.LogError("API錯誤: " + url);
                return null;
            }
            var resp = decode<GetServerTime_Res>(res);
            if (resp == null) return default(System.DateTimeOffset);
            return resp.ServerTime;
        }
        public class GetServerTime_Res {
            [JsonProperty("data")]
            public System.DateTimeOffset ServerTime;
        }

        public static async UniTask<DBPlayer> Signup(string _authType, string _authData, string _deviceUID) {
            string url = domain + "/game/signup";
            var sendData = new {
                AuthType = _authType,
                AuthData = _authData,
                DeviceUID = _deviceUID
            };
            string res = await post(url, sendData);
            var resp = decode<Signup_Res>(res);
            if (resp == null || resp.MyDBPlayer == null) return null;
            return resp.MyDBPlayer;
        }
        public class Signup_Res {
            [JsonProperty("data")]
            public DBPlayer MyDBPlayer;
        }

        public static async UniTask<DBPlayer> Signin(string _playerID, string _authType, string _authData, string _deviceType, string _deviceUID) {
            string url = domain + "/player/signin";
            var sendData = new {
                PlayerID = _playerID,
                AuthType = _authType,
                AuthData = _authData,
                DeviceType = _deviceType,
                DeviceUID = _deviceUID
            };
            string res = await post(url, sendData);
            var resp = decode<Signin_Res>(res);
            if (resp == null || resp.MyDBPlayer == null) return null;
            return resp.MyDBPlayer;
        }
        public class Signin_Res {
            [JsonProperty("data")]
            public DBPlayer MyDBPlayer;
        }

        public static async UniTask<DBGameState> GameState(string _connToken) {
            string url = domain + "/player/gamestate";
            var sendData = new {
                ConnToken = _connToken,
            };
            string res = await post(url, sendData);
            var resp = decode<GameState_Res>(res);
            if (resp == null || resp.MyDBGameState == null) return null;
            return resp.MyDBGameState;
        }
        public class GameState_Res {
            [JsonProperty("data")]
            public DBGameState MyDBGameState;
        }


    }

}