using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCorpApp.Contracts.Notification
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
