using Avrora.Core.JsonClassesContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core.Settings.ApplicationSettings;
using Avrora.Core.Settings.UserSettings;

namespace Avrora.Core.Settings
{
    public class Settings
    {
        public delegate void DelegateChangeActualServer(ServerSettingsContainer container);
        public event DelegateChangeActualServer? EventChangeActualServer;

        public delegate void DelegateDeleteActualServer(ServerSettingsContainer container);
        public event DelegateDeleteActualServer? EventDeleteActualServer;

        public delegate void DelegateChangeUser(UserSettingsContainer container);
        public event DelegateChangeUser? EventChangeActualUser;

        private string pathUserSettings = AppDomain.CurrentDomain.BaseDirectory + @"\settings\user\user.json";
        private string pathServerSettings = AppDomain.CurrentDomain.BaseDirectory + @"\settings\application\application.json";

        private UserSettings.UserSettings userSettings { get; set; }
        private ApplicationSettings.ServerSettings serverSettings { get; set; }

        public Settings() 
        {
            serverSettings = new ServerSettings(pathServerSettings);
            userSettings = new UserSettings.UserSettings(serverSettings.GetActualServer(), pathUserSettings);
        }

        public void SetActualServer(string uri)
        {
            serverSettings.SetActualServer(uri);
            userSettings.SetActualServer(serverSettings.GetActualServer());

            if (EventChangeActualServer!= null)
                EventChangeActualServer(serverSettings.GetCofigServers());
            
            if (EventChangeActualUser != null) 
                EventChangeActualUser(userSettings.GetActualUser());
        }

        public void DeleteServer(string uri)
        {
            serverSettings.DeleteServer(uri);
            userSettings.DeleteServer(serverSettings.GetActualServer());

            if (EventDeleteActualServer != null)
                EventDeleteActualServer(serverSettings.GetCofigServers());

            if (EventChangeActualUser != null)
                EventChangeActualUser(new UserSettingsContainer());
        }

        public string GetActualServer()
        {
            return serverSettings.GetActualServer();
        }

        public ServerSettingsContainer GetConfigServer()
        {
            return serverSettings.GetCofigServers();
        }

        public void SetActualUser(UserSettingsContainer container)
        {
            userSettings.SetActualUser(container);

            if (EventChangeActualUser != null)
                EventChangeActualUser(userSettings.GetActualUser());
        }

        public void SetActualUser(UserSettingsContainer container, string context)
        {
            SetActualUser(container);
        }

        public void DelActualUser(UserSettingsContainer container) 
        {
            userSettings.DeleteActualUser();

            if (EventChangeActualUser != null)
                EventChangeActualUser(new UserSettingsContainer());
        }

        public UserSettingsContainer GetActualUser()
        {
            return userSettings.GetActualUser();
        }
    }
}
