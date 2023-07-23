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
    public class ApplicationSettingsFields
    {
        public string actualURIServer { get; set; } = " ";

        public List<string> listServer { get; set; } = new List<string>();
    }
    public class ServerSettings : ApplicationSettingsFields
    {
        private string path_fileApplication;

        public ServerSettings(string path) 
        {
            this.path_fileApplication = path + "application.json";

            FileInfo fileInfo = new FileInfo(path_fileApplication);

            if (!fileInfo.Exists)
                Serializer();
            else
                Deserialize();
        }

        public void SetActualServer(string uri)
        {
            actualURIServer = uri ?? " ";

            if (!listServer.Contains(actualURIServer))
                listServer.Add(actualURIServer);

            Save();
        }

        public string GetActualServer()
        {
            return actualURIServer;
        }

        public ServerSettingsContainer GetCofigServers()
        {
            return new ServerSettingsContainer() { actualURIServer = actualURIServer, listServer = listServer };
        }

        public void DeleteServer(string uri)
        {
            listServer.Remove(uri);

            if (actualURIServer == uri)
                actualURIServer = " ";

            Save();
        }

        private void Serializer()
        {
            using (StreamWriter stream = new StreamWriter(path_fileApplication))
            {
                string obj = JsonSerializer.Serialize(new ApplicationSettingsFields());
                stream.Write(obj);
            }
        }

        private void Deserialize()
        {
            string obj;

            using(StreamReader stream= new StreamReader(path_fileApplication)) {
                obj = stream.ReadToEnd();
            }

            ApplicationSettingsFields applicationSettingsFields = JsonSerializer.Deserialize<ApplicationSettingsFields>(obj) ?? new ApplicationSettingsFields();

            actualURIServer = applicationSettingsFields.actualURIServer;
            listServer = applicationSettingsFields.listServer;
        }

        private void Save()
        {

            string json = JsonSerializer.Serialize(new ApplicationSettingsFields()
            {
                actualURIServer = actualURIServer,
                listServer = listServer
            });

            using(StreamWriter stream = new StreamWriter(path_fileApplication))
            {
                stream.WriteAsync(json);
            }
        }
    }
}
