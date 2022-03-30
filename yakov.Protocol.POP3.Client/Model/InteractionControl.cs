using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.Protocol.POP3.Client.Model
{
    public static class InteractionControl
    {
        private static readonly POP3Client _pop3Client;
        static InteractionControl()
        {
            _pop3Client = POP3Client.GetInstance();
        }
        public static string ClientConnect(string host, int port)
        {
            if (_pop3Client.Connect(host, port))
                return _pop3Client.Receive();
            else
                return null;
        }

        public static string Execute(string command)
        {
            _pop3Client.Send(command);
            return _pop3Client.Receive().Substring(command.Length > 5 ? 5 : 0);
        }

        public static bool IsCommandAvaliable(string command)
        {
            return POP3Client.Pop3Commands.Contains(command?.Split(new char[] { ' ' })[0].ToLower()) ? true : false;
        }
    }
}
