using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core;

namespace Avrora.ViewModel.ViewModelSettings
{
    public class ViewModelUserSettingsComponent : IViewModelSettingsComponent, INotifyPropertyChanged
    {
        public string name;
        public string nickname;
        public string first_key;
        public string second_key;

        public ViewModelUserSettingsComponent()
        {
            var settings = Core.Core.Settings.userSettings;

            Name = settings.name ?? "Not exists name";
            Nickname = settings.nickname ?? "Not exists nickname";
            FirstKey = settings.first_key ?? "None";
            SecondKey = settings.second_key ?? "None";
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

        public string Nickname
        {
            get { return nickname; }
            set
            {
                nickname = value;
                OnPropertyChanged();
            }
        }

        public string FirstKey
        {
            get { return first_key; }
            set
            {
                first_key = value;
                OnPropertyChanged();
            }
        }

        public string SecondKey
        {
            get { return second_key; }
            set
            {
                second_key = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
