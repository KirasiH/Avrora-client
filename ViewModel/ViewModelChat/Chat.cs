﻿using Avrora.Core;
using Avrora.Core.JsonClassesContainers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Avrora.ViewModel.ViewModelChat
{
    public class Chat : INotifyPropertyChanged
    {
        private string nickname;
        private string path_save_files;
        private string name = " ";
        private Message last_message = new Message() { type = "Text|", data = " " };
        private int quentity = 0;
        private ObservableCollection<Message> messages = new ObservableCollection<Message>();

        private bool selection = false;
        public bool Selection
        {
            get { return selection; }
            set
            {
                selection = value;

                if (selection)
                {
                    List<Message> list = Core.Core.Settings.GetMessages(nickname);

                    list.ForEach((message) => {

                        if (message.me)
                            message.sender = "you";

                        Messages.Add(message);
                    });

                    if(list.Count != 0)
                        LastMessage = Messages.Last();

                    Quentity = 0;
                }
                else
                {
                    Messages.Clear();
                }
            }
        }
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

                Core.Core.Settings.SetQuentity(nickname, quentity);

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
            Nickname = container.nickname;
            PathSaveFiles = container.path_save_files;
            Name = container.name ?? name;
            Quentity = container.quentity ?? quentity;
            LastMessage = container.last_message ?? last_message;
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
            Core.Core.Settings.AddEncryptingKey(nickname, encruptingKey);
        }
        public void SendMessage(string data, IsTypeMessage isSend)
        {
            Core.Core.SendMessage(nickname, data, isSend);
        }
        public void AddMessage(Message message)
        {
            if (message.me)
                message.sender = "you";

            Messages.Add(message);
            LastMessage = message;

            if (!selection)
                Quentity++;
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
