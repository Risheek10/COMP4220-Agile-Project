using System.Windows;
using ywBookStoreLIB;

namespace ywBookStoreGUI
{
    public partial class UserDialog : Window
    {
        public UserData User { get; private set; }

        public UserDialog(UserData user = null)
        {
            InitializeComponent();
            if (user != null)
            {
                User = user;
                UserNameTextBox.Text = user.UserName;
                PasswordTextBox.Text = user.Password;
                TypeTextBox.Text = user.Type;
                ManagerCheckBox.IsChecked = user.Manager;
            }
            else
            {
                User = new UserData();
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            User.UserName = UserNameTextBox.Text;
            User.Password = PasswordTextBox.Text;
            User.Type = TypeTextBox.Text;
            User.Manager = ManagerCheckBox.IsChecked.Value;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
