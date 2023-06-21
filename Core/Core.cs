using Avrora.Core.AvroraAPI;
using Avrora.Core.JsonClassesContainers;
using Avrora.Core.Settings.ApplicationSettings;
using Avrora.Core.Settings.UserSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core
{
    public class Core
    {
        public delegate void DelegateChangeActualServer(ServerSettingsContainer container);
        public static event DelegateChangeActualServer? EventChangeActualServer;

        public delegate void DelegateDeleteActualServer(ServerSettingsContainer container);
        public static event DelegateDeleteActualServer? EventDeleteActualServer;

        public delegate void DelegateChangeUser(UserSettingsContainer container);
        public static event DelegateChangeUser? EventChangeActualUser;

        public delegate void UserMethodsDelegate(UserSettingsContainer conteiner, string context);
        public static event UserMethodsDelegate? EventUserMethods;

        public delegate void DelegateErrorURLServer(string uri);
        public static event DelegateErrorURLServer? EventErrorURIServer;

        public delegate void DelegateRecvMessage(ServerRecvMessageContainer container);
        public static event DelegateRecvMessage? EventRecvMessage;

        public delegate void DelegateSendMessage(ServerSendMessageContainer container);
        public static event DelegateSendMessage? EventSendMessage;

        public static Settings.Settings Settings { get; private set; }
        public static AvroraAPI.AvroraAPI AvroraAPI { get; private set; }

        static Core()
        {
            Settings = new Settings.Settings();

            AvroraAPI = new AvroraAPI.AvroraAPI(Settings.GetActualServer(), Settings);
        }

        public static void SetActualServer(string uri)
        {
            Settings.SetActualServer(uri);
            AvroraAPI.ChangeActualURI(Settings.GetConfigServers());

            if (EventChangeActualServer != null)
                EventChangeActualServer(Settings.GetConfigServers());

            if (EventChangeActualUser != null)
                EventChangeActualUser(Settings.GetActualUser());
        }

        public static ServerSettingsContainer GetConfigServers()
        {
            return Settings.GetConfigServers();
        }

        public static void DeleteServer(string uri)
        {
            Settings.DeleteServer(uri);
            AvroraAPI.ChangeActualURI(Settings.GetConfigServers());

            if (EventDeleteActualServer != null)
                EventDeleteActualServer(Settings.GetConfigServers());

            if (EventChangeActualUser != null)
                EventChangeActualUser(new UserSettingsContainer());
        }

        public static void SetActualUser(UserSettingsContainer container)
        {
            Settings.SetActualUser(container);

            if (EventChangeActualUser != null)
                EventChangeActualUser(Settings.GetActualUser());
        }

        public static void DeleteActualUser(UserSettingsContainer container)
        {
            Settings.DelActualUser(container);

            if (EventChangeActualUser != null)
                EventChangeActualUser(new UserSettingsContainer());
        }

        public static async void CreateUserAsync(UserSettingsContainer container)
        {
            HttpResponseMessage mess = await AvroraAPI.CreateUserAsync(container);
            if (mess == null)
            {
                if (EventErrorURIServer != null)
                    EventErrorURIServer(Settings.GetActualServer());
                return;
            }

            SetActualUser(container);

            string context = await mess.Content.ReadAsStringAsync();

            if (EventUserMethods != null)
                EventUserMethods(container, context);
        }

        public static async void DeleteUserAsync(UserSettingsContainer container)
        {
            HttpResponseMessage mess = await AvroraAPI.DeleteUserAsync(container);
            if (mess == null)
            {
                if (EventErrorURIServer != null)
                    EventErrorURIServer(Settings.GetActualServer());
                return;
            }

            DeleteActualUser(container);

            string context = await mess.Content.ReadAsStringAsync();

            if (EventUserMethods != null)
                EventUserMethods(container, context);
        }

        public static async void RecreateUserAsync(UserSettingsTwoContainer container)
        {
            HttpResponseMessage mess = await AvroraAPI.RecreateUserAsync(container);
            if (mess == null)
            {
                if (EventErrorURIServer != null)
                    EventErrorURIServer(Settings.GetActualServer());
                return;
            }

            SetActualUser(container.new_user);

            string context = await mess.Content.ReadAsStringAsync();

            if (EventUserMethods != null)
                EventUserMethods(container.new_user, context);
        }

        public static void SendUser(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }

        public void RecvUser(UserSettingsContainer conteiner)
        {
            throw new NotImplementedException();
        }
    }
}
