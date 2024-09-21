using System.Numerics;

namespace Gladiators.Socket.Matchgame {
    public class STATE_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱
        public bool Myself { get; private set; }
        public int HPChange { get; private set; }
        public int CurHp { get; private set; }
        public int MaxHp { get; private set; }
    }


}