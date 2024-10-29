using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiators.Main {
    public class DBPlayer {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("createdAt")]
        public System.DateTime CreatedAt { get; set; }

        [JsonProperty("authDatas")]
        public Dictionary<string, string> AuthDatas { get; set; }

        [JsonProperty("authType")]
        public string AuthType { get; set; }

        [JsonProperty("connToken")]
        public string ConnToken { get; set; }

        [JsonProperty("gold")]
        public int Gold { get; set; }

        [JsonProperty("point")]
        public int Point { get; set; }

        [JsonProperty("onlineState")]
        public string OnlineState { get; set; }

        [JsonProperty("lastSigninAt")]
        public System.DateTime LastSigninAt { get; set; }

        [JsonProperty("lastSignoutAt")]
        public System.DateTime LastSignoutAt { get; set; }

        [JsonProperty("ban")]
        public bool Ban { get; set; }

        [JsonProperty("deviceType")]
        public string DeviceType { get; set; }

        [JsonProperty("deviceUID")]
        public string DeviceUID { get; set; }

        [JsonProperty("inMatchgameID")]
        public string InMatchgameID { get; set; }

        [JsonProperty("myGladiatorID")]
        public string MyGladiatorID { get; set; }
    }
}