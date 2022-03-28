using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.Protocol.POP3.Client.Model
{
    public class POP3Client
    {
        private static POP3Client _client;

        private POP3Client()
        { }

        public static POP3Client GetInstance()
        {
            return _client ?? (_client = new POP3Client());
        }
    }
}
