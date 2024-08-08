namespace Gladiators.Socket.Matchgame {
    public interface IActionContent { }
    public class PLAYERACTION : SocketContent {
        //class名稱就是封包的CMD名稱
        public string ActionType { get; private set; }
        public IActionContent ActionContent { get; private set; }
        public PLAYERACTION(string _actionType, IActionContent _actionContent) {
            ActionType = _actionType;
            ActionContent = _actionContent;
        }
    }
    public class PLAYERACTION_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public string PlayerDBID { get; private set; }
        public string ActionType { get; private set; }
        public IActionContent ActionContent { get; private set; }
    }
    public class PackAction_Skill : IActionContent {
        public bool On { get; private set; }
        public int SkillID { get; private set; }
        public PackAction_Skill(bool _on, int _skillID) {
            On = _on;
            SkillID = _skillID;
        }
    }
    public class PackAction_Skill_ToClient : IActionContent {
        public bool On { get; private set; }
        public int SkillID { get; private set; }
    }
    public class PackAction_DivineSkill : IActionContent {
        public bool On { get; private set; }
        public int SkillID { get; private set; }
        public PackAction_DivineSkill(bool _on, int _skillID) {
            On = _on;
            SkillID = _skillID;
        }
    }
    public class PackAction_DivineSkill_ToClient : IActionContent {
        public bool On { get; private set; }
        public int SkillID { get; private set; }
    }
    public class PackAction_Rush : IActionContent {
        public bool On { get; private set; }
        public PackAction_Rush(bool _on) {
            On = _on;
        }
    }
    public class PackAction_Rush_ToClient : IActionContent {
        public bool On { get; private set; }
    }
    public class PackAction_Surrender : IActionContent {
    }
    public class PackAction_Surrender_ToClient : IActionContent {
    }
}
