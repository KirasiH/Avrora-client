using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core.Settings
{
    public class Settings
    {
        public UserSettings.UserSettings userSettings;

        public Settings() {

            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/settings");
            DirectoryInfo dirUserSettings = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/settings/user");

            if (!dir.Exists)
                dir.Create();

            if (!dirUserSettings.Exists)
                dirUserSettings.Create();

            userSettings = new UserSettings.UserSettings();
        }
    }
}
