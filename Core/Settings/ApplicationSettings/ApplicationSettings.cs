using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Avrora.Core.JsonClassesContainers;

namespace Avrora.Core.Settings.ApplicationSettings
{

    public class ApplicationListServers
    {
        public List<string> listServer { get; set; } = new List<string>();
    }
    public class ApplicationSettingsFields : ApplicationListServers
    {
        public string actualURIServer { get; set; } = "";
    }

    public class ApplicationSettings : ApplicationSettingsFields, ISettings
    {
        private string path_fileApplication = AppDomain.CurrentDomain.BaseDirectory + @"\settings\application\application.json";

        public delegate void DelegateChangeActualServer(ApplicationSettingsContainer container);
        public event DelegateChangeActualServer? EventChangeActualServer;

        public delegate void DelegateDeleteActualServer(ApplicationSettingsContainer container);
        public event DelegateDeleteActualServer? EventDeleteActualServer;
        public ApplicationSettings() 
        {
            try
            {
                using (FileStream stream = new FileStream(path_fileApplication, FileMode.Open)) { }
            }
            catch (FileNotFoundException)
            {
                Serializer();
            }

            Deserialize();
        }

        public void SetActualServer(string uri)
        {
            actualURIServer = uri;

            ApplicationSettingsContainer container = new ApplicationSettingsContainer() { actualURIServer = uri, listServer = listServer };

            if (!listServer.Contains(actualURIServer))
                listServer.Add(actualURIServer);

                string json = JsonSerializer.Serialize(container);

                using (StreamWriter stream = new StreamWriter(path_fileApplication))
                {
                    stream.WriteAsync(json);
                }

            if (EventChangeActualServer != null)
                EventChangeActualServer(container);
        }

        public ApplicationSettingsContainer GetActualServer()
        {
            return new ApplicationSettingsContainer() { actualURIServer = actualURIServer, listServer = listServer };
        }

        public void DeleteServer(ApplicationSettingsContainer container)
        {
            listServer.Remove(container.actualURIServer);

            container.listServer = listServer;

            string json = JsonSerializer.Serialize(container);

            using (StreamWriter stream = new StreamWriter(path_fileApplication))
            {
                stream.WriteAsync(json);
            }

            if (EventDeleteActualServer != null)
                EventDeleteActualServer(container);
        }

        private void Serializer()
        {
            using (StreamWriter stream = new StreamWriter(path_fileApplication))
            {
                string obj = JsonSerializer.Serialize(this);
                stream.Write(obj);
            }

            listServer = new List<string>();
        }

        private void Deserialize()
        {
            string obj;

            using(StreamReader stream= new StreamReader(path_fileApplication)) {
                obj = stream.ReadToEnd();
            }

            ApplicationSettingsFields? applicationSettingsFields = JsonSerializer.Deserialize<ApplicationSettingsFields>(obj);

            if (applicationSettingsFields == null)
                return;

            actualURIServer = applicationSettingsFields.actualURIServer;
            listServer = applicationSettingsFields.listServer;
        }
    }
}
