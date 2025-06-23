using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Scoz.Func;
using UnityEngine;


namespace Gladiators.Main {
    public class APIManager {
        static string domain;

        enum QueryType {
            Get,
            Post,
        }

        public static void Init() {
            if (!MyEnum.TryParseEnum("Domain_" + GameManager.CurVersion, out GameSetting settingKey_domain)) {
                WriteLog.LogError("APIManager Init錯誤: 無法取得Domain");
                return;
            }
            domain = JsonGameSetting.GetStr(settingKey_domain);
            WriteLog.LogColor($"domain: {domain} ", WriteLog.LogType.ServerAPI);
            UniTask.Void(async () => {
                var (time, result) = await APIManager.GetServerTime();
                if (result == false) {
                    WriteLog.LogError($"APIManager.GetServerTime失敗");
                    PopupUI.ShowClickCancel("APIManager.GetServerTime失敗", null);
                    return;
                }
                time = time.AddHours(8); // +8時區
                GameManager.Instance.SetTime(time);
                WriteLog.LogColor($"time: {time} ", WriteLog.LogType.ServerAPI);
            });

        }
        static async UniTask<(string, bool)> query(QueryType _type, string _url, string _token, object _data) {
            if (string.IsNullOrEmpty(_url)) {
                WriteLog.LogError($"{_type} uri 為 null");
                return (null, false);
            }
            if (_type == QueryType.Post && _data == null) {
                WriteLog.LogError($"{_type} _data 為 null");
                return (null, false);
            }
            string jsonBody = JsonConvert.SerializeObject(_data);
            string res = "";
            bool result = false;
            if (_type == QueryType.Get) {
                (res, result) = await Poster.Get(_url, _token);
            } else {
                (res, result) = await Poster.Post(_url, _token, jsonBody);
            }
            return (res, result);
        }
        static T decode<T>(string _res) where T : class {
            if (string.IsNullOrEmpty(_res)) {
                WriteLog.LogError("_res 為空");
                return null;
            }
            var resp = JsonConvert.DeserializeObject<T>(_res);
            if (resp == null) {
                return null;
            }
            return resp;
        }


        public static async UniTask<(System.DateTimeOffset, bool)> GetServerTime() {
            string url = domain + "/game/servertime";
            var (res, result) = await query(QueryType.Get, url, null, null);
            if (string.IsNullOrEmpty(res)) {
                WriteLog.LogError("API錯誤: " + url);
                return (default(System.DateTimeOffset), false);
            }
            var resp = decode<GetServerTime_Res>(res);
            if (resp == null) return (default(System.DateTimeOffset), false);
            return (resp.ServerTime, true);
        }
        public class GetServerTime_Res {
            [JsonProperty("data")]
            public System.DateTimeOffset ServerTime;
        }

        public static async UniTask<(DBPlayer, bool)> Signup(string _authType, string _authData, string _deviceType, string _deviceUID) {
            string url = domain + "/game/signup";
            var sendData = new {
                AuthType = _authType,
                AuthData = _authData,
                DeviceType = _deviceType,
                DeviceUID = _deviceUID
            };
            var (res, result) = await query(QueryType.Post, url, null, sendData);
            var resp = decode<Signup_Res>(res);
            if (result == false || resp == null || resp.MyDBPlayer == null) return (null, false);
            return (resp.MyDBPlayer, true);
        }
        public class Signup_Res {
            [JsonProperty("data")]
            public DBPlayer MyDBPlayer;
        }



        public static async UniTask<(DBPlayer, bool)> Signin(string _playerID, string _authType, string _authData, string _deviceType, string _deviceUID) {
            string url = domain + "/game/signin";
            var sendData = new {
                PlayerID = _playerID,
                AuthType = _authType,
                AuthData = _authData,
                DeviceType = _deviceType,
                DeviceUID = _deviceUID
            };
            var (res, result) = await query(QueryType.Post, url, null, sendData);
            var resp = decode<Signin_Res>(res);
            if (result == false || resp == null || resp.MyDBPlayer == null) return (null, false);
            return (resp.MyDBPlayer, true);
        }
        public class Signin_Res {
            [JsonProperty("data")]
            public DBPlayer MyDBPlayer;
        }

        public static async UniTask<(DBGameState, bool)> GameState() {
            string url = domain + "/game/gamestate";
            var (res, result) = await query(QueryType.Get, url, null, null);
            var resp = decode<GameState_Res>(res);
            if (result == false || resp == null || resp.MyDBGameState == null) return (null, false);
            return (resp.MyDBGameState, true);
        }
        public class GameState_Res {
            [JsonProperty("data")]
            public DBGameState MyDBGameState;
        }


    }

}