using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
using ywBookStoreLIB;

namespace BookStoreGUI
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void okButton_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void cancelButton_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


