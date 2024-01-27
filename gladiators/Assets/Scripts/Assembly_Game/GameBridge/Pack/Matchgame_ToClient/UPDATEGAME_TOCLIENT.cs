using System.Collections;
using UnityEngine;

namespace Gladiators.Socket.Matchgame {
    public class UPDATEGAME_TOCLIENT : SocketContent {
        /// <summary>
        /// 遊戲開始X秒
        /// </summary>
        public double GameTime { get; private set; }

        public UPDATEGAME_TOCLIENT() {
        }
    }
}