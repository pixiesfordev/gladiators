using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Scoz.Func;
using System;
using System.Security.Policy;
using UnityEngine;


namespace Gladiators.Main {
    public class APIManager {
        static string domain;

        enum QueryType {
            Get,
            Post,
        }

        public static void Init() {
            if (!MyEnum.TryParseEnum("Domain_" + GameManager.CurVersion, out GameSetting settingKey)) {
                WriteLog.LogError("APIManager Init錯誤: 無法取得Domain");
                return;
            }
            domain = JsonGameSetting.GetStr(settingKey);
            WriteLog.LogColor($"domain: {domain}", WriteLog.LogType.ServerAPI);
        }
        static async UniTask<string> query(QueryType _type, string _url, object _data) {
            if (string.IsNullOrEmpty(_url)) {
                WriteLog.LogError($"{_type} uri 為 null");
                return null;
            }
            if (_type == QueryType.Post && _data == null) {
                WriteLog.LogError($"{_type} _data 為 null");
                return null;
            }
            string jsonBody = JsonConvert.SerializeObject(_data);
            string res = "";
            var dbPlayer = GamePlayer.Instance.GetDBData<DBPlayer>();
            string connToken = "";
            if (dbPlayer != null) connToken = dbPlayer.ConnToken;
            if (_type == QueryType.Get) {
                res = await Poster.Get(_url, connToken);
            } else {
                res = await Poster.Post(_url, connToken, jsonBody);
            }
            if (string.IsNullOrEmpty(res)) {
                WriteLog.LogError($"{_type} API 錯誤: {_url}");
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
        public static async UniTask<System.DateTimeOffset> GetServerTime() {
            string url = domain + "/game/servertime";
            string res = await query(QueryType.Get, url, null);
            if (string.IsNullOrEmpty(res)) {
                WriteLog.LogError("API錯誤: " + url);
                return default(System.DateTimeOffset);
            }
            var resp = decode<GetServerTime_Res>(res);
            if (resp == null) return default(System.DateTimeOffset);
            return resp.ServerTime;
        }
        public class GetServerTime_Res {
            [JsonProperty("data")]
            public System.DateTimeOffset ServerTime;
        }

        public static async UniTask<DBPlayer> Signup(string _authType, string _authData, string _deviceType, string _deviceUID) {
            string url = domain + "/game/signup";
            var sendData = new {
                AuthType = _authType,
                AuthData = _authData,
                DeviceType = _deviceType,
                DeviceUID = _deviceUID
            };
            string res = await query(QueryType.Post, url, sendData);
            var resp = decode<Signup_Res>(res);
            if (resp == null || resp.MyDBPlayer == null) return null;
            return resp.MyDBPlayer;
        }
        public class Signup_Res {
            [JsonProperty("data")]
            public DBPlayer MyDBPlayer;
        }

        public static async UniTask<DBPlayer> Signin(string _playerID, string _authType, string _authData, string _deviceType, string _deviceUID) {
            string url = domain + "/game/signin";
            var sendData = new {
                PlayerID = _playerID,
                AuthType = _authType,
                AuthData = _authData,
                DeviceType = _deviceType,
                DeviceUID = _deviceUID
            };
            string res = await query(QueryType.Post, url, sendData);
            var resp = decode<Signin_Res>(res);
            if (resp == null || resp.MyDBPlayer == null) return null;
            return resp.MyDBPlayer;
        }
        public class Signin_Res {
            [JsonProperty("data")]
            public DBPlayer MyDBPlayer;
        }

        public static async UniTask<DBGameState> GameState() {
            string url = domain + "/game/gamestate";
            string res = await query(QueryType.Get, url, null);
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