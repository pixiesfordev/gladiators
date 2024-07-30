namespace Gladiators.Socket.Matchgame {
    public class SetReady : SocketContent {
        //class名稱就是封包的CMD名稱
    }
    public class SetReady_ToClient : SocketContent {
        //class名稱就是封包的CMD名稱
        public bool[] PlayerReadies { get; private set; }
    }
}