using System.Numerics;

namespace Gladiators.Socket.Matchgame {
    public class BATTLESTATE : SocketContent {
        //class名稱就是封包的CMD名稱
    }
    public class BATTLESTATE_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public PackPlayerBribe[] PlayerBribes { get; private set; }
    }
    public class PackPlayerState {
        public PackSkill[] Skills { get; private set; }
        public PackBribeSkill[] BribeSkills { get; private set; }
        public PackGladiator[] Gladiators { get; private set; }
    }
    public class PackSkill {
        public int JsonID { get; private set; }
        public bool On { get; private set; }
    }
    public class PackBribeSkill {
        public int JsonID { get; private set; }
        public bool Used { get; private set; }
    }
    public class PackGladiator {
        public int HP { get; private set; }
        public int Vigor { get; private set; }
        public int BattlePos { get; private set; }
        public Vector2 StagePos { get; private set; }
        public PackBuffer[] Buffers { get; private set; }
    }
    public class PackBuffer {
        public int JsonID { get; private set; }
        public int Stack { get; private set; }
    }
}