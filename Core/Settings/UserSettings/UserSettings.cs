using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Avrora.Core.Settings;

namespace Avrora.Core.Settings.UserSettings
{
    public class UserSettingsContainer
    {
        public string? name { get; set; }
        public string? nickname { get; set; }
        public string? first_key { get; set; }
        public string? second_key { get; set; }
    }

    public class UserSettings : UserSettingsContainer, ISettings
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

            UserSettingsContainer? userSettingsContainer = JsonSerializer.Deserialize<UserSettingsContainer>(obj);

            name = userSettingsContainer.name;
            nickname = userSettingsContainer.nickname;
            first_key = userSettingsContainer.first_key;
            second_key = userSettingsContainer.second_key;
        }
    }
}
