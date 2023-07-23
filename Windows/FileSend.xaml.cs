using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Avrora.Windows
{
    /// <summary>
    /// Логика взаимодействия для HowIsSendFile.xaml
    /// </summary>
    public partial class HowIsSendFile : Window
    {
        private FileIs fileIs;
        public FileIs Is
        {
            get
            {
                if (alternativeButton.IsChecked??false)
                    return fileIs;
                return FileIs.File;
            }
        }
        public HowIsSendFile(FileIs fileIs, string filename)
        {
            this.fileIs = fileIs;
            
            InitializeComponent();

            TextBlockFileName.Text = filename;

            if(fileIs!=FileIs.File)
            {
                TextBlockForAlternative.Text= fileIs.ToString();
            } else
            {
                FileOrAlternative.Visibility = Visibility.Hidden;
            }
        }

        private void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) 
                DragMove();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            Close();
        }
    }
}
