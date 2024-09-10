namespace Gladiators.Socket.Matchgame {
    public class CARDSTATE : SocketContent {
        //class名稱就是封包的CMD名稱
    }
    public class CARDSTATE_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public PackCardState MyCardState { get; private set; }
    }
    public class PackCardState {
        public int[] HandSkillIDs { get; private set; }
        public int HandOnID { get; private set; }
        public int[] DivineSkillIDs { get; private set; }
        public int DivineSkillOnID { get; private set; }
    }
}