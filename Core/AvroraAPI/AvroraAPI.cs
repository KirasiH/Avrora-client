using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core.Settings.UserSettings;
using System.Security.Policy;
using System.Windows.Markup;
using Avrora.Core.JsonClassesContainers;

namespace Avrora.Core.AvroraAPI
{
    public class AvroraAPI : IAvroraAPI
    {
        private HttpClient client;
        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public AvroraAPI(string url)
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(2);

            this.url = url;
        }
        private async Task<HttpResponseMessage> UserMethods(string uri, UserSettingsTwoContainer twoContainer, HttpMethod method)
        {
            HttpRequestMessage req = new HttpRequestMessage(method, uri);

            req.Content = JsonContent.Create(twoContainer);

            HttpResponseMessage mess = await client.SendAsync(req);

            return mess;
        }

        private async Task<HttpResponseMessage> UserMethods(string uri, UserSettingsContainer conteiner, HttpMethod method)
        {
            HttpRequestMessage req = new HttpRequestMessage(method, uri);

            req.Content = JsonContent.Create(conteiner);

            HttpResponseMessage mess = await client.SendAsync(req);

            return mess;
        }

        public async Task<HttpResponseMessage> CreateUserAsync(UserSettingsContainer conteiner)
        {
            string uriResource = $"{url}/create";

            return await UserMethods(uriResource, conteiner, HttpMethod.Post);
        }

        public async Task<HttpResponseMessage> DeleteUserAsync(UserSettingsContainer conteiner)
        {
            string uriResource = $"{url}/delete";

            return await UserMethods(uriResource, conteiner, HttpMethod.Delete);
        }

        public async Task<HttpResponseMessage> RecreateUserAsync(UserSettingsTwoContainer twoContainer)
        {
            string uriResource = $"{url}/recreate";

            return await UserMethods(uriResource, twoContainer, HttpMethod.Put);
        }

        public async Task RecvUserAsync(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }

        public async Task SendUserAsync(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }

    }
}
