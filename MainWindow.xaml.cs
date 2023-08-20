using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
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
using Avrora.Core.Settings.ChatSettings;
using Avrora.Pages;
using Avrora.ViewModel.ViewModelChat;
using Avrora.Windows;
using Avrora.Core;
using System.Windows.Threading;
using System.Threading;

namespace Avrora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private ViewModelChatsSettings VMCS;
        public MainWindow()
        {
            InitializeComponent();

            VMCS = (ViewModelChatsSettings)Application.Current.Resources["viewModelChatSettings"];
            VMCS.OnStatusServer += ErrorPanelStatusServerForChatSet;
        }

        public void ErrorPanelStatusServerForChatSet(string text, bool visibility)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                if (visibility)
                {
                    ErrorStatusServerTextBlock.Text = text;
                    ErrorPanelStatusServerForChat.Visibility = Visibility.Visible;
                    return;
                }

                ErrorPanelStatusServerForChat.Visibility = Visibility.Collapsed;
            });
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

        private void ChatGridBlockTextBoxChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && !string.IsNullOrWhiteSpace(ChatGridBlockTextBoxChat.Text))
            {
                VMCS.SendMessage(ChatGridBlockTextBoxChat.Text, IsTypeMessage.Text);

                ChatGridBlockTextBoxChat.Text = "";
            }
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog OFD = new System.Windows.Forms.OpenFileDialog();

            if (OFD.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            
            string name = System.IO.Path.GetFileName(OFD.FileName);

            FileIs fileis = IsFile.Is(name);

            HowIsSendFile win = new HowIsSendFile(fileis, name);

            if (win.ShowDialog() ?? false)
            {
                VMCS.SendMessage(OFD.FileName, IsFile.ConvertInIsSendMessage(win.Is));
            }
        }
    }

    public class ConverterVisibilityForListChatsItems : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;

            if (val==0)
                return Visibility.Hidden;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class ConverterQuentityForListChatsItems : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = (int)value;

            if (val < 10)
                return val;
            return 9;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
