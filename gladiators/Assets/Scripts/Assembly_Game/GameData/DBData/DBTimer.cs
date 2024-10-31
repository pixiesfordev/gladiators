using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiators.Main {
    public class DBTimer : DBData {

        [JsonProperty("playerOfflineSecs")]
        public int PlayerOfflineSecs { get; set; }
    }

}