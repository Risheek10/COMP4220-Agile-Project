using System.Windows;

namespace ywBookStoreGUI
{
    public partial class ResetPasswordDialog : Window
    {
        public string NewPassword { get; private set; }

        public ResetPasswordDialog()
        {
            InitializeComponent();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (NewPasswordTextBox.Text == ConfirmPasswordTextBox.Text)
            {
                NewPassword = NewPasswordTextBox.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
