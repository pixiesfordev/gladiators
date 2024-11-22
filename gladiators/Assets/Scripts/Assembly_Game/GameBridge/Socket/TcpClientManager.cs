using System;
using System.Net.Sockets;
using System.Text;
using Cysharp.Threading.Tasks;
using Gladiators.Socket;
using Scoz.Func;

public class TcpClientManager {


    private TcpClient client;
    private NetworkStream stream;
    public bool IsConnected { get; private set; }

    public Action OnConnected;      // 連線成功事件
    public Action OnDisconnected;  // 斷線事件
    public Action<string> OnPacketReceived; // 接收封包

    /// <summary>
    /// 非同步連接到指定 IP 和 Port
    /// </summary>
    /// <param name="ip">伺服器 IP</param>
    /// <param name="port">伺服器 Port</param>
    public async UniTask ConnectAsync(string ip, int port) {
        try {
            client = new TcpClient();
            await client.ConnectAsync(ip, port); // 使用 UniTask 進行非同步連線
            stream = client.GetStream();
            IsConnected = true;

            // 呼叫連線成功事件
            OnConnected?.Invoke();

            WriteLog.LogColor($"已連線到伺服器 {ip}:{port}", WriteLog.LogType.Connection);
            DeviceManager.AddOnApplicationQuitAction(() => {
                Disconnect();
            });
            // 啟動接收數據的任務
            ReceiveDataAsync().Forget();
        } catch (Exception ex) {
            WriteLog.LogError($"連線失敗: {ex.Message}");
        }
    }

    /// <summary>
    /// 非同步發送封包到伺服器
    /// </summary>
    /// <typeparam name="T">封包內容的類型</typeparam>
    /// <param name="socketCMD">SocketCMD 封包</param>
    public async UniTask SendPacketAsync<T>(SocketCMD<T> socketCMD) where T : SocketContent {
        if (!IsConnected) {
            WriteLog.LogError("尚未連線，無法發送封包。");
            return;
        }

        try {
            // 將封包序列化成 JSON 格式
            string serializedPacket = LitJson.JsonMapper.ToJson(socketCMD);
            byte[] data = Encoding.UTF8.GetBytes(serializedPacket);
            // 發送封包
            await stream.WriteAsync(data, 0, data.Length);
            //WriteLog.LogColor($"已發送封包: {serializedPacket}", WriteLog.LogType.Connection);
        } catch (Exception ex) {
            WriteLog.LogError($"封包發送失敗: {ex.Message}");
        }
    }



    /// <summary>
    /// 非同步接收伺服器數據
    /// </summary>
    private async UniTaskVoid ReceiveDataAsync() {
        try {
            byte[] buffer = new byte[1024];
            StringBuilder dataBuffer = new StringBuilder();

            while (IsConnected) {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead == 0) {
                    // 伺服器已關閉連線
                    Disconnect();
                    break;
                }

                // 將接收到的字節轉換為字串，並添加到緩衝區
                string receivedChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                dataBuffer.Append(receivedChunk);

                // 檢查是否有完整的封包（以換行符號作為封包結束標誌）
                string data = dataBuffer.ToString();
                int packetEndIndex;
                while ((packetEndIndex = data.IndexOf('\n')) != -1) {
                    // 提取一個完整的封包
                    string completePacket = data.Substring(0, packetEndIndex);
                    data = data.Substring(packetEndIndex + 1);

                    // 處理完整的封包
                    OnPacketReceived?.Invoke(completePacket);
                }

                // 將未處理完的數據重新放回緩衝區
                dataBuffer.Clear();
                dataBuffer.Append(data);
            }
        } catch (Exception ex) {
            WriteLog.LogError($"接收封包錯誤: {ex.Message}");
            Disconnect();
        }
    }


    /// <summary>
    /// 主動斷開連線
    /// </summary>
    public void Disconnect() {
        if (!IsConnected) return;

        try {
            IsConnected = false;
            stream?.Close();
            client?.Close();

            // 呼叫斷線事件
            OnDisconnected?.Invoke();

            WriteLog.LogColor("已斷開連線。", WriteLog.LogType.Connection);
        } catch (Exception ex) {
            WriteLog.LogError($"斷線失敗: {ex.Message}");
        }
    }

}
