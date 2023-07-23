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
    static class Converter
    {
        public static string GetStringFromBytes(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static byte[] GetBytesFromString(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
    }
    class ChatsSettings
    {
        private string path_fileServersJson;
        private string? actualServer;
        private string path;
        private Dictionary<string, string> FoldersOfServer = new Dictionary<string, string>();
        private Dictionary<string, ServerChatsSettings> URIAndServerChatsSettings = new Dictionary<string, ServerChatsSettings>();
        private IServerChatsSettings actualServerChatsSettings = new PlugServerChatsSettings();
        public ChatsSettings(string path)
        {
            path_fileServersJson = $"{path}.json";
            this.path = path;

            FileInfo fileInfo = new FileInfo(path_fileServersJson);

            if (!fileInfo.Exists)
                Serialize();
            else
                Deserialize();
        }
        public void DeleteChat(string nickname)
        {
            actualServerChatsSettings.DeleteChat(nickname);
        }
        public void DeleteMessage(string nickname, int id)
        {
            actualServerChatsSettings.DeleteMessage(nickname, id);
        }
        public void SetPathSave(string nickname, string path)
        {
            actualServerChatsSettings.SetPathSave(nickname, path);
        }
        public void SetTimeClear(string nickname, DateTime date)
        {
            actualServerChatsSettings.SetTimeClear(nickname, date);
        }
        public void SetQuentity(string nickname, int quentity)
        {
            actualServerChatsSettings.SetQuentity(nickname, quentity);
        }
        public void AddEncryptingKey(string nickname, string key)
        {
            actualServerChatsSettings.AddEncryptingKey(nickname, key);
        }
        public void AddChat(string nickname)
        {
            actualServerChatsSettings.AddChat(nickname);
        }
        public List<Message> GetMessages(string nickname)
        {
            return actualServerChatsSettings.GetMessages(nickname);
        }
        public List<ChatContainer>? GetChats()
        {
            return actualServerChatsSettings.GetChats();
        }
        public Message? AddMessage(ServerSendMessageContainer message)
        {
            return actualServerChatsSettings.AddMessage(message);
        }
        public Message? AddMessage(ServerRecvMessageContainer message)
        {
            return actualServerChatsSettings.AddMessage(message);
        }
        public void SetActualServer(string uri)
        {
            if (!FoldersOfServer.TryGetValue(uri, out string? val))
            {
                FoldersOfServer.Add(uri, ConvertURI(uri));
                Serialize();
            }

            if (!URIAndServerChatsSettings.TryGetValue(uri, out ServerChatsSettings? serverChatsSettings))
            {
                string path_server = $"{path}{FoldersOfServer[uri]}\\";

                ServerChatsSettings serverChats = new ServerChatsSettings(path_server);

                URIAndServerChatsSettings.Add(uri, serverChats);
            }

            actualServerChatsSettings = URIAndServerChatsSettings[uri];
        }
        public void DeleteServer(string uri)
        {
            if (FoldersOfServer.TryGetValue(uri, out string? folder))
            {
                URIAndServerChatsSettings.Remove(uri);
                FoldersOfServer.Remove(uri);

                actualServerChatsSettings = new PlugServerChatsSettings();

                string full_path = $"{path}{folder}";

                DirectoryInfo dir = new DirectoryInfo(full_path);
                dir.Delete(true);
            }

            Serialize();
        }
        private void Serialize()
        {
            using (StreamWriter writer = new StreamWriter(path_fileServersJson))
            {
                string json = JsonSerializer.Serialize(FoldersOfServer);
                writer.WriteAsync(json);
            }

            foreach (KeyValuePair<string, string> kvp in FoldersOfServer)
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
            using (StreamReader reader = new StreamReader(path_fileServersJson))
            {
                json = reader.ReadToEnd();
            }

            Dictionary<string, string>? memento = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            if (memento == null)
                return;

            FoldersOfServer = memento;
        }
        private string ConvertURI(string uri)
        {
            char[] forbiddenChars = new char[] { '/', '\\', '*', ':', '?', '|', '>', '<', ' ' };

            foreach (char c in uri)
            {
                if (forbiddenChars.Contains(c))
                {
                    uri = uri.Replace(c, '_');
                }
            }

            return $"{uri}";
        }
    }
}
