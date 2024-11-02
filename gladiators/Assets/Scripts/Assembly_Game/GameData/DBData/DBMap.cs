using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiators.Main {
    public class DBMap : DBData {

        [JsonProperty("matchType")]
        public string MatchType { get; set; }

        [JsonProperty("jsonMapID")]
        public int JsonMapID { get; set; }

        [JsonProperty("enable")]
        public bool Enable { get; set; }
    }

}