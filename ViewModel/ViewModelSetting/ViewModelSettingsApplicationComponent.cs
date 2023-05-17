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
    public class ViewModelSettingsApplicationComponent : INotifyPropertyChanged
    {
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
                Core.Core.Settings.SetActualServer(selected);
                OnPropertyChanged();
            }
        }
        public ViewModelSettingsApplicationComponent() 
        {
            Core.Core.Settings.EventChangeActualServer += ChangeServer;
            Core.Core.Settings.EventDeleteActualServer += DeleteServer;

            ChangeServer(Core.Core.Settings.GetConfigServer());
        }

        public void AddServer(string uri)
        {
            Servers.Add(uri);

            SelectedServer= uri;
        }

        public void ChangeServer(ServerSettingsContainer container)
        {
            if (container.actualURIServer == selected)
                return;

            Servers = new ObservableCollection<string>(container.listServer);

            SelectedServer = container.actualURIServer;
        }

        public void DeleteServer(ServerSettingsContainer container)
        {
            Servers = new ObservableCollection<string>(container.listServer);

            SelectedServer = " ";
        }

        public void DeleteServer(string uri)
        {
            Core.Core.Settings.DeleteServer(uri);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null) 
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
