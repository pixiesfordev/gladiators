using System.Numerics;

namespace Gladiators.Socket.Matchgame {
    public class MELEE_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public PackPlayerState MyPlayerState { get; private set; }
        public PackPlayerState OpponentPlayerState { get; private set; }
        public PackAttack MyAttack { get; private set; }
        public PackAttack OpponentAttack { get; private set; }
        public double GameTime { get; private set; }
    }

    public class PackAttack {
        public double AttackPos { get; private set; }
        public double Knockback { get; private set; }
        public int SkillID { get; private set; }
    }
}