using System.Windows;
using ywBookStoreLIB;

namespace ywBookStoreGUI
{
    public partial class UserDetailDialog : Window
    {
        public UserDetailDialog(UserData user)
        {
            InitializeComponent();
            if (user != null)
            {
                UserIDTextBlock.Text = user.UserID.ToString();
                UsernameTextBlock.Text = user.UserName;
                PasswordTextBlock.Text = user.Password;
                TypeTextBlock.Text = user.Type;
                ManagerCheckBox.IsChecked = user.Manager;
                // FullNameTextBlock.Text = user.FullName; // Assuming FullName is a property in UserData
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
