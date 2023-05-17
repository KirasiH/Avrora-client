using Avrora.Core.AvroraAPI;
using Avrora.Core.JsonClassesContainers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core
{
    public class Core
    {
        public static Settings.Settings Settings { get; private set; }
        public static AvroraAPI.AvroraAPI AvroraAPI { get; private set; }

        static Core()
        {

            Settings = new Settings.Settings();

            AvroraAPI = new AvroraAPI.AvroraAPI(Settings.GetActualServer(), Settings);

            AvroraAPI.EventUserMethods += Settings.SetActualUser;
            Settings.EventChangeActualServer += AvroraAPI.EventChangeActualURI;
        }

        public static async void CreateUserAsync(UserSettingsContainer container)
        {
            await AvroraAPI.CreateUserAsync(container);
        }

        public static async void DeleteUserAsync(UserSettingsContainer container)
        {
            await AvroraAPI.DeleteUserAsync(container);
        }

        public static async void RecreateUserAsync(UserSettingsTwoContainer container)
        {
            await AvroraAPI.RecreateUserAsync(container);
        }

        public static void SendUser(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }

        public void RecvUser(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }
    }
}
