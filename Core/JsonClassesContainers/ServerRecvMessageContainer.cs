using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core.JsonClassesContainers
{
    public class ServerRecvMessageContainer
    {
        public string sender_name { get; set; }
        public string sender_nickname { get; set; }
        public string whom { get; set; }
        public byte[] content { get; set; }
        public string type { get; set; }
    }
}
