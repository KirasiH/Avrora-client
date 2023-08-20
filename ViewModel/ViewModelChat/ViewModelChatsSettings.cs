using Avrora.Core;
using Avrora.Core.JsonClassesContainers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Avrora.ViewModel.ViewModelChat 
{ 
    public class ViewModelChatsSettings : INotifyPropertyChanged
    {
        public delegate void EventStatusServer(string status, bool flag);
        public event EventStatusServer OnStatusServer;
        private object locker = new object();
        private ObservableCollection<Chat> chats = new ObservableCollection<Chat>();
        private Chat? chat;

        private SynchronizationContext syncContext;

        public Chat Chat
        {
            get { return chat; }
            set 
            {
                if (chat != null)
                    chat.Selection = false;

                if (value != null)
                    value.Selection= true;

                chat = value;

                OnPropertyChanged();
            }
        }
        public ObservableCollection<Chat> Chats
        {
            get { return chats; }
            set 
            { 
                chats = value;
                OnPropertyChanged();
            }
        }
        public ViewModelChatsSettings()
        {
            syncContext = SynchronizationContext.Current;
            
            Core.Core.EventChangeActualServer += ChangeActualServer;
            Core.Core.EventChangeActualUser += ChangeActualUser;

            Core.Core.EventSendMessage += AddMessage;
            Core.Core.EventRecvMessage += AddMessage;
            Core.Core.EventErrorSendMessage += ErrorSendMessage;

            TakeChats();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public void AddMessage(Message message, string nickname)
        {
            lock (locker)
            {

                syncContext.Post(_ =>
                {
                    TakeChats();

                    foreach (Chat chat in chats)
                    {
                        if (chat.Nickname == nickname)
                        {
                            chat.AddMessage(message);
                            return;
                        }
                    }
                }, null);

            }
        }
        public void ErrorSendMessage(StatusMessage status)
        {
            if (OnStatusServer == null)
                return;

            switch (status)
            {
                case StatusMessage.ErrorData:
                    OnStatusServer("Error data user", true);
                    break;
                case StatusMessage.ErrorMessage:
                    OnStatusServer("Error user data", true);
                    break;
                case StatusMessage.ErrorConnect:
                    OnStatusServer("Server dont answer", true);
                    break;
            }
        }
        public void AddChat(string nickname)
        {
            Core.Core.Settings.AddChat(nickname);

            TakeChats();
        }
        public void SendMessage(string data, IsTypeMessage isSend)
        {
            chat?.SendMessage(data, isSend);
        }
        public void ChangeActualServer(ServerSettingsContainer _)
        {
            TakeChats();
        }
        public void ChangeActualUser(UserSettingsContainer _)
        {
            TakeChats();
        }
        private void TakeChats()
        {
            Chat old_chat = chat;

            List<ChatContainer> list_chats = Core.Core.Settings.GetChats();

            chats.Clear();

            list_chats.ForEach((container) =>
                {
                    Chat ch = new Chat(container);

                    chats.Add(ch);
                });

            Chat = old_chat;
        }
    }
}
