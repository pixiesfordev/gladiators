using Cysharp.Threading.Tasks;
using Gladiators.Socket;
using Scoz.Func;
using System;
using System.Collections.Generic;


namespace Gladiators.Main {
    public class GameConnector {
        public string Name { get; private set; }
        TcpClientManager tcpClient;
        Action onConnectedActions;
        Action onDisconnectedActions;
        Action<string> receiveAc;
        private static readonly object dicLocker = new object();
        static Dictionary<string, GameConnector> tcpClients = new Dictionary<string, GameConnector>();
        static HashSet<string> connectings = new HashSet<string>();

        public static async UniTask<bool> NewConnector(
            string _name,
            string _ip,
            int _port,
            Action _onConnectedAc,
            Action _onDisconnectedAc) {
            WriteLog.LogColor($"建立 {_name} 連線 Ip: {_ip} Port: {_port}", WriteLog.LogType.Connection);

            // 名稱已存在
            if (tcpClients.ContainsKey(_name)) {
                WriteLog.LogError($"嘗試連線已連線中的Server({_name})");
                return false;
            }

            // 已在連線中
            if (connectings.Contains(_name)) {
                WriteLog.LogError($"嘗試連線，目前正在連線中的Server({_name})，可能是重複呼叫了");
                return false;
            }

            connectings.Add(_name); // 加入連線中的Server清單，在連線成功/失敗之後才會移除，否則會判斷為錯誤呼叫了重複的連線

            var connector = new GameConnector {
                Name = _name,
                tcpClient = new TcpClientManager()
            };
            connector.tcpClient.OnConnected = () => connector.onConnectedToMatchGame(connector);
            connector.tcpClient.OnDisconnected = () => connector.onDisconnectedToMatchGame(connector);
            connector.tcpClient.OnPacketReceived += connector.receive;

            if (_onConnectedAc != null) connector.onConnectedActions += _onConnectedAc;
            if (_onDisconnectedAc != null) connector.onDisconnectedActions += _onDisconnectedAc;

            try {
                // 連線到伺服器
                await connector.tcpClient.ConnectAsync(_ip, _port);
                // 連線成功時
                return true;
            } catch (Exception ex) {
                WriteLog.LogError($"連線到伺服器 {_name} 時失敗: {ex.Message}");
                // 如有需要也可以把 tcpClient 設為 null
                connector.tcpClient = null;
                return false;
            } finally {
                // 不論成功或失敗，都移除「連線中」狀態
                connectings.Remove(_name);
            }
        }

        public void RegisterOnPacketReceived(Action<string> _ac) {
            if (!tcpClient.IsConnected) {
                WriteLog.LogError($" RegisterOnPacketReceived 失敗，因為尚未連線到伺服器 {Name}");
                return;
            }
            receiveAc += _ac;
        }

        public void UnregisterOnPacketReceived(Action<string> _ac) {
            if (!tcpClient.IsConnected) {
                WriteLog.LogError($" UnregisterOnPacketReceived 失敗，因為尚未連線到伺服器 {Name}");
                return;
            }
            receiveAc -= _ac;
        }
        void onConnectedToMatchGame(GameConnector _connector) {
            connectings.Remove(_connector.Name);
            if (!tcpClients.ContainsKey(_connector.Name)) {
                lock (dicLocker) {
                    tcpClients.Add(_connector.Name, _connector);
                }
            } else {
                WriteLog.LogError($"重複連線 {_connector.Name}");
                return;
            }
            WriteLog.LogColor($"成功連線到 {_connector.Name}！", WriteLog.LogType.Connection);
            onConnectedActions?.Invoke();
        }
        void onDisconnectedToMatchGame(GameConnector _connector) {
            connectings.Remove(_connector.Name);
            if (tcpClients.ContainsKey(_connector.Name)) {
                lock (dicLocker) {
                    tcpClients.Remove(_connector.Name);
                }
            } else {
                WriteLog.LogError($"要移除的連線不存在 {_connector.Name}");
            }

            WriteLog.LogColor($"已跟 {_connector.Name} 連線已中斷！", WriteLog.LogType.Connection);
            onDisconnectedActions?.Invoke();
        }

        public static GameConnector GetConnector(string _serverName) {
            lock (dicLocker) {
                if (!tcpClients.ContainsKey(_serverName)) {
                    WriteLog.LogError($"嘗試取得的 server({_serverName}) 不存在");
                    return null;
                }
                return tcpClients[_serverName];
            }
        }

        public void Disconnect() {
            if (tcpClient != null) {
                tcpClient.Disconnect();
                onConnectedActions = null;
                onDisconnectedActions = null;
                tcpClient = null;
            }
        }
        void receive(string _msg) {
            receiveAc?.Invoke(_msg);
        }
        public void Send<T>(SocketCMD<T> packet) where T : SocketContent {
            if (tcpClient == null) {
                WriteLog.LogError("tcpClient為null");
                return;
            }
            tcpClient.SendPacketAsync(packet).Forget();
        }


    }

}