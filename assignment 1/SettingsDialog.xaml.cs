using System;
using System.Text.RegularExpressions;
using System.Windows;
using ywBookStoreLIB;

namespace ywBookstoreGUI
{
    public partial class SettingsDialog : Window
    {
        private readonly UserData userData;

        public SettingsDialog(UserData userData)
        {
            InitializeComponent();
            this.userData = userData ?? throw new ArgumentNullException(nameof(userData));
            currentUsernameText.Text = userData.LoginName ?? string.Empty;
            newUsernameTextBox.Text = userData.LoginName ?? string.Empty;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Verify user logged in
            if (userData.UserID <= 0 || !userData.LoggedIn)
            {
                MessageBox.Show("You must be logged in to change account settings.", "Account Settings", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string currentPassword = currentPasswordBox.Password ?? string.Empty;
            string newUsername = newUsernameTextBox.Text?.Trim() ?? string.Empty;
            string newPassword = newPasswordBox.Password ?? string.Empty;

            if (string.IsNullOrEmpty(currentPassword))
            {
                MessageBox.Show("Please enter your current password to confirm changes.", "Account Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Verify current password by attempting login for this user name
            var dal = new DALUserInfo();
            int verifiedId = dal.LogIn(userData.LoginName, currentPassword);
            if (verifiedId != userData.UserID)
            {
                MessageBox.Show("Current password is incorrect.", "Account Settings", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate new username (non-empty)
            if (string.IsNullOrWhiteSpace(newUsername))
            {
                MessageBox.Show("New username cannot be empty.", "Account Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Validate new password (if provided): require at least 6 chars and start with letter and contain only letters/digits
            if (!string.IsNullOrEmpty(newPassword))
            {
                if (newPassword.Length < 6)
                {
                    MessageBox.Show("New password must be at least 6 characters long.", "Account Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (!Regex.IsMatch(newPassword, @"^[A-Za-z][A-Za-z0-9]*$"))
                {
                    MessageBox.Show("New password must start with a letter and contain only letters and digits.", "Account Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            // Apply changes: if newPassword is empty, keep existing password unchanged
            bool success = userData.UpdateCredentials(newUsername, string.IsNullOrEmpty(newPassword) ? currentPassword : newPassword);
            if (success)
            {
                MessageBox.Show("Account settings updated.", "Account Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to update account settings. The username might be in use.", "Account Settings", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}