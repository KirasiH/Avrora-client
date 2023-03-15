using Avrora.Core.Settings.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core.Settings;
using Avrora.Core.JsonClassesContainers;
using System.Collections.ObjectModel;
using Avrora.Core.AvroraAPI;

namespace Avrora.Core.AvroraAPI
{
    public class AvroraAPI
    {
        public AvroraAPIMethods avroraAPIMethods;

        public delegate void UserMethodsDelegate(UserSettingsContainer conteiner, string content);
        public event UserMethodsDelegate? EventUserMethods;

        public delegate void DelegateErrorURLServer(string uri);
        public event DelegateErrorURLServer? EventErrorURIServer;

        private Settings.Settings settings;

        public string Uri
        {
            get { return avroraAPIMethods.Url; }
            set { avroraAPIMethods.Url = value;}
        }

        public AvroraAPI(string uri, Settings.Settings settings)
        {
            this.settings = settings;

            avroraAPIMethods = new AvroraAPIMethods(uri);
        }

        public void EventChangeActualURI(ApplicationSettingsContainer container)
        {
            Uri = container.actualURIServer;
        }

        public async Task CreateUserAsync(UserSettingsContainer conteiner)
        {
            HttpResponseMessage mess;
            try 
            { 
                mess = await avroraAPIMethods.CreateUserAsync(conteiner);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException){
                if (EventErrorURIServer != null)
                    EventErrorURIServer(avroraAPIMethods.Url);
                return;
            }

            string content = await mess.Content.ReadAsStringAsync();

            if (EventUserMethods != null) 
                EventUserMethods(conteiner, content);
        }

        public async Task DeleteUserAsync(UserSettingsContainer conteiner)
        {
            HttpResponseMessage mess;

            try
            {
                mess = await avroraAPIMethods.DeleteUserAsync(conteiner);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException)
            {
                if (EventErrorURIServer != null)
                    EventErrorURIServer(avroraAPIMethods.Url);
                return;
            }

            string content = await mess.Content.ReadAsStringAsync();

            if (EventUserMethods != null) 
                EventUserMethods(conteiner, content);
        }

        public async Task RecreateUserAsync(UserSettingsTwoContainer twoConteiner)
        {
            twoConteiner.old_user = settings.userSettings.GetActualUser();

            HttpResponseMessage mess;

            try
            {
                mess = await avroraAPIMethods.RecreateUserAsync(twoConteiner);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException)
            {
                if (EventErrorURIServer != null)
                    EventErrorURIServer(avroraAPIMethods.Url);
                return;
            }

            string content = await mess.Content.ReadAsStringAsync();

            if (EventUserMethods != null)
                EventUserMethods(twoConteiner.new_user, content);
        }

        public Task RecvUserAsync(UserSettingsContainer conteiner)
        {
            Task task = avroraAPIMethods.RecvUserAsync(conteiner);

            return task;
        }

        public Task SendUserAsync(UserSettingsContainer conteiner)
        {
            Task task = avroraAPIMethods.SendUserAsync(conteiner);

            return task;
        }
    }
}
