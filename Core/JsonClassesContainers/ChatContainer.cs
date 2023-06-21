using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core.JsonClassesContainers
{
    public class ChatContainer
    {
        public string? nickname { get; set; }
        public string? name { get; set; }
        public Message? last_message { get; set; }
        public int? quentity { get; set; }
        public List<Message>? messages { get; set; }
        public string? path_save_files { get; set; }
    }
}
