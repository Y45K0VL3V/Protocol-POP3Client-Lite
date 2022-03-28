using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace yakov.Protocol.POP3.Client.Model
{
    public class POP3Client
    {
        const int ReadTimeOut = 1000;

        private static POP3Client s_client;
        private TcpClient _tcpClient;
        private SslStream _sslStream;

        private POP3Client()
        {
            _tcpClient = new TcpClient();
        }

        public static POP3Client GetInstance()
        {
            return s_client ?? (s_client = new POP3Client());
        }

        public bool Connect(string host, int port)
        {
            try
            {
                _tcpClient.Connect(host, port);
                _sslStream = new SslStream(_tcpClient.GetStream());
                _sslStream.ReadTimeout = ReadTimeOut;
                _sslStream.AuthenticateAsClient(host);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void Send(string message)
        {
            _sslStream.Write(Encoding.ASCII.GetBytes(message));
        }

        public string Receive()
        {
            var serverAnswer = new StringBuilder();
            byte[] buffer = new byte[1024];
            int bytes = default;
            do
            {
                try 
                {
                    bytes = _sslStream.Read(buffer, 0, buffer.Length);
                    serverAnswer.Append(Encoding.ASCII.GetString(buffer, 0, bytes));
                }
                catch
                {
                    break;
                }
                finally
                {
                    _sslStream.Flush();
                }
            } while (bytes > 0);
            
            return serverAnswer.ToString() ?? "";
        }
    }
}
