﻿using Avrora.Core.Settings.UserSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Логика взаимодействия для PageSettingsUser.xaml
    /// </summary>
    public partial class PageSettingsUser : Page
    {
        public PageSettingsUser()
        {
            InitializeComponent();

            Core.Core.proxyAvroraAPI.EventUserMethods += EventUserMethods;
            Core.Core.proxyAvroraAPI.EventErrorURIServer += ErrorURIServer;
        }

        private void Click_ButtonDelete(object sender, RoutedEventArgs e)
        {
            
            UserSettingsContainer container = new UserSettingsContainer() 
            { 
                name = TextBoxName.Text,
                nickname = TextBoxNickname.Text,
                first_key= TextBoxFirstKey.Text,
                second_key= TextBoxSecondKey.Text,
            };

            _ = Core.Core.proxyAvroraAPI.DeleteUserAsync(container);
        }

        private void Click_ButtonRecreate(object sender, RoutedEventArgs e) 
        {
            UserSettingsContainer container = new UserSettingsContainer()
            {
                name = TextBoxName.Text,
                nickname = TextBoxNickname.Text,
                first_key = TextBoxFirstKey.Text,
                second_key = TextBoxSecondKey.Text,
            };

            UserSettingsTwoContainer twoContainer= new UserSettingsTwoContainer() { new_user= container };

            _ = Core.Core.proxyAvroraAPI.RecreateUserAsync(twoContainer);
        }

        private void Click_ButtomCreate(object sender, RoutedEventArgs e)
        {
            UserSettingsContainer container = new UserSettingsContainer()
            {
                name = TextBoxName.Text,
                nickname = TextBoxNickname.Text,
                first_key = TextBoxFirstKey.Text,
                second_key = TextBoxSecondKey.Text,
            };

            _ = Core.Core.proxyAvroraAPI.CreateUserAsync(container);
        }

        private void CLick_ButtonSet(object sender, RoutedEventArgs e)
        {
            UserSettingsContainer container = new UserSettingsContainer()
            {
                name = TextBoxName.Text,
                nickname = TextBoxNickname.Text,
                first_key = TextBoxFirstKey.Text,
                second_key = TextBoxSecondKey.Text,
            };

            Core.Core.Settings.userSettings.SetActualUser(container);
        }

        public void EventUserMethods(UserSettingsContainer conteiner, string content)
        {
            StatusBlock.Text = content;
        }
        
        public void ErrorURIServer(string uri)
        {
            StatusBlock.Text = $"Server with this uri - {uri} dont answer";
        }
    }
}
