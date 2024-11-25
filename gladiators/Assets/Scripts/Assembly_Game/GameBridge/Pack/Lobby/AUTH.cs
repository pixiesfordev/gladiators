using System.Collections;
using UnityEngine;

namespace Gladiators.Socket.Lobby {
    public class AUTH : SocketContent {
        //class名稱就是封包的CMD名稱
        public string ConnToken { get; private set; }

        public AUTH(string token) {
            ConnToken = token;
        }
    }
    public class AUTH_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱

        public bool IsAuth { get; private set; }
        public long Time { get; private set; }

        public AUTH_TOCLIENT() {
        }
    }
}