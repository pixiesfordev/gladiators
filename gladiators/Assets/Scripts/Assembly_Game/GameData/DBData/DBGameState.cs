using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiators.Main {
    public class DBGameState : DBData {

        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; }

        [JsonProperty("maintain")]
        public bool Maintain { get; set; }

        [JsonProperty("maintainEndAt")]
        public DateTime MaintainEndAt { get; set; }

        [JsonProperty("maintainExemptPlayerIDs")]
        public List<string> MaintainExemptPlayerIDs { get; set; }

        [JsonProperty("minGameVersion")]
        public string MinimumGameVersion { get; set; }

        [JsonProperty("matchgame-testver-roomName")]
        public string MatchgameTestverRoomName { get; set; }

        [JsonProperty("matchgame-testver-mapID")]
        public string MatchgameTestverMapID { get; set; }

        [JsonProperty("matchgame-testver-port")]
        public int MatchgameTestverPort { get; set; }

        [JsonProperty("matchgame-testver-tcp-ip")]
        public string MatchgameTestverTcpIP { get; set; }

        [JsonProperty("lobbyIP")]
        public string LobbyIP { get; set; }

        [JsonProperty("lobbyEnable")]
        public bool LobbyEnable { get; set; }

        [JsonProperty("lobbyPort")]
        public int LobbyPort { get; set; }
    }


}