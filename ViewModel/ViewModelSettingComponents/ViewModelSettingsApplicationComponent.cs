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

namespace Avrora.ViewModel.ViewModelSettingComponents
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
                OnPropertyChanged();
            }
        }
        public ViewModelSettingsApplicationComponent() 
        {
            ApplicationSettingsContainer applicationSettings = Core.Core.Settings.applicationSettings.GetActualServer();

            Core.Core.Settings.applicationSettings.EventChangeActualServer += ChangeServer;
            Core.Core.Settings.applicationSettings.EventDeleteActualServer += DeleteServer;

            Servers = new ObservableCollection<string>(applicationSettings.listServer);

            selected = applicationSettings.actualURIServer;
        }

        public void ChangeServer(ApplicationSettingsContainer container)
        {
            Servers = new ObservableCollection<string>(container.listServer);

            SelectedServer = container.actualURIServer;
        }

        public void DeleteServer(ApplicationSettingsContainer container)
        {
            Servers = new ObservableCollection<string>(container.listServer);

            SelectedServer = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null) 
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
