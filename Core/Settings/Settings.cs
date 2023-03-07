using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core.Settings.ApplicationSettings;
using Avrora.Core.Settings.UserSettings;

namespace Avrora.Core.Settings
{
    public class Settings
    {
        public UserSettings.UserSettings userSettings;
        public ApplicationSettings.ApplicationSettings applicationSettings;

        public Settings() {

            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/settings");
            DirectoryInfo dirUserSettings = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/settings/user");
            DirectoryInfo dirApplication = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/settings/application");

            if (!dir.Exists)
                dir.Create();

            if (!dirUserSettings.Exists)
                dirUserSettings.Create();

            if (!dirApplication.Exists)
                dirApplication.Create();

            applicationSettings = new ApplicationSettings.ApplicationSettings();

            userSettings = new UserSettings.UserSettings(applicationSettings.GetActualServer());

            applicationSettings.EventChangeActualServer += userSettings.SetActualServer;

            applicationSettings.SetActualServer(new JsonClassesContainers.ApplicationSettingsContainer() { actualURIServer = "http://127.0.0.1:5000" });
        }
    }
}
