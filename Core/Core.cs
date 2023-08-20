using Avrora.Core.AvroraAPI;
using Avrora.Core.JsonClassesContainers;
using Avrora.Core.Settings.ApplicationSettings;
using Avrora.Core.Settings.UserSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Avrora.Core
{
    public enum IsTypeMessage
    {
        Text = 0,
        Photo = 1,
        File = 2,
    }
    public enum ResponceStatus
    {
        ErrorData = 0,
        ErrorDataUser = 1,
        Create = 3,
        ErrorDelete = 4,
        Delete = 5,
        ErrorRecv = 6,
        Recreate = 7,
        Set = 8,
    }

    public enum StatusMessage
    {
        ErrorData = 0,
        ErrorMessage = 1,
        Send = 3,
        ErrorConnect = 4,
    }
    public class Core
    {
        public delegate void DelegateChangeActualServer(ServerSettingsContainer container);
        public static event DelegateChangeActualServer? EventChangeActualServer;

        public delegate void DelegateDeleteActualServer(ServerSettingsContainer container);
        public static event DelegateDeleteActualServer? EventDeleteActualServer;

        public delegate void DelegateChangeUser(UserSettingsContainer container);
        public static event DelegateChangeUser? EventChangeActualUser;

        public delegate void UserMethodsDelegate(UserSettingsContainer conteiner, ResponceStatus resStatus);
        public static event UserMethodsDelegate? EventUserMethods;

        public delegate void DelegateErrorURLServer(string uri);
        public static event DelegateErrorURLServer? EventErrorURIServer;

        public delegate void DelegateRecvMessage(Message message, string nickname);
        public static event DelegateRecvMessage? EventRecvMessage;

        public delegate void DelegateSendMessage(Message message, string nickname);
        public static event DelegateSendMessage? EventSendMessage;

        public delegate void DelegateErrorSendMessage(StatusMessage statusErrorSendMessage);
        public static event DelegateErrorSendMessage EventErrorSendMessage;

        public static Settings.Settings Settings { get; private set; }
        public static AvroraAPI.AvroraAPI AvroraAPI { get; private set; }

        static Core()
        {
            Settings = new Settings.Settings();

            AvroraAPI = new AvroraAPI.AvroraAPI(Settings.GetActualServer(), Settings);

            CycleRecvMessage();
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
                EventUserMethods(container, ConvertInStatus(context));
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
                EventUserMethods(container, ConvertInStatus(context));
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
                EventUserMethods(container.new_user, ConvertInStatus(context));
        }

        public static async void SendMessage(string whom, string data, IsTypeMessage ISM)
        {
            ServerSendMessageContainer message = new ServerSendMessageContainer() {
                whom = whom,
                user = Settings.GetActualUser(),
                content = new MessageContentContainer() {
                    type = $"{ISM}|",
                }
            };

            byte[] mess_data;

            if (ISM == IsTypeMessage.Text)
            {
                mess_data = Encoding.UTF8.GetBytes(data);
            }
            else
            {
                FileStream fileStream = new FileStream(data, FileMode.Open);

                mess_data = new byte[fileStream.Length];

                fileStream.Read(mess_data, 0, (int)fileStream.Length);

                message.content.type += Path.GetFileName(data);
            }

            byte[] key = Encoding.UTF8.GetBytes(Settings.GetEncryptingKey(whom));

            message.content.b = EncryptingData(key, mess_data);

            HttpResponseMessage response = await AvroraAPI.SendUserAsync(message);

            if (response == null)
            {
                EventErrorSendMessage(StatusMessage.ErrorConnect);
                return;
            }

            string context = await response.Content.ReadAsStringAsync();

            if (context == "Send") {
                message.content.b = mess_data;

                Message mess = Settings.AddMessage(message);

                if (EventSendMessage != null)
                    EventSendMessage(mess, whom);
            } else if(context == "Error data") {
                if (EventErrorSendMessage != null)
                    EventErrorSendMessage(StatusMessage.ErrorData);
            } else if (context == "Error Message") {
                if (EventErrorSendMessage != null)
                    EventErrorSendMessage(StatusMessage.ErrorMessage);
            }
        }

        public static void CycleRecvMessage()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);

                    HttpResponseMessage response = AvroraAPI.RecvUserAsync(Settings.GetActualUser()).Result;

                    if (response == null)
                        continue;

                    string context = response.Content.ReadAsStringAsync().Result;

                    if (context == "Error")
                        continue;

                    JsonRecvMessageContainer container = JsonSerializer.Deserialize<JsonRecvMessageContainer>(context);
                    ServerRecvMessageContainer recvmessage = container.json;

                    if (recvmessage == null)
                        continue;

                    string sender = recvmessage.sender_nickname;

                    string e = Settings.GetEncryptingKey(sender);

                    byte[] key = Encoding.UTF8.GetBytes(e);

                    try
                    {
                        recvmessage.content = DecryptingData(key, recvmessage.content);
                    }
                    catch (CryptographicException)
                    {
                        recvmessage.content = Encoding.UTF8.GetBytes("We have another keys");
                        recvmessage.type = "Text|";
                    }

                    Message message = Settings.AddMessage(recvmessage);

                    if (EventRecvMessage != null)
                        EventRecvMessage(message, recvmessage.sender_nickname);
                }
            });

            thread.IsBackground = true;
            thread.Name = "CycleRecvMessage";

            thread.Start();
        }
        private static byte[] EncryptingData(byte[] key, byte[] data)
        {
            Aes aes = Aes.Create();
            SHA256 sha256 = SHA256.Create();
            MD5 md5 = MD5.Create();

            aes.Key = sha256.ComputeHash(key);
            aes.IV = md5.ComputeHash(Encoding.UTF8.GetBytes("IV"));

            using (MemoryStream stream = new MemoryStream())
            {
                using(CryptoStream cryptoStream = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                }
                data = stream.ToArray();
            }

            return data;
        }
        private static byte[] DecryptingData(byte[] key, byte[] data)
        {
            Aes aes = Aes.Create();
            SHA256 sha256 = SHA256.Create();
            MD5 md5 = MD5.Create();

            aes.Key = sha256.ComputeHash(key);
            aes.IV = md5.ComputeHash(Encoding.UTF8.GetBytes("IV"));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                }

                data = memoryStream.ToArray();
            }

            return data;
        }

        private static ResponceStatus ConvertInStatus(string status)
        {
            switch (status)
            {
                case "Error data":
                    return ResponceStatus.ErrorData;

                case "Error data user":
                    return ResponceStatus.ErrorDataUser;

                case "create":
                    return ResponceStatus.Create;

                case "Error delete":
                    return ResponceStatus.Delete;

                case "delete":
                    return ResponceStatus.Delete;

                case "Error recv":
                    return ResponceStatus.ErrorRecv;

                case "recreate":
                    return ResponceStatus.Recreate;
            }

            throw new NotImplementedException($"Not status for {status}");
        }
    }

    class JsonRecvMessageContainer
    {
        public ServerRecvMessageContainer json { get; set; }
    }
}
