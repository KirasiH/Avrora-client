using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core.Settings;
using Avrora.Core.JsonClassesContainers;

namespace Avrora.Core.JsonClassesContainers
{
    public class UserSettingsTwoContainer
    {
        public UserSettingsContainer old_user { get; set; } = new UserSettingsContainer();
        public UserSettingsContainer new_user { get; set; } = new UserSettingsContainer();
    }
}
