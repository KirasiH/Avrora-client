using Avrora.Core.Settings.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core.JsonClassesContainers;

namespace Avrora.Core.AvroraAPI
{
    public interface IAvroraAPI
    {
        public Task<HttpResponseMessage> CreateUserAsync(UserSettingsContainer conteiner);

        public Task<HttpResponseMessage> DeleteUserAsync(UserSettingsContainer conteiner);

        public Task<HttpResponseMessage> RecreateUserAsync(UserSettingsTwoContainer conteiner);

        public Task SendUserAsync(UserSettingsContainer conteiner);

        public Task RecvUserAsync(UserSettingsContainer conteiner);
    }
}
