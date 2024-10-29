using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiators.Main {
    public class DBAddress : DBData {

        [JsonProperty("storeURL_Apple")]
        public string StoreURL_Apple { get; set; }

        [JsonProperty("storeURL_Google")]
        public string StoreURL_Google { get; set; }

    }

}