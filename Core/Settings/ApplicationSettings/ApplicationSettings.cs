using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avrora.Core.JsonClassesContainers;

namespace Avrora.Core.Settings.ApplicationSettings
{

    public class ApplicationListServers
    {
        public List<string>? listServer;
    }
    public class ApplicationSettingsFields : ApplicationListServers
    {
        public string? actualURIServer { get; set; }
    }

    public class ApplicationSettings : ApplicationSettingsFields, ISettings
    {
        private string path_fileApplication = AppDomain.CurrentDomain.BaseDirectory + @"\settings\application\application.json";

        public delegate void DelegateChangeActualUser(ApplicationSettingsContainer container);
        public event DelegateChangeActualUser EventChangeActualServer;
        public ApplicationSettings() 
        {
            try
            {
                using (FileStream stream = new FileStream(path_fileApplication, FileMode.Open)) { }

                Deserialize();

            }
            catch (FileNotFoundException)
            {
                Serializer();
            }
        }

        public void SetActualServer(ApplicationSettingsContainer container)
        {
            string json = JsonSerializer.Serialize(container);

            using (StreamWriter stream = new StreamWriter(path_fileApplication))
            {
                stream.WriteAsync(json);
            }

            EventChangeActualServer(container);
        }

        public ApplicationSettingsContainer GetActualServer()
        {
            return new ApplicationSettingsContainer() { actualURIServer = actualURIServer };
        }

        private void Serializer()
        {
            using (StreamWriter stream = new StreamWriter(path_fileApplication))
            {
                string obj = JsonSerializer.Serialize(this);
                stream.Write(obj);
            }
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
        }
    }
}
