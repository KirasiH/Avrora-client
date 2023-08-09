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

        public void ChangeActualURI(ServerSettingsContainer container)
        {
            Uri = container.actualURIServer;
        }

        public async Task<HttpResponseMessage> CreateUserAsync(UserSettingsContainer conteiner)
        {
            HttpResponseMessage mess;
            try 
            { 
                mess = await avroraAPIMethods.CreateUserAsync(conteiner);

                return mess;
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException){
                return null;
            }
        }

        public async Task<HttpResponseMessage> DeleteUserAsync(UserSettingsContainer conteiner)
        {
            HttpResponseMessage mess;
            try
            {
                mess = await avroraAPIMethods.DeleteUserAsync(conteiner);

                return mess;
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> RecreateUserAsync(UserSettingsTwoContainer twoConteiner)
        {
            twoConteiner.old_user = settings.GetActualUser();

            HttpResponseMessage mess;

            try
            {
                mess = await avroraAPIMethods.RecreateUserAsync(twoConteiner);

                return mess;
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> RecvUserAsync(UserSettingsContainer container)
        {
            HttpResponseMessage mess;

            try
            {
                mess = await avroraAPIMethods.RecvUserAsync(container);

                return mess;
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> SendUserAsync(ServerSendMessageContainer messageContainer)
        {
            HttpResponseMessage mess;

            try
            {
                mess = await avroraAPIMethods.SendUserAsync(messageContainer);

                return mess;
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is HttpRequestException || ex is TaskCanceledException)
            {
                return null;
            }
        }
    }
}
