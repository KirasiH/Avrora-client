using Avrora.Core.Settings.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core.AvroraAPI
{
    public class ProxyAvroraAPI : IAvroraAPI
    {
        public AvroraAPI avroraAPI;

        public ProxyAvroraAPI(string uri)
        {
            this.avroraAPI = new AvroraAPI(uri);
        }

        public Task CreateUserAsync(UserSettingsContainer conteiner)
        {
            Task task = avroraAPI.CreateUserAsync(conteiner);

            return task;
        }

        public Task DeleteUser(UserSettingsContainer conteiner)
        {
            Task task = avroraAPI.DeleteUser(conteiner);

            return task;
        }

        public Task RecreateUser(UserSettingsContainer conteiner)
        {
            Task task = avroraAPI.RecreateUser(conteiner);

            return task;
        }

        public Task RecvUser(UserSettingsContainer conteiner)
        {
            Task task = avroraAPI.RecvUser(conteiner);

            return task;
        }

        public Task SendUser(UserSettingsContainer conteiner)
        {
            Task task = avroraAPI.SendUser(conteiner);

            return task;
        }
    }
}
