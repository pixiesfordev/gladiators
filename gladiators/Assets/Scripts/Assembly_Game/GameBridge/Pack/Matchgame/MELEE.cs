using System.Collections.Generic;

namespace Gladiators.Socket.Matchgame {
    public class MELEE_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public PackMelee MyAttack { get; private set; }
        public PackMelee OpponentAttack { get; private set; }
        public int[] MyHandSkillIDs { get; private set; }
    }
    public class BEFORE_MELEE_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public int MySkillID { get; private set; }
        public int OpponentSkillID { get; private set; }
    }

    public class PackMelee {
        public int SkillID { get; private set; }
        public double MeleePos { get; private set; }
        public double Knockback { get; private set; }
        public double CurPos { get; private set; }
        public List<string> EffectTypes { get; private set; }
    }
}