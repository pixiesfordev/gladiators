using Cysharp.Threading.Tasks;
using Gladiators.Socket;
using Scoz.Func;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditor.MemoryProfiler;


namespace Gladiators.Main {
    public class GameConnector {
        public string Name { get; private set; }
        TcpClientManager tcpClient;
        Action onConnectedActions;
        Action onDisconnectedActions;
        private static readonly object dicLocker = new object();
        static Dictionary<string, GameConnector> tcpClients = new Dictionary<string, GameConnector>();
        static HashSet<string> connectings = new HashSet<string>();

        public static async UniTask NewConnector(string _name, string _ip, int _port, Action _onConnectedAc, Action _onDisconnectedAc) {
            var connector = new GameConnector();
            if (tcpClients.ContainsKey(_name)) {
                WriteLog.LogError($"嘗試連線已連線中的Server({_name})");
                return;
            }
            if (connectings.Contains(_name)) {
                WriteLog.LogError($"嘗試連線，目前正在連線中的Server({_name})，可能是重複呼叫了");
                return;
            }
            connectings.Add(_name); // 加入連線中的Server清單，在連線成功/失敗之後才會移除，否則會判斷為錯誤呼叫了重複的連線
            connector.Name = _name;
            connector.tcpClient = new TcpClientManager();
            connector.tcpClient.OnConnected = () => connector.onConnectedToMatchGame(connector);
            connector.tcpClient.OnDisconnected = () => connector.onDisconnectedToMatchGame(connector);
            if (_onConnectedAc != null) connector.onConnectedActions += _onConnectedAc;
            if (_onDisconnectedAc != null) connector.onDisconnectedActions += _onDisconnectedAc;
            // 連線到伺服器
            try {
                await connector.tcpClient.ConnectAsync(_ip, _port);
            } catch (Exception ex) {
                WriteLog.LogError($"連線到伺服器 {_name} 時失敗: {ex.Message}");
                connector.tcpClient = null;
                return;
            }
        }
        public void RegisterOnPacketReceived(Action<string> _ac) {
            if (!tcpClient.IsConnected) {
                WriteLog.LogError($" RegisterOnPacketReceived 失敗，因為尚未連線到伺服器 {Name}");
                return;
            }
            tcpClient.OnPacketReceived += _ac;
        }

        public void UnregisterOnPacketReceived(Action<string> _ac) {
            if (!tcpClient.IsConnected) {
                WriteLog.LogError($" UnregisterOnPacketReceived 失敗，因為尚未連線到伺服器 {Name}");
                return;
            }
            tcpClient.OnPacketReceived -= _ac;
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
            tcpClient.Disconnect();
            onConnectedActions = null;
            onDisconnectedActions = null;
            tcpClient = null;
        }
        public void Send<T>(SocketCMD<T> packet) where T : SocketContent {
            tcpClient.SendPacketAsync(packet).Forget();
        }


    }

}