using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core;
using Avrora.Core.JsonClassesContainers;

namespace Avrora.ViewModel.ViewModelSettings
{
    enum StateViewModel
    {
        ChangeAfterEvent,
        ChangeAfterView,
    }

    public class ViewModelSettingsApplicationComponent : INotifyPropertyChanged
    {
        private StateViewModel state;

        public ObservableCollection<string> servers;
        public ObservableCollection<string> Servers
        {
            get { return servers; }
            set 
            { 
                servers = value;
                OnPropertyChanged();
            }
        }

        public string selected;
        public string SelectedServer
        {
            get { return selected; }
            set
            {
                selected = value;
                if (state == StateViewModel.ChangeAfterView)
                {
                    Core.Core.SetActualServer(selected);

                    OnPropertyChanged();
                }
            }
        }
        public ViewModelSettingsApplicationComponent() 
        {
            Core.Core.EventChangeActualServer += ChangeServer;
            Core.Core.EventDeleteActualServer += DeleteServer;

            ChangeServer(Core.Core.GetConfigServers());
        }

        public void AddServer(string uri)
        {
            Servers.Add(uri);

            SelectedServer= uri;
        }

        public void ChangeServer(ServerSettingsContainer container)
        {
            state = StateViewModel.ChangeAfterEvent;

            Servers = new ObservableCollection<string>(container.listServer);

            SelectedServer = container.actualURIServer;

            state = StateViewModel.ChangeAfterView;
        }
        public void DeleteServer(ServerSettingsContainer container)
        {
            state = StateViewModel.ChangeAfterEvent;

            Servers = new ObservableCollection<string>(container.listServer);

            SelectedServer = " ";

            state = StateViewModel.ChangeAfterView;
        }
        public void DeleteServer(string uri)
        {
            Core.Core.DeleteServer(uri);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null) 
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
