using Avrora.ViewModel.ViewModelSettingComponents;
using Avrora.ViewModel.ViewModelSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.ViewModel
{
    public class ViewModelSetting : INotifyPropertyChanged
    {

        public ViewModelUserSettingsComponent viewModelUserSettingsComponent;
        public ViewModelSettingsApplicationComponent viewModelSettingsApplicationComponent;

        public ViewModelSettingsApplicationComponent ViewModelSettingsApplicationComponent
        {
            get { return viewModelSettingsApplicationComponent; }
            set
            {
                viewModelSettingsApplicationComponent = value;
                OnPropertyChanged();
            }
        }

        public ViewModelUserSettingsComponent ViewModelUserSettingsComponent
        {
            get
            {
                return viewModelUserSettingsComponent;
            }

            set
            {
                viewModelUserSettingsComponent = value;
                OnPropertyChanged();
            }
        }
        public ViewModelSetting() 
        {
            viewModelUserSettingsComponent= new ViewModelUserSettingsComponent();
            viewModelSettingsApplicationComponent= new ViewModelSettingsApplicationComponent();
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
