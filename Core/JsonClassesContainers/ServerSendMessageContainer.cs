using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core.JsonClassesContainers
{
    public class MessageContentContainer
    {
        public string? type { get; set; }
        public byte[]? b { get; set; }
    }
    public class ServerSendMessageContainer
    {
        public string? whom { get; set; }
        public MessageContentContainer? content { get; set; }
        public UserSettingsContainer? user { get; set; }
    }
}
