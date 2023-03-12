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
        public string? name { get; set; }
        public string? nickname { get; set; }
        public string? first_key { get; set; }
        public string? second_key { get; set; }
    }

    public class UserSettings : UserSettingsFields, ISettings
    {

        private string path_fileSettings = AppDomain.CurrentDomain.BaseDirectory + @"\settings\user\user.json";

        private ApplicationSettingsContainer actualServer;
        private Dictionary<string, UserSettingsContainer> userSettings;

        public delegate void DelegateChangeUser(UserSettingsContainer container);
        public event DelegateChangeUser EventChangeActualUser;

        public UserSettings(ApplicationSettingsContainer actServer)
        {
            actualServer = actServer ?? new ApplicationSettingsContainer() { actualURIServer = "" };

            try
            {
                using (FileStream stream = new FileStream(path_fileSettings, FileMode.Open)) { }

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

            UserSettingsContainer? container = null;

            if (!userSettings.TryGetValue(actualServer.actualURIServer ?? "", out container))
                return;

            SetActualUser(container);
        }

        public UserSettingsContainer GetActualUser()
        {
            return new UserSettingsContainer() { 
                name = name,
                nickname = nickname,
                first_key = first_key,
                second_key = second_key};
        }

        public void SetActualUser(UserSettingsContainer container)
        {
            name = container.name;
            nickname = container.nickname;
            first_key = container.first_key;
            second_key = container.second_key;

            userSettings[actualServer.actualURIServer] = container;

            if (EventChangeActualUser == null)
                return;

            EventChangeActualUser(container);

            Save();
        }

        public void SetActualServer(ApplicationSettingsContainer container)
        {
            UserSettingsContainer? userContainer = null;

            actualServer = container;

            if (!userSettings.TryGetValue(actualServer.actualURIServer ?? "", out userContainer))
            {
                return;
            }

            SetActualUser(userContainer);
        }

        public void SetActualUser(UserSettingsContainer container, string content)
        {
            if (content == "recreate" || content == "create" )
                SetActualUser(container);
            if (content == "delete")
                DeleteActualUser();
        }

        public void DeleteActualUser(ApplicationSettingsContainer? container = null)
        {
            try
            {
                if (container == null)
                    userSettings[actualServer.actualURIServer] = new UserSettingsContainer();
                else
                    userSettings[container.actualURIServer] = new UserSettingsContainer();

            }
            catch { }

            EventChangeActualUser(userSettings[actualServer.actualURIServer]);

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
