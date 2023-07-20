﻿using System;
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
using Avrora.Pages;

namespace Avrora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonWindowsMinimize(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        
        private void WindowsStateButton(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Click_StackPanelUserListChatGroup(object sender, MouseButtonEventArgs e)
        {
            ListChatGrid.Visibility = Visibility.Hidden;
            ChatGridBlock.Visibility = Visibility.Hidden;

            ListSettingsGrid.Visibility= Visibility.Visible;
            SettingsGridBlock.Visibility= Visibility.Visible;
        }

        private void Click_ButtonExitListSettingsGrid(object sender, RoutedEventArgs e)
        {
            ListSettingsGrid.Visibility = Visibility.Hidden;
            SettingsGridBlock.Visibility = Visibility.Hidden;

            ListChatGrid.Visibility = Visibility.Visible;
            ChatGridBlock.Visibility = Visibility.Visible;
        }

        private void Click_SettingsButton(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Name == "UserButton")
                SettingsFrame.Navigate(new PageSettingsUser());
            else if (btn.Name == "ApplicationButton")
                SettingsFrame.Navigate(new PageApplicationSettings());
            else if (btn.Name == "ChatButton")
                SettingsFrame.Navigate(new PageChatsSettings());
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
