namespace Gladiators.Socket.Matchgame {
    public class AUTO : SocketContent {
        /// <summary>
        /// �O�_�}��auto�Ҧ�
        /// </summary>
        public bool IsAuto { get; private set; }
        public AUTO(bool isAuto) {
            IsAuto = isAuto;
        }
    }
}
