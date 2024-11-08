namespace Gladiators.Socket.Matchgame {
    public class GMACTION : SocketContent {
        public enum GMActionType {
            GMACTION_SETGLADIATOR,
        }
        //class名稱就是封包的CMD名稱
        public string ActionType { get; private set; }
        public object ActionContent { get; private set; }
        public GMACTION(string _actionType, object _actionContent) {
            ActionType = _actionType;
            ActionContent = _actionContent;
        }
    }
    public class GMACTION_TOCLIENT<T> : SocketContent {
        //class名稱就是封包的CMD名稱
        public string PlayerDBID { get; private set; }
        public string ActionType { get; private set; }
        public bool Result { get; private set; }
        public T ActionContent { get; private set; }

    }
    public class PackGMAction_SetGladiator {
        public int GladiatorID { get; private set; }
        public int[] SkillIDs { get; private set; }
        public PackGMAction_SetGladiator(int _gladiatorID, int[] _skillIDs) {
            GladiatorID = _gladiatorID;
            SkillIDs = _skillIDs;
        }
    }
}
