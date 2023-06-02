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
using Avrora.Core;

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

            Core.Core.EventUserMethods += EventUserMethods;
            Core.Core.EventErrorURIServer += ErrorURIServer;
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

            Core.Core.DeleteUserAsync(container);
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

            Core.Core.RecreateUserAsync(twoContainer);
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

            Core.Core.CreateUserAsync(container);
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

            Core.Core.SetActualUser(container);
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
