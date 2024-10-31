using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiators.Main {
    public class DBGladiator : DBData {

        [JsonProperty("ownerID")]
        public string OwnerID { get; set; }

        [JsonProperty("jsonGladiatorID")]
        public int JsonGladiatorID { get; set; }

        [JsonProperty("jsonSkillIDs")]
        public List<int> JsonSkillIDs { get; set; }

        [JsonProperty("jsonTraitIDs")]
        public List<int> JsonTraitIDs { get; set; }

        [JsonProperty("jsonEquipIDs")]
        public List<int> JsonEquipIDs { get; set; }

        [JsonProperty("hp")]
        public int HP { get; set; }

        [JsonProperty("curHP")]
        public int CurHP { get; set; }

        [JsonProperty("vigorRegon")]
        public double VigorRegon { get; set; }

        [JsonProperty("str")]
        public int STR { get; set; }

        [JsonProperty("def")]
        public int DEF { get; set; }

        [JsonProperty("mdef")]
        public int MDEF { get; set; }

        [JsonProperty("crit")]
        public double CRIT { get; set; }

        [JsonProperty("init")]
        public int INIT { get; set; }

        [JsonProperty("knockback")]
        public int Knockback { get; set; }
    }

}