namespace Gladiators.Socket.Matchgame {
    public class SETREADY : SocketContent {
        //class名稱就是封包的CMD名稱
    }
    public class SETREADY_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public bool[] PlayerReadies { get; private set; }
    }
}