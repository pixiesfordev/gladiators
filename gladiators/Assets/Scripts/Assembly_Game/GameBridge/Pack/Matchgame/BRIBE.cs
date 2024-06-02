namespace Gladiators.Socket.Matchgame {
    public class BRIBE : SocketContent {
        //class名稱就是封包的CMD名稱
        public int[] JsonBribeIDs { get; private set; }
        public BRIBE(int[] _jsonBribeIDs) {
            JsonBribeIDs = _jsonBribeIDs;
        }
    }
    public class BRIBE_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public PackPlayerState[] PlayerStates { get; private set; }
    }
}