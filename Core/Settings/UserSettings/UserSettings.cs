using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Avrora.Core.Settings;
using Avrora.Core.JsonClassesContainers;
using System.Net.Http;
using System.ComponentModel;

namespace Avrora.Core.Settings.UserSettings
{
    public class UserSettingsFields
    {
        public string name { get; set; } = "";
        public string nickname { get; set; } = "";
        public string first_key { get; set; } = "";
        public string second_key { get; set; } = "";
    }

    public class UserSettings : UserSettingsFields
    {

        private string path_fileSettings;

        private string actualServer;
        private Dictionary<string, UserSettingsContainer> userSettings = new Dictionary<string, UserSettingsContainer>();

        public UserSettings(string actServer, string path_fileSettings)
        {
            actualServer = actServer;
            this.path_fileSettings = path_fileSettings + "user.json";

            try
            {
                using (FileStream stream = new FileStream(this.path_fileSettings, FileMode.Open)) { }
            }
            catch (FileNotFoundException)
            {
                Serializer();
            }

            Deserialize();
        }

        private void Serializer()
        {

            using (StreamWriter stream = new StreamWriter(path_fileSettings))
            {
                string obj = JsonSerializer.Serialize(new Dictionary<string, UserSettingsFields>());
                stream.Write(obj);
            }
        }

        private void Deserialize()
        {
            string obj;

            using (StreamReader stream = File.OpenText(path_fileSettings)) { obj = stream.ReadToEnd(); }

            userSettings = JsonSerializer.Deserialize<Dictionary<string, UserSettingsContainer>>(obj) ?? new Dictionary<string, UserSettingsContainer>();

            UserSettingsContainer? container;

            if (!userSettings.TryGetValue(actualServer, out container))
            {
                SetActualUser(new UserSettingsContainer());
                return;
            }
                
            SetActualUser(container);
        }

        public UserSettingsContainer GetActualUser()
        {
            return new UserSettingsContainer() { 
                name = name,
                nickname = nickname,
                first_key = first_key,
                second_key = second_key
            };
        }

        public void SetActualUser(UserSettingsContainer container)
        {
            name = container.name;
            nickname = container.nickname;
            first_key = container.first_key;
            second_key = container.second_key;

            userSettings[actualServer] = container;

            Save();
        }

        public void SetActualServer(string uri)
        {
            UserSettingsContainer? userContainer;

            actualServer = uri;

            if (!userSettings.TryGetValue(actualServer, out userContainer))
            {
                userSettings.Add(actualServer, new UserSettingsContainer());
                userContainer = new UserSettingsContainer();
            }

            SetActualUser(userContainer);
        }

        public void DeleteActualUser()
        {
            userSettings[actualServer] = new UserSettingsContainer();

            Save();
        }

        public void DeleteServer(string uri)
        {
            if (userSettings.TryGetValue(uri, out UserSettingsContainer? userContainer))
                userSettings.Remove(uri);

            Save();
        }

        private void Save()
        {
            string json = JsonSerializer.Serialize(userSettings);

            using (StreamWriter stream = new StreamWriter(path_fileSettings))
            {
                stream.WriteAsync(json);
            }
        }
    }
}
