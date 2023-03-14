using Avrora.Core.AvroraAPI;
using Avrora.Core.JsonClassesContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core
{
    public class Core
    {
        public static Settings.Settings Settings { get; private set; }

        public static ProxyAvroraAPI proxyAvroraAPI { get; private set; }

        static Core()
        {

            Settings = new Settings.Settings();

            string actual = Settings.applicationSettings.actualURIServer;

            proxyAvroraAPI = new ProxyAvroraAPI(actual, Settings);

            proxyAvroraAPI.EventUserMethods += Settings.userSettings.SetActualUser;
        }
    }
}
