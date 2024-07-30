namespace Gladiators.Socket.Matchgame {
    public class EndGame_ToClient : SocketContent {
        //class名稱就是封包的CMD名稱
        public string Result { get; private set; }
        public PackPlayerResult[] PlayerResults { get; private set; }
    }
    public class PackPlayerResult {
        public string Result { get; private set; }
        public string DBPlayerID { get; private set; }
        public int GainGold { get; private set; }
        public int GainEXP { get; private set; }
    }
}