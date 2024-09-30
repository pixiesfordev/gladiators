namespace Gladiators.Socket.Matchgame {
    public class SETPLAYER : SocketContent {
        //class名稱就是封包的CMD名稱
        public string DBGladiatorID { get; private set; }
        public SETPLAYER(string _id) { DBGladiatorID = _id; }
    }

    public class SETPLAYER_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public long Time { get; private set; }
        public PackPlayer MyPackPlayer { get; private set; }
        public PackPlayer OpponentPackPlayer { get; private set; }
    }
    public class PackPlayer {
        public string DBID { get; private set; }
        public PackGladiator MyPackGladiator { get; private set; }
    }
    public class PackGladiator {
        public string DBID { get; private set; } //DBGladiator的DBID
        public int JsonID { get; private set; }      // Gladitaor的Json id
        public int[] SkillIDs { get; private set; }    // (玩家自己才會收到)
        public int[] HandSkillIDs { get; private set; }    // (玩家自己才會收到)
        public int MaxHP { get; private set; } // 最大生命
        public int CurHp { get; private set; }      // 目前生命
        public double CurVigor { get; private set; }  // 目前體力
        public double CurSpd { get; private set; }// 目前速度
        public double CurPos { get; private set; }// 目前位置
        public string[] EffectTypes { get; private set; }  // 狀態清單
    }
}