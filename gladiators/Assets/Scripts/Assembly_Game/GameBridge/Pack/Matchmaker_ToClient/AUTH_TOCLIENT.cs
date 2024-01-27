using System.Collections;
using UnityEngine;

namespace Gladiators.Socket.Matchmaker {
    public class AUTH_TOCLIENT : SocketContent {
        //class名稱就是封包的CMD名稱

        public bool IsAuth { get; private set; }

        public AUTH_TOCLIENT() {
        }
    }
}