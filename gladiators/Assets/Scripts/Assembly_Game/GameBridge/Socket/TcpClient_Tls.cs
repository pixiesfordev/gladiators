using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Scoz.Func;
using Cysharp.Threading.Tasks;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Gladiators.Socket {
    public partial class TcpClient : MonoBehaviour, INetworkClient {


        SslStream sslStream;

        private async void Thread_Connect_TLS() {
            try {
                WriteLog.LogColor("IP=" + IP, WriteLog.LogType.Connection);
                WriteLog.LogColor("Port=" + Port, WriteLog.LogType.Connection);

                // 连接到服务器
                socket.Connect(IPAddress.Parse(IP), Port);
                WriteLog.LogColor("Socket connect initiated", WriteLog.LogType.Connection);

                await UniTask.WaitUntil(() => socket.Connected, cancellationToken: cancellationToken);
                syncContext.Post(state => OnConnect(true), null);

                WriteLog.LogColor("Tcp connect success", WriteLog.LogType.Connection);

                // 使用 NetworkStream 和 SslStream 进行 TLS 连接
                NetworkStream networkStream = new NetworkStream(socket, ownsSocket: false);
                sslStream = new SslStream(networkStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

                // 添加日志以调试 TLS 握手
                WriteLog.LogColor("Starting TLS handshake", WriteLog.LogType.Connection);

                // 执行 TLS 握手
                await UniTask.Run(() => sslStream.AuthenticateAsClient(IP), cancellationToken: cancellationToken);

                WriteLog.LogColor("TLS handshake completed", WriteLog.LogType.Connection);

                thread_receive = new Thread(Thread_Receive_TLS);
                thread_receive.Start();
            } catch (Exception e) {
                WriteLog.LogErrorFormat("(TCP)Socket send error: {0}", e.ToString());
                isTryConnect = false;

                cancellationSource.Cancel();
                syncContext.Post(state => OnConnect(false), null);
            }
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;
            WriteLog.LogError("Certificate error: " + sslPolicyErrors);
            return false;
        }

        private void Thread_Receive_TLS() {
            string mseQueue = "";
            while (IsConnected) {
                try {
                    //if (socket.Available <= 0) continue;
                    byte[] tmp = new byte[2048];
                    int length = sslStream.Read(tmp, 0, tmp.Length); // 修改这一行
                    if (length <= 0) {
                        //OnDisConnect();
                        break;
                    }

                    string msg = Encoding.UTF8.GetString(tmp, 0, length);
                    //WriteLog.Log("Socket Recieve " + msg + " " + length);
                    if (!string.IsNullOrEmpty(mseQueue)) {
                        msg = mseQueue + msg;
                        mseQueue = string.Empty;
                    }

                    //黏包
                    string[] packets = msg.Split('\n');
                    int packetNumber = packets.Length;
                    //分包 最后一包被截断
                    if (msg.LastIndexOf('\n') != length - 1) {
                        mseQueue = mseQueue + packets[packets.Length - 1];
                        packetNumber--;
                    }
                    for (int i = 0; i < packetNumber; i++) {
                        if (!string.IsNullOrEmpty(packets[i])) {
                            string packet = packets[i];
                            syncContext.Post(state => OnReceiveMsg?.Invoke(packet), null);
                        }
                        //messageQueue.Enqueue(packets[i]);
                    }
                } catch (ThreadAbortException e) {
                    //this.Close();
                    WriteLog.Log($"TcpClient Exception={e}");
                    WriteLog.LogWarning($"TcpClient Exception={e}");
                    break;
                } catch (Exception e) {
                    WriteLog.LogErrorFormat("Socket receive error: {0}", e.ToString());
                    //OnDisConnect();
                    break;
                }
            }
            try {
                socket.Close();
            } catch {
            }
        }
    }

}
