namespace Gladiators.Socket.Matchgame {
    public class SETDIVINESKILL : SocketContent {
        //class名稱就是封包的CMD名稱
        public int[] JsonSkillIDs { get; private set; }
        public SETDIVINESKILL(int[] _jsonSkillIDs) { JsonSkillIDs = _jsonSkillIDs; }
    }
    public class SETDIVINESKILL_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public int[] JsonSkillIDs { get; private set; }
    }
}