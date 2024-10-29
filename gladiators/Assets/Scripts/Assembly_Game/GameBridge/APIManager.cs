using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Scoz.Func;


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
        public static async UniTask<System.DateTimeOffset?> GetServerTime() {
            string url = domain + "/game/servertime";
            var res = await Poster.Get(url, null);
            if (string.IsNullOrEmpty(res)) {
                WriteLog.LogError("API錯誤: " + url);
                return null;
            }
            return JsonConvert.DeserializeObject<GetServerTime_Res>(res).ServerTime;
        }
        public class GetServerTime_Res {
            [JsonProperty("data")]
            public System.DateTimeOffset ServerTime;
        }

        public static async UniTask<DBPlayer> Signup(string _authType, string _authData, string _deviceUID) {
            string url = domain + "/game/signup";
            var signupData = new {
                AuthType = _authType,
                AuthData = _authData,
                DeviceUID = _deviceUID
            };
            string jsonBody = JsonConvert.SerializeObject(signupData);
            string res = await Poster.Post(url, jsonBody);
            if (string.IsNullOrEmpty(res)) {
                WriteLog.LogError("API 錯誤: " + url);
                return null;
            }
            var resp = JsonConvert.DeserializeObject<Signup_Res>(res);
            return resp.MyDBPlayer;
        }
        public class Signup_Res {
            [JsonProperty("data")]
            public DBPlayer MyDBPlayer;
        }

        public static async UniTask<DBPlayer> Signin(string _playerID, string _authType, string _authData, string _deviceType, string _deviceUID) {
            string url = domain + "/player/signin";
            var signinData = new {
                PlayerID = _playerID,
                AuthType = _authType,
                AuthData = _authData,
                DeviceType = _deviceType,
                DeviceUID = _deviceUID
            };
            string jsonBody = JsonConvert.SerializeObject(signinData);
            string res = await Poster.Post(url, jsonBody);
            if (string.IsNullOrEmpty(res)) {
                WriteLog.LogError("API 錯誤: " + url);
                return null;
            }
            var resp = JsonConvert.DeserializeObject<Signin_Res>(res);
            onSignin(resp.MyDBPlayer);
            return resp.MyDBPlayer;
        }
        public class Signin_Res {
            [JsonProperty("data")]
            public DBPlayer MyDBPlayer;
        }


    }

}