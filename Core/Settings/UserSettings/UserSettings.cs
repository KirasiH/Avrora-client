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

namespace Avrora.Core.Settings.UserSettings
{
    public class UserSettingsAttributes
    {
        public string? name { get; set; }
        public string? nickname { get; set; }
        public string? first_key { get; set; }
        public string? second_key { get; set; }
    }

    public class UserSettings : UserSettingsAttributes, ISettings
    {

        private string path_fileSettings = AppDomain.CurrentDomain.BaseDirectory + @"\settings\user\user.json";

        public UserSettings()
        {
            // FileNotFoundException

            try
            {
                using (FileStream stream = new FileStream(path_fileSettings, FileMode.Open))
                {

                }

                Deserialize();
                
            }
            catch (FileNotFoundException)
            {
                Serializer();
            }

        }

        private void Serializer()
        {

            using (StreamWriter stream = new StreamWriter(path_fileSettings))
            {
                string obj = JsonSerializer.Serialize(this);
                stream.Write(obj);
            }
        }

        private void Deserialize()
        {
            string obj;

            using (StreamReader stream = File.OpenText(path_fileSettings))
            {
                obj = stream.ReadToEnd();
            }

            UserSettingsAttributes? userSettingsContainer = JsonSerializer.Deserialize<UserSettingsAttributes>(obj);

            name = userSettingsContainer.name;
            nickname = userSettingsContainer.nickname;
            first_key = userSettingsContainer.first_key;
            second_key = userSettingsContainer.second_key;
        }

        public UserSettingsContainer GetContainer()
        {
            return new UserSettingsContainer() { name = name,
                nickname = nickname,
                first_key = first_key,
                second_key = second_key};
        }

        public void ChangeUser(UserSettingsContainer container, HttpResponseMessage mess)
        {
            string status = mess.Content.ReadAsStringAsync().Result;
            if (status == "recreate" || status == "create" )
            {
                name = container.name;
                nickname = container.nickname;
                first_key = container.first_key;
                second_key = container.second_key;
            }
        }
    }
}
