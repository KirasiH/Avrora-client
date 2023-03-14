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
    public class ProxyAvroraAPI
    {
        public AvroraAPI avroraAPI;

        public delegate void UserMethodsDelegate(UserSettingsContainer conteiner, string content);
        public event UserMethodsDelegate EventUserMethods;

        public delegate void DelegateErrorURLServer(string uri);
        public event DelegateErrorURLServer EventErrorURIServer;

        private Settings.Settings settings;

        public ProxyAvroraAPI(string uri, Settings.Settings settings)
        {
            this.settings = settings;

            avroraAPI = new AvroraAPI(uri);
        }

        public void ChangeURI(string uri)
        {
            avroraAPI.Url = uri;
        }

        public async Task CreateUserAsync(UserSettingsContainer conteiner)
        {
            HttpResponseMessage mess;

            try
            {
                mess = await avroraAPI.CreateUserAsync(conteiner);
            }
            catch (InvalidOperationException) 
            {
                EventErrorURIServer(avroraAPI.Url);
                return;
            }
            catch (HttpRequestException)
            {
                EventErrorURIServer(avroraAPI.Url);
                return;
            }
            catch(TaskCanceledException)
            {
                EventErrorURIServer(avroraAPI.Url);
                return;
            }

            string content = await mess.Content.ReadAsStringAsync();

            EventUserMethods(conteiner, content);
        }

        public async Task DeleteUserAsync(UserSettingsContainer conteiner)
        {
            HttpResponseMessage mess;

            try
            {
                mess = await avroraAPI.DeleteUserAsync(conteiner);
            }
            catch (InvalidOperationException)
            {
                EventErrorURIServer(avroraAPI.Url);
                return;
            }
            catch (HttpRequestException)
            {
                EventErrorURIServer(avroraAPI.Url);
                return;
            }
            catch(TaskCanceledException)
            {
                EventErrorURIServer(avroraAPI.Url);
                return;
            }

            string content = await mess.Content.ReadAsStringAsync();

            EventUserMethods(conteiner, content);
        }

        public async Task RecreateUserAsync(UserSettingsTwoContainer twoConteiner)
        {
            twoConteiner.old_user = settings.userSettings.GetActualUser();

            HttpResponseMessage mess;

            try
            {
                mess = await avroraAPI.RecreateUserAsync(twoConteiner);
            }
             catch (InvalidOperationException)
            {
                EventErrorURIServer(avroraAPI.Url);
                return;
            }
            catch (TaskCanceledException)
            {
                EventErrorURIServer(avroraAPI.Url);
                return;
            }

            string content = await mess.Content.ReadAsStringAsync();

            EventUserMethods(twoConteiner.new_user, content);
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
