namespace Gladiators.Socket.Matchgame {
    public class PLAYERACTION : SocketContent {
        public enum PackActionType {
            ACTION_SKILL,
            ACTIVE_MELEE_SKILL,
            INSTANT_SKILL,
            ACTION_OPPONENTSKILL,
            ACTION_RUSH,
            ACTION_DIVINESKILL,
            ACTION_SURRENDER,
        }
        //class名稱就是封包的CMD名稱
        public string ActionType { get; private set; }
        public object ActionContent { get; private set; }
        public PLAYERACTION(string _actionType, object _actionContent) {
            ActionType = _actionType;
            ActionContent = _actionContent;
        }
    }
    public class PLAYERACTION_TOCLIENT<T> : SocketContent {
        //class名稱就是封包的CMD名稱
        public string PlayerDBID { get; private set; }
        public string ActionType { get; private set; }
        public T ActionContent { get; private set; }

    }
    public class PackAction_Skill {
        public bool On { get; private set; }
        public int SkillID { get; private set; }
        public PackAction_Skill(bool _on, int _skillID) {
            On = _on;
            SkillID = _skillID;
        }
    }
    // 啟用肉搏技能
    public class PackAction_ActiveMeleeSkill_ToClient {
        public bool On { get; private set; } // 啟用或關閉
        public int SkillID { get; private set; } // 肉搏技能ID
    }
    public class PackAction_InstantSkill_ToClient {
        public int SkillID { get; private set; }  // 施放的即時技能ID
        public int NewSkilID { get; private set; } // 新抽到的技能(對手不會收到)
        public int[] HandSkills { get; private set; } // 手牌(對手不會收到)
    }
    public class PackAction_DivineSkill {
        public bool On { get; private set; }
        public int SkillID { get; private set; }
        public PackAction_DivineSkill(bool _on, int _skillID) {
            On = _on;
            SkillID = _skillID;
        }
    }
    public class PackAction_DivineSkill_ToClient {
        public bool On { get; private set; }
        public int SkillID { get; private set; }
    }
    public class PackAction_Rush {
        public bool On { get; private set; }
        public PackAction_Rush(bool _on) {
            On = _on;
        }
    }
    public class PackAction_Surrender {
    }
    public class PackAction_Surrender_ToClient {
    }
}
