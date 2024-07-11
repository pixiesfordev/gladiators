using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Scoz.Func;
using Cysharp.Threading.Tasks;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Tls;
using Org.BouncyCastle.Tls.Crypto;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;

namespace Gladiators.Socket {
    public partial class TcpClient : MonoBehaviour, INetworkClient {

        private async void Thread_Connect_TLS() {
            try {
                string hostname = "www.pixies.dev";
                int port = 443;
                WriteLog.LogColor("hostname=" + hostname, WriteLog.LogType.Connection);
                WriteLog.LogColor("port=" + port, WriteLog.LogType.Connection);

                socket.Connect(hostname, port);
                WriteLog.LogColor("Socket connect initiated", WriteLog.LogType.Connection);

                await UniTask.WaitUntil(() => socket.Connected, cancellationToken: cancellationToken);
                WriteLog.LogColor("Tcp connect success", WriteLog.LogType.Connection);

                NetworkStream networkStream = new NetworkStream(socket, ownsSocket: false);
                TlsClientProtocol tlsClientProtocol = new TlsClientProtocol(networkStream, networkStream);
                MyTlsClient tlsClient = new MyTlsClient(new BcTlsCrypto(new SecureRandom()));

                tlsClientProtocol.Connect(tlsClient);

                WriteLog.LogColor("TLS handshake completed", WriteLog.LogType.Connection);
                syncContext.Post(state => OnConnect(true), null);

                thread_receive = new Thread(() => Thread_Receive_TLS(tlsClientProtocol));
                thread_receive.Start();
            } catch (Exception e) {
                WriteLog.LogErrorFormat("(TCP)Socket send error: {0}", e.ToString());
                isTryConnect = false;

                cancellationSource.Cancel();
                syncContext.Post(state => OnConnect(false), null);
            }
        }

        private void Thread_Receive_TLS(TlsClientProtocol tlsClientProtocol) {
            string mseQueue = "";
            while (IsConnected) {
                try {
                    byte[] tmp = new byte[2048];
                    int length = tlsClientProtocol.Stream.Read(tmp, 0, tmp.Length);
                    if (length <= 0) {
                        break;
                    }

                    string msg = Encoding.UTF8.GetString(tmp, 0, length);
                    if (!string.IsNullOrEmpty(mseQueue)) {
                        msg = mseQueue + msg;
                        mseQueue = string.Empty;
                    }

                    string[] packets = msg.Split('\n');
                    int packetNumber = packets.Length;
                    if (msg.LastIndexOf('\n') != length - 1) {
                        mseQueue = mseQueue + packets[packets.Length - 1];
                        packetNumber--;
                    }
                    for (int i = 0; i < packetNumber; i++) {
                        if (!string.IsNullOrEmpty(packets[i])) {
                            string packet = packets[i];
                            syncContext.Post(state => OnReceiveMsg?.Invoke(packet), null);
                        }
                    }
                } catch (ThreadAbortException e) {
                    WriteLog.Log($"TcpClient Exception={e}");
                    WriteLog.LogWarning($"TcpClient Exception={e}");
                    break;
                } catch (Exception e) {
                    WriteLog.LogErrorFormat("Socket receive error: {0}", e.ToString());
                    break;
                }
            }
            try {
                socket.Close();
            } catch {
            }
        }

        public class MyTlsClient : DefaultTlsClient {
            public MyTlsClient(TlsCrypto crypto) : base(crypto) { }

            public override ProtocolVersion[] GetProtocolVersions() {
                return ProtocolVersion.TLSv12.DownTo(ProtocolVersion.TLSv10);
            }

            public override int[] GetCipherSuites() {
                return new int[] {
                    CipherSuite.TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256,
                    CipherSuite.TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384
                };
            }

            public override TlsAuthentication GetAuthentication() {
                return new MyTlsAuthentication();
            }
        }

        public class MyTlsAuthentication : TlsAuthentication {
            public void NotifyServerCertificate(TlsServerCertificate serverCertificate) {
                // 在這裡添加驗證邏輯
                var certList = serverCertificate.Certificate;
                // 您可以根據需要添加驗證邏輯，例如檢查證書的公鑰
            }

            public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest) {
                return null; // 如果不需要客戶端證書，返回null
            }
        }
    }
}
