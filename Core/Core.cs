using Avrora.Core.AvroraAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core
{
    public class Core
    {
        public static Settings.Settings Settings { get; private set; }

        public static ProxyAvroraAPI proxyAvroraAPI { get; private set; }

        public static void Start()
        {
            Settings = new Settings.Settings();

            proxyAvroraAPI = new ProxyAvroraAPI("");
        }
    }
}
