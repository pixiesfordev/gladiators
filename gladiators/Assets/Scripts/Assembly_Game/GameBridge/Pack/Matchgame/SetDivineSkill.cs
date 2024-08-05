namespace Gladiators.Socket.Matchgame {
    public class SetDivineSkill : SocketContent {
        //class名稱就是封包的CMD名稱
        public int[] JsonSkillIDs { get; private set; }
        public SetDivineSkill(int[] _jsonSkillIDs) { JsonSkillIDs = _jsonSkillIDs; }
    }
    public class SetDivineSkill_ToClient : SocketContent {
        //class名稱就是封包的CMD名稱
        public PackPlayerState MyPlayerState { get; private set; }
        public PackPlayerState OpponentPlayerState { get; private set; }
    }
}