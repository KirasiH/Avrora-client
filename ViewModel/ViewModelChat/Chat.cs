using Avrora.Core.JsonClassesContainers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.ViewModel.ViewModelChat
{
    public class Chat : INotifyPropertyChanged
    {
        private string nickname;
        private string path_save_files;
        private string name = " ";
        private Message last_message = new Message() { type = "text|", data = " " };
        private int quentity = 0;
        private ObservableCollection<Message> messages = new ObservableCollection<Message>();

        public string Nickname
        {
            get { return nickname; }
            set
            {
                nickname = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public Message LastMessage
        {
            get { return last_message; }
            set
            {
                last_message = value;
                OnPropertyChanged();
            }
        }
        public int Quentity
        {
            get { return quentity; }
            set
            {
                quentity = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                OnPropertyChanged();
            }
        }
        public string PathSaveFiles
        {
            get { return path_save_files; }
            set
            {
                path_save_files = value;
                OnPropertyChanged();
            }
        }
        public Chat(ChatContainer container)
        {
            nickname = container.nickname;
            path_save_files = container.path_save_files;
            name = container.name ?? name;
            quentity = container.quentity ?? quentity;
            last_message = container.last_message ?? last_message;
        }
        public void Delete()
        {
            Core.Core.Settings.DeleteChat(nickname);
        }
        public void ChangePathFile(string path)
        {
            Core.Core.Settings.SetPathSave(nickname, path);

            PathSaveFiles= path;
        }      
        public void SetEncruptingKey(string encruptingKey)
        {
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
