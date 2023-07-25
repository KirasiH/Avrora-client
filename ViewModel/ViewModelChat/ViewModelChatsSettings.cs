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
using System.Threading.Tasks;

namespace Avrora.ViewModel.ViewModelChat 
{ 
    public class ViewModelChatsSettings : INotifyPropertyChanged
    {
        private ObservableCollection<Chat> chats = new ObservableCollection<Chat>();
        private Chat? chat;
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
            Core.Core.EventChangeActualServer += ChangeActualServer;
            Core.Core.EventChangeActualUser += ChangeActualUser;

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
        public void AddChat(string nickname)
        {
            Core.Core.Settings.AddChat(nickname);

            TakeChats();
        }
        public void SendMessage(string data, IsSendMessage isSend)
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
            List<ChatContainer> list_chats = Core.Core.Settings.GetChats();

            chats.Clear();

            list_chats.ForEach((container) =>
                {
                    Chat chat = new Chat(container);

                    chats.Add(chat);
                });
        }
    }
}
