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

namespace Avrora.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageApplicationSettings.xaml
    /// </summary>
    public partial class PageApplicationSettings : Page
    {
        public PageApplicationSettings()
        {
            InitializeComponent();
        }

        private void Click_AddServer(object sender, RoutedEventArgs e)
        {
            Core.Core.Settings.applicationSettings.SetActualServer(new ApplicationSettingsContainer() { actualURIServer = TextBoxNameServer.Text });
        }

        private void Click_DeleteServer(object sender, RoutedEventArgs e)
        {
            Core.Core.Settings.applicationSettings.DeleteServer(new ApplicationSettingsContainer() { actualURIServer = TextBlockDeleteServer.Text });
        }

        private void SelectedChanged(object sender, SelectionChangedEventArgs e) 
        {
            ComboBox combox = (ComboBox)sender;

            ApplicationSettingsContainer container = new ApplicationSettingsContainer() { actualURIServer = (string)combox.SelectedItem };

            Core.Core.Settings.applicationSettings.SetActualServer(container);
            Core.Core.proxyAvroraAPI.ChangeURI(container.actualURIServer);
        }
    }
}
