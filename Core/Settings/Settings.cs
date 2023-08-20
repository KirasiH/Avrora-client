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
        public object locker = new object();

        private string pathSettings = AppDomain.CurrentDomain.BaseDirectory + @"settings\";
        private string pathUserSettings = AppDomain.CurrentDomain.BaseDirectory + @"settings\user\";
        private string pathServerSettings = AppDomain.CurrentDomain.BaseDirectory + @"settings\application\";
        private string pathChatSettings = AppDomain.CurrentDomain.BaseDirectory + @"settings\chat\";
        private UserSettings.UserSettings userSettings { get; set; }
        private ServerSettings serverSettings { get; set; }
        private ChatsSettings chatsSettings { get; set; }
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
            chatsSettings = new ChatsSettings(pathChatSettings);
            chatsSettings.SetActualServer(serverSettings.GetActualServer());
            chatsSettings.SetActualUser(userSettings.GetActualUser());
        }
        public void DeleteChat(string nickname)
        {
            lock (locker)
            {
                chatsSettings.DeleteChat(nickname);
            }
        }
        public void DeleteMessage(string nickname, int id)
        {
            lock (locker)
            {
                chatsSettings.DeleteMessage(nickname, id);
            }
        }
        public void SetPathSave(string nickname, string path)
        {
            lock (locker)
            {
                chatsSettings.SetPathSave(nickname, path);
            }
        }
        public void SetTimeClear(string nickname, DateTime date)
        {
            lock (locker)
            {
                chatsSettings.SetTimeClear(nickname, date);
            }
        }
        public void SetQuentity(string nickname, int quentity)
        {
            lock (locker)
            {
                chatsSettings.SetQuentity(nickname, quentity);
            }
        }
        public void AddEncryptingKey(string nickname, string key)
        {
            lock (locker)
            {
                chatsSettings.AddEncryptingKey(nickname, key);
            }
        }
        public string GetEncryptingKey(string nickname)
        {
            lock (locker)
            {
                return chatsSettings.GetEncryptinhKey(nickname);
            }
        }
        public void AddChat(string nickname)
        {
            lock (locker)
            {
                chatsSettings.AddChat(nickname);
            }
        }
        public List<Message> GetMessages(string nickname)
        {
            lock (locker)
            {
                return chatsSettings.GetMessages(nickname);
            }
        }
        public List<ChatContainer> GetChats()
        {
            lock (locker)
            {
                return chatsSettings.GetChats();
            }
        }
        public Message AddMessage(ServerSendMessageContainer message)
        {
            lock (locker)
            {
                return chatsSettings.AddMessage(message);
            }
        }
        public Message AddMessage(ServerRecvMessageContainer message)
        {
            lock (locker)
            {
                return chatsSettings.AddMessage(message);
            }
        }
        public void SetActualServer(string uri)
        {
            lock (locker)
            {
                serverSettings.SetActualServer(uri);
                userSettings.SetActualServer(serverSettings.GetActualServer());
                chatsSettings.SetActualServer(serverSettings.GetActualServer());
                chatsSettings.SetActualUser(userSettings.GetActualUser());
            }
        }
        public void DeleteServer(string uri)
        {
            lock (locker)
            {
                serverSettings.DeleteServer(uri);
                chatsSettings.DeleteServer(uri);
                userSettings.DeleteServer(serverSettings.GetActualServer());
                chatsSettings.SetActualServer(serverSettings.GetActualServer());
                chatsSettings.SetActualUser(userSettings.GetActualUser());
            }
        }
        public string GetActualServer()
        {
            lock (locker)
            {
                return serverSettings.GetActualServer();
            }
        }
        public ServerSettingsContainer GetConfigServers()
        {
            lock (locker)
            {
                return serverSettings.GetCofigServers();
            }
        }
        public void SetActualUser(UserSettingsContainer container)
        {
            lock (locker)
            {
                userSettings.SetActualUser(container);
                chatsSettings.SetActualUser(userSettings.GetActualUser());
            }
        }
        public void SetActualUser(UserSettingsContainer container, string context)
        {
            lock (locker)
            {
                SetActualUser(container);
            }
        }
        public void DelActualUser(UserSettingsContainer container) 
        {
            lock (locker)
            {
                userSettings.DeleteActualUser();
                chatsSettings.SetActualUser(userSettings.GetActualUser());
            }
        }
        public UserSettingsContainer GetActualUser()
        {
            lock (locker)
            {
                return userSettings.GetActualUser();
            }
        }
    }
}
