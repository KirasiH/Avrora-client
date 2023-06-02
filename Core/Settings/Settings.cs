using Avrora.Core.JsonClassesContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core.Settings.ApplicationSettings;
using Avrora.Core.Settings.UserSettings;
using Avrora.Core.Settings.ChatSettings;
using System.IO;

namespace Avrora.Core.Settings
{
    public class Settings
    {

        private string pathSettings = AppDomain.CurrentDomain.BaseDirectory + @"settings\";
        private string pathUserSettings = AppDomain.CurrentDomain.BaseDirectory + @"settings\user\";
        private string pathServerSettings = AppDomain.CurrentDomain.BaseDirectory + @"settings\application\";
        private string pathChatSettings = AppDomain.CurrentDomain.BaseDirectory + @"settings\chat\";

        private UserSettings.UserSettings userSettings { get; set; }
        private ApplicationSettings.ServerSettings serverSettings { get; set; }

        private ChatSettings.ChatSettings chatSettings { get; set; }

        public Settings() 
        {
            DirectoryInfo settings = new DirectoryInfo(pathSettings);
            DirectoryInfo settings_app = new DirectoryInfo(pathServerSettings);
            DirectoryInfo settings_user = new DirectoryInfo(pathUserSettings);
            DirectoryInfo settings_chat = new DirectoryInfo(pathChatSettings);

            if (!settings.Exists)
                settings.Create();

            if (!settings_app.Exists)
                settings_app.Create();

            if (!settings_user.Exists)
                settings_user.Create();

            if (!settings_chat.Exists)
                settings_chat.Create();

            serverSettings = new ServerSettings(pathServerSettings);
            userSettings = new UserSettings.UserSettings(serverSettings.GetActualServer(), pathUserSettings);
            chatSettings = new ChatSettings.ChatSettings(pathChatSettings);
        }

        public void SetActualServer(string uri)
        {
            serverSettings.SetActualServer(uri);
            userSettings.SetActualServer(serverSettings.GetActualServer());
        }

        public void DeleteServer(string uri)
        {
            serverSettings.DeleteServer(uri);
            userSettings.DeleteServer(serverSettings.GetActualServer());
        }

        public string GetActualServer()
        {
            return serverSettings.GetActualServer();
        }

        public ServerSettingsContainer GetConfigServers()
        {
            return serverSettings.GetCofigServers();
        }

        public void SetActualUser(UserSettingsContainer container)
        {
            userSettings.SetActualUser(container);
        }

        public void SetActualUser(UserSettingsContainer container, string context)
        {
            SetActualUser(container);
        }

        public void DelActualUser(UserSettingsContainer container) 
        {
            userSettings.DeleteActualUser();
        }

        public UserSettingsContainer GetActualUser()
        {
            return userSettings.GetActualUser();
        }
    }
}
