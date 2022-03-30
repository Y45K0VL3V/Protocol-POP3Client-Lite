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
        public static string[] Pop3Commands = new string[]
        {
            "user",
            "pass",
            "stat",
            "list",
            "rert",
            "top",
            "dele",
            "quit"
        };

        const int ReadTimeOut = 1000;

        private static POP3Client s_client;
        private TcpClient _tcpClient;
        private SslStream _sslStream;
        private bool _isClosing = false;

        private POP3Client()
        {
            
        }

        public static POP3Client GetInstance()
        {
            return s_client ?? (s_client = new POP3Client());
        }

        public bool Connect(string host, int port)
        {
            try
            {
                _tcpClient = new TcpClient(host, port);
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
            _sslStream.Flush();
            _sslStream.Write(Encoding.ASCII.GetBytes(message + "\r\n"));
            if (String.Equals(message, "quit", StringComparison.CurrentCultureIgnoreCase))
                _isClosing = true;
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
                    _sslStream.Flush();
                    bytes = _sslStream.Read(buffer, 0, buffer.Length);
                    if (bytes != 5)
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

            if (_isClosing)
            {
                _tcpClient = null;
                _sslStream = null;
                _isClosing = false;
            }

            return serverAnswer.ToString() ?? "";
        }
    }
}
