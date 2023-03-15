using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avrora.Core;
using Avrora.Core.Settings;
using Avrora.Core.Settings.UserSettings;
using Avrora.Core.JsonClassesContainers;

namespace Avrora.ViewModel.ViewModelSettings
{
    public class ViewModelUserSettingsComponent : INotifyPropertyChanged
    {
        public string name;
        public string nickname;
        public string first_key;
        public string second_key;

        public ViewModelUserSettingsComponent()
        {
            var settings = Core.Core.Settings.userSettings;

            ChangeAttridutes(settings.GetActualUser());

            Core.Core.Settings.userSettings.EventChangeActualUser += ChangeAttridutes;
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

        public void ChangeAttridutes(UserSettingsContainer container)
        {
            Name = container.name ?? "Not exists name";
            Nickname = container.nickname ?? "Not exists nickname";
            FirstKey = container.first_key ?? "None";
            SecondKey = container.second_key ?? "None";
        }

        public void EventChangeAttributes(UserSettingsContainer container, string content)
        {
            if (content == "create" || content == "recreate")
            {
                ChangeAttridutes(container);
            }
        }
    }
}
