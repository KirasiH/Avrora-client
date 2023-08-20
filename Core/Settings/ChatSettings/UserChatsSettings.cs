using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avrora.Core.JsonClassesContainers;

namespace Avrora.Core.Settings.ChatSettings
{
    public class UserChatsSettings
    {
        private string path;
        private string path_fileChatsJson;
        private Dictionary<string, string> FolderOfChats = new Dictionary<string, string>();
        private Dictionary<string, ChatSettings> NicknameAndChatSettings = new Dictionary<string, ChatSettings>();
        public UserChatsSettings(string path)
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
            if (FolderOfChats.TryGetValue(nickname, out string? folder))
            {
                NicknameAndChatSettings.Remove(nickname);
                FolderOfChats.Remove(nickname);

                DirectoryInfo dir = new DirectoryInfo($"{path}{folder}");
                dir.Delete(true);

                Serialize();
            }
        }
        public void DeleteMessage(string nickname, int id)
        {
            ChatSettings chatSettings = NicknameAndChatSettings[nickname];

            chatSettings.Clear(id);
        }
        public void SetPathSave(string nickname, string path)
        {
            ChatSettings chatSettings = NicknameAndChatSettings[nickname];

            chatSettings.SetPathSave(path);
        }
        public void SetTimeClear(string nickname, DateTime date)
        {
            ChatSettings chatSettings = NicknameAndChatSettings[nickname];

            chatSettings.SetTimeClear(date);
        }
        public void SetQuentity(string nickname, int quentity)
        {
            ChatSettings chatSettings = NicknameAndChatSettings[nickname];

            chatSettings.SetQuentity(quentity);
        }
        public void AddEncryptingKey(string nickname, string key)
        {
            ChatSettings chatSettings = NicknameAndChatSettings[nickname];

            chatSettings.AddEncryptingKey(key);
        }
        public string GetEncryptingKey(string nickname)
        {
            AddChat(nickname);

            ChatSettings chatSettings = NicknameAndChatSettings[nickname];

            return chatSettings.GetEncryptingKey();
        }
        public void AddChat(string nickname)
        {
            if (!FolderOfChats.TryGetValue(nickname, out string? _))
            {
                FolderOfChats.Add(nickname, ConvertPathFolderFromNickname(nickname));
                Serialize();
            }

            if (!NicknameAndChatSettings.TryGetValue(nickname, out ChatSettings _))
            {
                string path_dir = $"{path}{ConvertPathFolderFromNickname(nickname)}\\";

                ChatSettings chatSettings = new ChatSettings(path_dir);

                NicknameAndChatSettings.Add(nickname, chatSettings);
            }
        }
        public List<Message> GetMessages(string nickname)
        {
            AddChat(nickname);

            ChatSettings chatSettings = NicknameAndChatSettings[nickname];

            return chatSettings.GetMessage();
        }
        public List<ChatContainer> GetChats()
        {
            List<ChatContainer> list_chatContainer = new List<ChatContainer>();

            foreach (KeyValuePair<string, ChatSettings> pair in NicknameAndChatSettings)
            {
                ChatContainer chatContainer = pair.Value.GetConfig();

                chatContainer.nickname = pair.Key;

                list_chatContainer.Add(chatContainer);
            }

            return list_chatContainer;
        }
        public Message AddMessage(ServerSendMessageContainer message)
        {
            string nickname = message.whom;

            AddChat(nickname);

            ChatSettings chatSettings = NicknameAndChatSettings[nickname];

            return chatSettings.AddMessage(message);
        }
        public Message AddMessage(ServerRecvMessageContainer message)
        {
            string nickname = message.sender_nickname;

            AddChat(nickname);

            ChatSettings chatSettings = NicknameAndChatSettings[nickname];

            return chatSettings.AddMessage(message);
        }
        private string ConvertPathFolderFromNickname(string nickname)
        {
            return $"Chat_{nickname}";
        }
        private void Serialize()
        {
            using (StreamWriter writer = new StreamWriter(path_fileChatsJson))
            {
                string json = JsonSerializer.Serialize(FolderOfChats);
                writer.Write(json);
            }

            foreach (KeyValuePair<string, string> kvp in FolderOfChats)
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

            FolderOfChats = _dictFolderOfChats;

            foreach (KeyValuePair<string, string> pair in FolderOfChats)
            {
                string path_chatSettings = $"{path}{pair.Value}\\";

                ChatSettings chatSettings = new ChatSettings(path_chatSettings);

                NicknameAndChatSettings.Add(pair.Key, chatSettings);
            }
        }
    }
}
