using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiators.Main {
    public class DBMatchgame : DBData {

        [JsonProperty("dbMapID")]
        public string DBMapID { get; set; }

        [JsonProperty("ip")]
        public string IP { get; set; }

        [JsonProperty("matchmakerPodName")]
        public string MatchmakerPodName { get; set; }

        [JsonProperty("nodeName")]
        public string NodeName { get; set; }

        [JsonProperty("playerIDs")]
        public string[] PlayerIDs { get; set; }  // 假設 PLAYER_NUMBER 是常數，定義該陣列大小

        [JsonProperty("podName")]
        public string PodName { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }
    }

}