using Avrora.Core.JsonClassesContainers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Avrora.Core.Settings.ChatSettings
{
    public class ChatConfigMemento
    {
        public Message? last_messages { get; set; }
        public int? quentity { get; set; }
        public string? path_save_files { get; set; }
        public string? clear_date { get; set; }
        public int? id { get; set; }
        public string? name { get; set; }
        public string? encrypting_key { get; set; } 
    }
    public class ChatSettingsMemento
    {
        public List<Message>? messages { get; set; }
    }
    public class ChatSettings
    {
        private string format_time = "yyyyMMddTHH:mm:ssZ";
        private CultureInfo culture_info = CultureInfo.InvariantCulture;
        private DateTime clear_date = DateTime.UtcNow.AddMonths(-6);

        private List<Message> messages = new List<Message>();

        private string name;

        private string encrypting_key = "YourFirstKey";

        private string path_fileChatSettings;
        private string path_fileConfigSettings;
        private string path_save_files;

        private Message? last_message;

        private int quentity = 0;
        private int id = 0;

        private bool clear_status = false;
        private bool deserialize_chat_status = false;
        private bool deserialize_chat_config = false;

        public ChatSettings(string path)
        {
            path_fileChatSettings = $"{path}chat.json";
            path_fileConfigSettings = $"{path}config.json";
            path_save_files = $"{path}files\\";

            FileInfo fileChatSettings = new FileInfo(path_fileChatSettings);
            FileInfo fileConfigSettings = new FileInfo(path_fileConfigSettings);

            if (!fileChatSettings.Exists)
            {
                fileChatSettings.Create().Dispose();
                SerializeChat();
            }

            if (!fileConfigSettings.Exists)
            {
                fileConfigSettings.Create().Dispose();
                SerializeConfig();
            }
            else
            {
                DeserializeConfig();
            }
        }
        public void SetPathSave(string path)
        {
            path_save_files = path;
            SerializeConfig();
        }
        public void SetTimeClear(DateTime date)
        {
            clear_date = date;
            SerializeConfig();
        }
        public void SetQuentity(int qurntity)
        {
            quentity = qurntity;
            SerializeConfig();
        }
        public Message AddMessage(ServerSendMessageContainer message)
        {
            DeserializeChat();

            string[] date = message.content.type.Split("|");
            string type = date[0];
            string file_name = date[1];

            string path = $"{path_save_files}{id}";

            Message saving_message = new Message()
            {
                id = id,
                type = type,
                date = DateTime.UtcNow,
                me = true
            };

            if (type == IsSendMessage.Text.ToString())
            {
                saving_message.data = Converter.GetStringFromBytes(message.content.b);
            }
            else if (IsSave(type))
            {
                string path_file = $"{path}\\{file_name}";

                saving_message.data = path_file;

                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                if (!directoryInfo.Exists)
                    directoryInfo.Create();

                using (FileStream stream = new FileStream(path_file, FileMode.OpenOrCreate))
                    stream.Write(message.content.b, 0, message.content.b.Length);
            }
            else
            {
                saving_message.data = null;
            }

            messages.Add(saving_message);
            last_message = saving_message;

            id++;

            SerializeChat();
            SerializeConfig();

            return saving_message;
        }
        public Message AddMessage(ServerRecvMessageContainer message)
        {
            DeserializeChat();

            string[] date = message.type.Split("|");
            string type = date[0];
            string file_name = date[1];

            string path = $"{path_save_files}{id}\\";

            Message saving_message = new Message()
            {
                id = id,
                type = type,
                date = DateTime.UtcNow,
                me = false,
                sender = message.sender_nickname
            };

            if (type == "text")
            {
                saving_message.data = Converter.GetStringFromBytes(message.content);
            }
            else
            {
                string path_file = $"{path}{file_name}";

                saving_message.data = path_file;

                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                if (!directoryInfo.Exists)
                    directoryInfo.Create();

                using (FileStream stream = new FileStream(path_file, FileMode.OpenOrCreate))
                    stream.Write(message.content, 0, message.content.Length);
            }

            messages.Add(saving_message);

            id++;

            SerializeChat();
            SerializeConfig();

            return saving_message;
        }
        public void Clear()
        {
            DeserializeChat();

            if (clear_status)
                return;

            messages = messages.FindAll(delegate (Message message)
            {
                if (message.date == null)
                    return true;

                int result = DateTime.Compare(clear_date, message.date);

                if (result == 1)
                    return false;

                return true;
            });

            clear_status = true;

            SerializeChat();
        }
        public void Clear(int id)
        {
            DeserializeChat();

            messages = messages.FindAll(delegate (Message message)
            {
                if (message.id == id)
                    return false;
                return true;
            });

            SerializeChat();
        }
        public ChatContainer GetConfig()
        {
            return new ChatContainer()
            {
                last_message = last_message,
                quentity = quentity,
                name = name,
                path_save_files = path_save_files,
            };
        }
        public List<Message> GetMessage()
        {
            DeserializeChat();

            List<Message> list_messages = new List<Message>(messages.Count);
            messages.ForEach((item) =>
            {
                list_messages.Add((Message)item.Clone());
            });

            return list_messages;
        }
        public string GetEncryptingKey()
        {
            return encrypting_key;
        }
        public void AddEncryptingKey(string key)
        { 
            encrypting_key = key;

            SerializeConfig();
        }
        private bool IsSave(string type)
        {
            if (type != IsSendMessage.File.ToString())
                return true;

            return false;
        }
        private void SerializeChat()
        {
            string json = JsonSerializer.Serialize(new ChatSettingsMemento()
            {
                messages = messages
            });

            using (StreamWriter writer = new StreamWriter(path_fileChatSettings))
                writer.WriteAsync(json);
        }
        private void SerializeConfig()
        {
            string json = JsonSerializer.Serialize(new ChatConfigMemento()
            {
                last_messages = last_message,
                quentity = quentity,
                path_save_files = path_save_files,
                clear_date = clear_date.ToString(format_time, culture_info),
                id = id,
                name = name,
                encrypting_key = encrypting_key,
            });

            using (StreamWriter writer = new StreamWriter(path_fileConfigSettings))
                writer.WriteAsync(json);
        }
        private void DeserializeChat()
        {
            if (deserialize_chat_status)
                return;

            string json;

            using (StreamReader reader = new StreamReader(path_fileChatSettings))
                json = reader.ReadToEnd();

            ChatSettingsMemento memento = JsonSerializer.Deserialize<ChatSettingsMemento>(json);

            messages = memento.messages ?? messages;

            deserialize_chat_status = true;

            Clear();
        }
        private void DeserializeConfig()
        {
            if (deserialize_chat_config)
                return;

            string json;

            using (StreamReader reader = new StreamReader(path_fileConfigSettings))
                json = reader.ReadToEnd();

            ChatConfigMemento memento = JsonSerializer.Deserialize<ChatConfigMemento>(json);

            last_message = memento.last_messages;
            path_save_files = memento.path_save_files ?? path_save_files;
            quentity = memento.quentity ?? quentity;
            id = memento.id ?? id;
            name = memento.name ?? name;
            encrypting_key = memento.encrypting_key ?? encrypting_key;

            if (memento.clear_date != null)
            {
                string date = memento.clear_date;

                clear_date = DateTime.ParseExact(date, format_time, culture_info);
            }
        }
    }
}
