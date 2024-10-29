using Newtonsoft.Json;
using Scoz.Func;

namespace Gladiators.Main {
    public class DBData {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("createdAt")]
        public System.DateTime CreatedAt { get; set; }
    }
    public enum DBDataType {
        // 遊戲設定
        GameState,
        Address,
        Timer,
        GameLog,
        Map,
        // 玩家資料
        Player,
        PlayerHistory,
        Gladiator,
        // 遊戲資料
        Matchgame,
    }

}