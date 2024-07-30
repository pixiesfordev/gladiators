namespace Gladiators.Socket.Matchgame {
    public class Auth : SocketContent {
        //class名稱就是封包的CMD名稱
        public string Token { get; private set; }

        public Auth(string token) {
            Token = token;
        }
    }
    public class Auth_ToClient : SocketContent {
        //class名稱就是封包的CMD名稱
        public bool IsAuth { get; private set; }
        public string ConnToken { get; private set; } // Auth驗證成功後之後UDP要連Matchgame都是透過這個連線Token
    }
}