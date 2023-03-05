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

        public delegate void UserMethodsDelegate(UserSettingsContainer conteiner, HttpResponseMessage message);
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

            EventUserMethods(conteiner, mess);

            return mess;
        }

        public async Task<HttpResponseMessage> DeleteUserAsync(UserSettingsContainer conteiner)
        {
            HttpResponseMessage mess = await avroraAPI.DeleteUserAsync(conteiner);

            EventUserMethods(conteiner, mess);

            return mess;
        }

        public async Task<HttpResponseMessage> RecreateUserAsync(UserSettingsTwoContainer twoConteiner)
        {
            twoConteiner.old_user = settings.userSettings.GetContainer();

            HttpResponseMessage mess = await avroraAPI.RecreateUserAsync(twoConteiner);

            EventUserMethods(twoConteiner.new_user, mess);

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
