using System.Collections.Generic;
using System.Numerics;

namespace Gladiators.Socket.Matchgame {

    public class GameState_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public string State { get; private set; }
    }
    public class GLADIATORSTATES_TOCLIENT : SocketContent {
        public long Time { get; private set; }
        //class名稱就是封包的CMD名稱
        public PACKGLADIATORSTATE MyState { get; private set; }
        public PACKGLADIATORSTATE OpponentState { get; private set; }
    }
    public class PACKGLADIATORSTATE {
        //class名稱就是封包的CMD名稱
        public double CurPos { get; private set; }
        public double CurSpd { get; private set; }
        public double CurVigor { get; private set; }
        public bool Rush { get; private set; }
        public List<string> EffectTypes { get; private set; }
    }

    public class Hp_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public string PlayerID { get; private set; }
        public int HPChange { get; private set; }
        public int CurHp { get; private set; }
        public int MaxHp { get; private set; }

    }

}