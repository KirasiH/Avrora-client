using Avrora.Core.Settings.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core.Settings;
using Avrora.Core.JsonClassesContainers;

namespace Avrora.Core.AvroraAPI
{
    public class ProxyAvroraAPI : IAvroraAPI
    {
        public AvroraAPI avroraAPI;

        public delegate void UserMethodsDelegate(UserSettingsContainer conteiner, string content);
        public event UserMethodsDelegate EventUserMethods;

        private Settings.Settings settings;

        public ProxyAvroraAPI(string uri, Settings.Settings settings)
        {
            this.settings = settings;

            avroraAPI = new AvroraAPI(uri);
        }

        public async Task<HttpResponseMessage> CreateUserAsync(UserSettingsContainer conteiner)
        {
            HttpResponseMessage mess = await avroraAPI.CreateUserAsync(conteiner);

            string content = mess.Content.ReadAsStringAsync().Result;

            EventUserMethods(conteiner, content);

            return mess;
        }

        public async Task<HttpResponseMessage> DeleteUserAsync(UserSettingsContainer conteiner)
        {
            HttpResponseMessage mess = await avroraAPI.DeleteUserAsync(conteiner);

            string content = mess.Content.ReadAsStringAsync().Result;

            EventUserMethods(conteiner, content);

            return mess;
        }

        public async Task<HttpResponseMessage> RecreateUserAsync(UserSettingsTwoContainer twoConteiner)
        {
            twoConteiner.old_user = settings.userSettings.GetActualUser();

            HttpResponseMessage mess = await avroraAPI.RecreateUserAsync(twoConteiner);

            string content = mess.Content.ReadAsStringAsync().Result;

            if (twoConteiner.new_user == null)
                twoConteiner.new_user = new UserSettingsContainer();

            EventUserMethods(twoConteiner.new_user, content);

            return mess;
        }

        public Task RecvUserAsync(UserSettingsContainer conteiner)
        {
            Task task = avroraAPI.RecvUserAsync(conteiner);

            return task;
        }

        public Task SendUserAsync(UserSettingsContainer conteiner)
        {
            Task task = avroraAPI.SendUserAsync(conteiner);

            return task;
        }
    }
}
