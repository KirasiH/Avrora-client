using Avrora.Core.JsonClassesContainers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Avrora.Core.Settings.ChatSettings
{
    public interface IServerChatsSettings
    {
        public void DeleteChat(string nickname);
        public void DeleteMessage(string nickname, int id);
        public void SetPathSave(string nickname, string path);
        public void SetTimeClear(string nickname, DateTime date);
        public void SetQuentity(string nickname, int quentity);
        public void AddEncryptingKey(string nickname, string key);
        public void AddChat(string nickname);
        public List<Message> GetMessages(string nickname);
        public List<ChatContainer> GetChats();
        public Message? AddMessage(ServerSendMessageContainer message);
        public Message? AddMessage(ServerRecvMessageContainer message);
        public void SetUser(string nickname);
    }
    public class PlugServerChatsSettings : IServerChatsSettings
    {
        public void AddChat(string nickname) { }
        public Message? AddMessage(ServerSendMessageContainer message)
        {
            return null;
        }
        public Message? AddMessage(ServerRecvMessageContainer message)
        {
            return null;
        }
        public void DeleteChat(string nickname) { }
        public void DeleteMessage(string nickname, int id) { }
        public List<ChatContainer> GetChats()
        {
            return new List<ChatContainer>();
        }
        public List<Message> GetMessages(string nickname)
        {
            return new List<Message>();
        }
        public void SetPathSave(string nickname, string path) { }
        public void SetQuentity(string nickname, int quentity) { }
        public void SetTimeClear(string nickname, DateTime date) { }
        public void AddEncryptingKey(string nickname, string key) { }
        public void SetUser(string nickname) { }
    }
    public class ServerChatsSettings : IServerChatsSettings
    {
        private string path;
        private string path_fileChatsJson;
        private Dictionary<string, string> FoldersOfUser = new Dictionary<string, string>();
        private Dictionary<string, UserChatsSettings> UserAndUserChatSettings = new Dictionary<string, UserChatsSettings>();
        private UserChatsSettings actualUserChatsSettings;
        public ServerChatsSettings(string path)
        {
            this.path = path;
            path_fileChatsJson = $"{path}.json";

            FileInfo fileInfo = new FileInfo(path_fileChatsJson);

            if (!fileInfo.Exists)
            {
                Serialize();
            }

            Deserialize();
        }
        public void DeleteChat(string nickname)
        {
            actualUserChatsSettings.DeleteChat(nickname);
        }
        public void DeleteMessage(string nickname, int id)
        {
            actualUserChatsSettings.DeleteMessage(nickname, id);
        }
        public void SetPathSave(string nickname, string path)
        {
            actualUserChatsSettings.SetPathSave(nickname, path);
        }
        public void SetTimeClear(string nickname, DateTime date)
        {
            actualUserChatsSettings.SetTimeClear(nickname, date);
        }
        public void SetQuentity(string nickname, int quentity)
        {
            actualUserChatsSettings.SetQuentity(nickname, quentity);
        }
        public void SetUser(string nickname)
        {
            if (!FoldersOfUser.TryGetValue(nickname, out string? _))
            {
                FoldersOfUser.Add(nickname, ConvertPathFolderFromNickname(nickname));
                Serialize();
            }

            if (!UserAndUserChatSettings.TryGetValue(nickname, out UserChatsSettings? _))
            {
                string path_nickname = $"{path}{FoldersOfUser[nickname]}\\";

                UserChatsSettings userChatsSettings =  new UserChatsSettings(path_nickname);

                UserAndUserChatSettings.Add(nickname, userChatsSettings);
            }

            actualUserChatsSettings = UserAndUserChatSettings[nickname];
        }
        public List<Message> GetMessages(string nickname)
        {
            return actualUserChatsSettings.GetMessages(nickname);
        }
        public List<ChatContainer> GetChats()
        {
            return actualUserChatsSettings.GetChats();
        }
        public Message AddMessage(ServerSendMessageContainer message)
        {
            return actualUserChatsSettings.AddMessage(message);
        }
        public Message AddMessage(ServerRecvMessageContainer message)
        {
            return actualUserChatsSettings.AddMessage(message);
        }
        public void AddEncryptingKey(string nickname, string key)
        {
            actualUserChatsSettings.AddEncryptingKey(nickname, key);
        }
        public string GetEnctypringKey(string nickname)
        {
            return actualUserChatsSettings.GetEncryptingKey(nickname);
        }
        public void AddChat(string nickname)
        {
            actualUserChatsSettings.AddChat(nickname);
        }
        private string ConvertPathFolderFromNickname(string nickname)
        {
            return $"User_{nickname}";
        }
        private void Serialize()
        {
            using (StreamWriter writer = new StreamWriter(path_fileChatsJson))
            {
                string json = JsonSerializer.Serialize(FoldersOfUser);
                writer.Write(json);
            }

            foreach (KeyValuePair<string, string> kvp in FoldersOfUser)
            {
                DirectoryInfo dirInfo = new DirectoryInfo($"{path}{kvp.Value}");
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
            }
        }
        private void Deserialize()
        {
            string json;

            using (StreamReader reader = new StreamReader(path_fileChatsJson))
                json = reader.ReadToEnd();

            Dictionary<string, string>? _dictFolderOfChats = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            if (_dictFolderOfChats == null)
                return;

            FoldersOfUser = _dictFolderOfChats;

            foreach (KeyValuePair<string, string> pair in FoldersOfUser)
            {
                string path_userchatSettings = $"{path}{pair.Value}\\";

                UserChatsSettings userChatSettings = new UserChatsSettings(path_userchatSettings);

                UserAndUserChatSettings.Add(pair.Key, userChatSettings);
            }
        }
    }
}
