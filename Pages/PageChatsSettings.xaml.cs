using Avrora.ViewModel.ViewModelChat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using Avrora.Windows;

namespace Avrora.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageChatsSettings.xaml
    /// </summary>
    public partial class PageChatsSettings : Page
    {
        private ViewModelChatsSettings VMCS;
        public PageChatsSettings()
        {
            InitializeComponent();

            VMCS = (ViewModelChatsSettings)Application.Current.Resources["viewModelChatSettings"];
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            VMCS.AddChat(TextBoxChatName.Text);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Chat chat = (Chat)((Button)sender).DataContext;

            MessageBoxResult result = MessageBox.Show("Do you want to delete the chat?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                chat.Delete();

                VMCS.Chats.Remove(chat);
            }
        }

        private void ChangePathFileButton_CLick(object sender, RoutedEventArgs e)
        {
            Chat chat = (Chat)((Button)sender).DataContext;

            System.Windows.Forms.FolderBrowserDialog FBD = new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = FBD.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                chat.ChangePathFile(FBD.SelectedPath);
            }
        }

        private void ChangeKeyButton_CLick(object sender, RoutedEventArgs e)
        {
            ChangeKeyWindows CKW = new ChangeKeyWindows();
            if (CKW.ShowDialog()??false)
            {
                Chat chat = (Chat)((Button)sender).DataContext;
                chat.SetEncruptingKey(CKW.Key);
            }
        }
    }
}
