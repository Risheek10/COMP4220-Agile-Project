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

namespace ywBookStoreGUI
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

        // ESSENTIAL: Login button functionality
        private void okButton_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        // ESSENTIAL: Cancel button functionality  
        private void cancelButton_click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        // NEW: Enhanced Sign Up button with comprehensive form
        private void signUpButton_click(object sender, RoutedEventArgs e)
        {
            // Create enhanced sign-up window
            Window signUpWindow = new Window
            {
                Title = "Create New Account",
                Width = 400,
                Height = 550,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize
            };

            // Create scrollable form
            ScrollViewer scrollViewer = new ScrollViewer();
            StackPanel mainPanel = new StackPanel { Margin = new Thickness(20) };
            scrollViewer.Content = mainPanel;
            signUpWindow.Content = scrollViewer;

            // Personal Information Section
            mainPanel.Children.Add(new Label { Content = "Personal Information", FontWeight = FontWeights.Bold, FontSize = 14 });
            
            mainPanel.Children.Add(new Label { Content = "First Name:" });
            TextBox firstNameBox = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            mainPanel.Children.Add(firstNameBox);

            mainPanel.Children.Add(new Label { Content = "Last Name:" });
            TextBox lastNameBox = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            mainPanel.Children.Add(lastNameBox);

            // Account Information Section
            mainPanel.Children.Add(new Label { Content = "Account Information", FontWeight = FontWeights.Bold, FontSize = 14, Margin = new Thickness(0, 10, 0, 0) });
            
            mainPanel.Children.Add(new Label { Content = "Username:" });
            TextBox usernameBox = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            mainPanel.Children.Add(usernameBox);

            mainPanel.Children.Add(new Label { Content = "Password:" });
            PasswordBox passwordBox = new PasswordBox { Margin = new Thickness(0, 0, 0, 10) };
            mainPanel.Children.Add(passwordBox);

            mainPanel.Children.Add(new Label { Content = "Confirm Password:" });
            PasswordBox confirmPasswordBox = new PasswordBox { Margin = new Thickness(0, 0, 0, 10) };
            mainPanel.Children.Add(confirmPasswordBox);

            // Contact Information Section
            mainPanel.Children.Add(new Label { Content = "Contact Information", FontWeight = FontWeights.Bold, FontSize = 14, Margin = new Thickness(0, 10, 0, 0) });
            
            mainPanel.Children.Add(new Label { Content = "Email:" });
            TextBox emailBox = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            mainPanel.Children.Add(emailBox);

            mainPanel.Children.Add(new Label { Content = "Phone Number:" });
            TextBox phoneBox = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            mainPanel.Children.Add(phoneBox);

            // Address Section
            mainPanel.Children.Add(new Label { Content = "Address", FontWeight = FontWeights.Bold, FontSize = 14, Margin = new Thickness(0, 10, 0, 0) });
            
            mainPanel.Children.Add(new Label { Content = "Street Address:" });
            TextBox streetBox = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            mainPanel.Children.Add(streetBox);

            mainPanel.Children.Add(new Label { Content = "City:" });
            TextBox cityBox = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
            mainPanel.Children.Add(cityBox);

            mainPanel.Children.Add(new Label { Content = "Province:" });
            ComboBox provinceBox = new ComboBox { Margin = new Thickness(0, 0, 0, 15) };
            
            // Add Canadian provinces
            provinceBox.Items.Add("AB - Alberta");
            provinceBox.Items.Add("BC - British Columbia");
            provinceBox.Items.Add("MB - Manitoba");
            provinceBox.Items.Add("NB - New Brunswick");
            provinceBox.Items.Add("NL - Newfoundland and Labrador");
            provinceBox.Items.Add("NS - Nova Scotia");
            provinceBox.Items.Add("NT - Northwest Territories");
            provinceBox.Items.Add("NU - Nunavut");
            provinceBox.Items.Add("ON - Ontario");
            provinceBox.Items.Add("PE - Prince Edward Island");
            provinceBox.Items.Add("QC - Quebec");
            provinceBox.Items.Add("SK - Saskatchewan");
            provinceBox.Items.Add("YT - Yukon");
            
            mainPanel.Children.Add(provinceBox);

            // Buttons
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0)
            };

            Button createButton = new Button
            {
                Content = "Create Account",
                Width = 120,
                Height = 30,
                Margin = new Thickness(0, 0, 10, 0)
            };

            Button cancelButton = new Button
            {
                Content = "Cancel",
                Width = 80,
                Height = 30
            };

            buttonPanel.Children.Add(createButton);
            buttonPanel.Children.Add(cancelButton);
            mainPanel.Children.Add(buttonPanel);

            // Button events with comprehensive validation
            createButton.Click += (s, args) =>
            {
                // Validate all fields
                List<string> errors = new List<string>();
                
                // Check for blank fields
                if (string.IsNullOrWhiteSpace(firstNameBox.Text))
                    errors.Add("First Name is required.");
                
                if (string.IsNullOrWhiteSpace(lastNameBox.Text))
                    errors.Add("Last Name is required.");
                
                if (string.IsNullOrWhiteSpace(usernameBox.Text))
                    errors.Add("Username is required.");
                else if (usernameBox.Text.Length < 3 || usernameBox.Text.Length > 20)
                    errors.Add("Username must be 3-20 characters long.");
                
                if (string.IsNullOrEmpty(passwordBox.Password))
                    errors.Add("Password is required.");
                else if (passwordBox.Password.Length < 6)
                    errors.Add("Password must be at least 6 characters long.");
                else if (!char.IsLetter(passwordBox.Password[0]))
                    errors.Add("Password must start with a letter.");
                else
                {
                    // Check password contains only letters and numbers
                    bool validPassword = true;
                    foreach (char c in passwordBox.Password)
                    {
                        if (!char.IsLetterOrDigit(c))
                        {
                            validPassword = false;
                            break;
                        }
                    }
                    if (!validPassword)
                        errors.Add("Password can only contain letters and numbers.");
                }
                
                if (string.IsNullOrEmpty(confirmPasswordBox.Password))
                    errors.Add("Please confirm your password.");
                else if (passwordBox.Password != confirmPasswordBox.Password)
                    errors.Add("Passwords do not match.");
                
                if (string.IsNullOrWhiteSpace(emailBox.Text))
                    errors.Add("Email is required.");
                else if (!emailBox.Text.Contains("@") || !emailBox.Text.Contains("."))
                    errors.Add("Email must contain @ and a domain (e.g., user@example.com).");
                
                if (string.IsNullOrWhiteSpace(phoneBox.Text))
                    errors.Add("Phone number is required.");
                else
                {
                    // Count only digits in phone number
                    string digitsOnly = "";
                    foreach (char c in phoneBox.Text)
                    {
                        if (char.IsDigit(c))
                            digitsOnly += c;
                    }
                    if (digitsOnly.Length != 10)
                        errors.Add("Phone number must contain exactly 10 digits.");
                }
                
                if (string.IsNullOrWhiteSpace(streetBox.Text))
                    errors.Add("Street address is required.");
                
                if (string.IsNullOrWhiteSpace(cityBox.Text))
                    errors.Add("City is required.");
                
                if (provinceBox.SelectedItem == null)
                    errors.Add("Please select a province.");
                
                // Names validation - only letters and spaces
                if (!string.IsNullOrWhiteSpace(firstNameBox.Text))
                {
                    bool validFirstName = true;
                    foreach (char c in firstNameBox.Text)
                    {
                        if (!char.IsLetter(c) && c != ' ')
                        {
                            validFirstName = false;
                            break;
                        }
                    }
                    if (!validFirstName)
                        errors.Add("First name can only contain letters and spaces.");
                }
                
                if (!string.IsNullOrWhiteSpace(lastNameBox.Text))
                {
                    bool validLastName = true;
                    foreach (char c in lastNameBox.Text)
                    {
                        if (!char.IsLetter(c) && c != ' ')
                        {
                            validLastName = false;
                            break;
                        }
                    }
                    if (!validLastName)
                        errors.Add("Last name can only contain letters and spaces.");
                }
                
                // If there are errors, show them
                if (errors.Count > 0)
                {
                    string errorMessage = "Please fix the following errors:\n\n" + string.Join("\n", errors);
                    MessageBox.Show(errorMessage, "Validation Errors", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Don't proceed with account creation
                }
                
                // If we get here, all validation passed!
                string selectedProvince = provinceBox.SelectedItem.ToString();
                
                MessageBox.Show("✅ All validation passed! Account would be created here!\n\n" +
                               $"Name: {firstNameBox.Text} {lastNameBox.Text}\n" +
                               $"Username: {usernameBox.Text}\n" +
                               $"Email: {emailBox.Text}\n" +
                               $"Phone: {phoneBox.Text}\n" +
                               $"Address: {streetBox.Text}, {cityBox.Text}\n" +
                               $"Province: {selectedProvince}", "Account Creation Success",
                               MessageBoxButton.OK, MessageBoxImage.Information);
                signUpWindow.Close();
            };

            cancelButton.Click += (s, args) => signUpWindow.Close();

            // Show the form
            signUpWindow.ShowDialog();
        }

		private void recoverPasswordButton_click(object sender, RoutedEventArgs e)
		{
			var dlg = new recoverPasswordDialog
			{
				Owner = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};

			dlg.ShowDialog();
		}
	}
}
