using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core;

namespace Avrora.Core.JsonClassesContainers
{
    public class Message : ICloneable
    {
        public int? id { get; set; }
        public string type { get; set; }
        public string data { get; set; }
        public DateTime date { get; set; }
        public string sender { get; set; }
        public bool me { get; set; }
        public object Clone()
        {
            return new Message()
            {
                id = id,
                type = type,
                data = data,
                date = date,
                sender = sender,
                me = me,
            };
        }
    }
}
