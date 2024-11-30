using System.Collections.Generic;

namespace Gladiators.Socket.Matchgame {
    public class MELEE_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public PackMelee MyAttack { get; private set; }
        public PackMelee OpponentAttack { get; private set; }
        public int NewSkilID { get; private set; }   // 新抽到的技能
        public int SkillOnID { get; private set; }   // 啟用中的肉搏技能        
        public int[] MyHandSkillIDs { get; private set; } // 目前手牌
    }
    public class PackMelee {
        public int SkillID { get; private set; }
        public double MeleePos { get; private set; }
        public double BeKnockback { get; private set; }
        public double CurPos { get; private set; }
        public List<PackEffect> EffectDatas { get; private set; }
    }
    public class BEFORE_MELEE_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public int MySkillID { get; private set; }
        public int OpponentSkillID { get; private set; }
    }
    public class LOCK_INSTANT_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public bool Lock { get; private set; }
    }

}