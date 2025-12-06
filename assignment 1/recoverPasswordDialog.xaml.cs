using System;
using System.Text.RegularExpressions;
using System.Windows;
using ywBookStoreLIB;

namespace ywBookStoreGUI
{
    public partial class recoverPasswordDialog : Window
    {
        private string _verificationCode;
        private int _targetUserId = -1;
        private string _targetUserName;

        public recoverPasswordDialog()
        {
            InitializeComponent();

            SendCodeButton.Click += SendCodeButton_Click;
            CancelButton.Click += (s, e) => this.Close();
            VerifyButton.Click += VerifyButton_Click;
            BackButton.Click += BackButton_Click;
        }

        private void SendCodeButton_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = string.Empty;
            string username = UsernameTextBox.Text?.Trim();
            string email = EmailTextBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                StatusText.Text = "Please enter a username.";
                return;
            }

            // check if username exists in DB
            var dal = new DALUserInfo();
            int uid = dal.GetUserIdByUserName(username);
            if (uid <= 0)
            {
                StatusText.Text = "Username not found.";
                return;
            }

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                StatusText.Text = "Please enter a valid email address.";
                return;
            }

            // if valid account, save found user info
            _targetUserId = uid;
            _targetUserName = username;

            // Generate a simple 6-digit verification code
            var rng = new Random();
            _verificationCode = rng.Next(100000, 999999).ToString();

            // In a production app you would send the code to the user's email address here.
            // For this sample/demo we show a message so the flow can be tested locally.
            MessageBox.Show(this,
                $"A verification code has been sent to {email}.\n\n(For demo purposes the code is: {_verificationCode})",
                "Verification Sent",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            // Lock username/email input and show verification / reset UI
            UsernameTextBox.IsEnabled = false;
            EmailTextBox.IsEnabled = false;
            SendCodeButton.IsEnabled = false;
            VerificationPanel.Visibility = Visibility.Visible;
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = string.Empty;

            // if no account found
            if (_targetUserId <= 0)
            {
                StatusText.Text = "No target user. Please enter username and send verification first.";
                return;
            }


            if (string.IsNullOrEmpty(_verificationCode))
            {
                StatusText.Text = "No verification code was generated. Please send a code first.";
                return;
            }

            if (CodeTextBox.Text?.Trim() != _verificationCode)
            {
                StatusText.Text = "Verification code is incorrect.";
                return;
            }

            string newPass = NewPasswordBox.Password;
            string confirm = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(newPass))
            {
                StatusText.Text = "Please enter a new password.";
                return;
            }

            if (newPass.Length < 6)
            {
                StatusText.Text = "Password must be at least 6 characters long.";
                return;
            }

            if (newPass != confirm)
            {
                StatusText.Text = "Passwords do not match.";
                return;
            }

            // Update password in DB for the found user
            try
            {
                var dal = new DALUserInfo();
                bool ok = dal.UpdateCredentials(_targetUserId, _targetUserName, newPass);
                if (!ok)
                {
                    StatusText.Text = "Failed to update password in database. Please try again or contact support.";
                    return;
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = "Error updating password: " + ex.Message;
                return;
            }

            MessageBox.Show(this, "Your password has been reset successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // allow user to try another email / resend
            VerificationPanel.Visibility = Visibility.Collapsed;
            UsernameTextBox.IsEnabled = true;
            EmailTextBox.IsEnabled = true;
            SendCodeButton.IsEnabled = true;
            CodeTextBox.Text = string.Empty;
            NewPasswordBox.Password = string.Empty;
            ConfirmPasswordBox.Password = string.Empty;
            _verificationCode = null;
            _targetUserId = -1;
            _targetUserName = null;
            StatusText.Text = string.Empty;
        }

        private bool IsValidEmail(string email)
        {
            // Basic email validation (sufficient for demo)
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // simple regex for basic validation
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
	}
}
