using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core.Settings.UserSettings;

namespace Avrora.Core.AvroraAPI
{
    public class AvroraAPI : IAvroraAPI
    {
        private HttpClient client = new HttpClient();
        private string url;


        public AvroraAPI(string url)
        {
            this.url = url;
        }

        public async Task CreateUserAsync(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUser(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }

        public async Task RecreateUser(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }

        public async Task RecvUser(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }

        public async Task SendUser(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }

    }
}
