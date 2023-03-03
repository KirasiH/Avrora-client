using Avrora.Core.Settings.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core.AvroraAPI
{
    public interface IAvroraAPI
    {
        public Task CreateUserAsync(UserSettingsContainer conteiner);

        public Task DeleteUser(UserSettingsContainer conteiner);

        public Task RecreateUser(UserSettingsContainer conteiner);

        public Task SendUser(UserSettingsContainer conteiner);

        public Task RecvUser(UserSettingsContainer conteiner);
    }
}
