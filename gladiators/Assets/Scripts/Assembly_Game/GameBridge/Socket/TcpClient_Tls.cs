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
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using System.Linq;

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
                string expectedPublicKey = @"-----BEGIN PUBLIC KEY-----
3082010A0282010100C1D373FA79C9595610B0917C594D7758FCB67223FECAA0BFF78CF44022
18925633B83F8A74B838ECB668B480519D3939C645CC9D1912A5BC098B8A477BF855B24B9784
FAD9ABF6C5AED41D631FC6D0886430E96038564FA4E44B2E37DD295245455A0D8BA549F00C9C
D4CF82D79BCE0735779BFC0581AACF164A6A36BA87D9E0FF82D78790C2F0295880CC062074A9
3D9E427FF6C05E4B4D4EA081CCE61C50781C3B9D35EDAB5E297EEC831897D354A3DD056F4E77
26F72506C24716F346BB346192C510E56976B0E73BFA4571BECDFA19DC2737C8D26EDD82F2A9
96DFFD2BA4983974845495D6CC3F58F00DC63541FAFD7659D7E517EC16EB69D6A6ED98B4B502
03010001
-----END PUBLIC KEY-----";

                MyTlsClient tlsClient = new MyTlsClient(new BcTlsCrypto(new SecureRandom()), expectedPublicKey);

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

                    // 記錄接收到的數據包長度和部分內容
                    WriteLog.LogColor($"Received packet of length: {length}", WriteLog.LogType.Connection);
                    WriteLog.LogColor($"Received data: {BitConverter.ToString(tmp, 0, Math.Min(length, 64))}", WriteLog.LogType.Connection); // 只記錄前64字節

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
            private readonly string expectedPublicKey;

            public MyTlsClient(TlsCrypto crypto, string expectedPublicKey) : base(crypto) {
                this.expectedPublicKey = expectedPublicKey;
            }

            public override TlsAuthentication GetAuthentication() {
                return new MyTlsAuthentication(expectedPublicKey);
            }
        }


        public class MyTlsAuthentication : TlsAuthentication {
            private readonly string expectedPublicKey;

            public MyTlsAuthentication(string expectedPublicKey) {
                this.expectedPublicKey = expectedPublicKey;
            }

            public void NotifyServerCertificate(TlsServerCertificate serverCertificate) {
                var cert = serverCertificate.Certificate.GetCertificateAt(0);
                X509CertificateStructure x509Cert = X509CertificateStructure.GetInstance(cert.GetEncoded());
                SubjectPublicKeyInfo keyInfo = x509Cert.SubjectPublicKeyInfo;

                // 將公鑰轉換為 PEM 格式
                var keyBytes = keyInfo.GetEncoded();
                string pemPublicKey = Convert.ToBase64String(keyBytes);

                // 去除 PEM 文件中的 "-----BEGIN PUBLIC KEY-----" 和 "-----END PUBLIC KEY-----"
                string formattedPemPublicKey = $"-----BEGIN PUBLIC KEY-----\n{pemPublicKey}\n-----END PUBLIC KEY-----";

                WriteLog.LogError("formattedPemPublicKey=" + formattedPemPublicKey);
                WriteLog.LogError("expectedPublicKey=" + expectedPublicKey);

                if (formattedPemPublicKey == expectedPublicKey) {

                    Debug.Log("Server certificate validated successfully.");
                } else {
                    //throw new TlsFatalAlert(AlertDescription.bad_certificate);
                }
            }

            public TlsCredentials GetClientCredentials(CertificateRequest certificateRequest) {
                // 如果不需要客戶端證書，返回null
                return null;
            }
        }



    }
}
