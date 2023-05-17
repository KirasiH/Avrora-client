using Avrora.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Avrora.Core.JsonClassesContainers;
using Avrora.ViewModel.ViewModelSettings;

namespace Avrora.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageApplicationSettings.xaml
    /// </summary>
    public partial class PageApplicationSettings : Page
    {
        private ViewModelSettingsApplicationComponent VMSAC;
        public PageApplicationSettings()
        {
            InitializeComponent();

            VMSAC = (ViewModelSettingsApplicationComponent)Application.Current.Resources["viewModelSettingsApplicationComponent"];
        }

        private void Click_AddServer(object sender, RoutedEventArgs e)
        {
            VMSAC.AddServer(TextBoxNameServer.Text);
        }

        private void Click_DeleteServer(object sender, RoutedEventArgs e)
        {
            VMSAC.DeleteServer(TextBlockDeleteServer.Text);
        }
    }
}
