using System;
using System.Collections.Generic;

namespace ywBookStoreGUI
{
    public class SignUpValidator
    {
        public List<string> ValidateSignUpForm(string firstName, string lastName, string username,
            string password, string confirmPassword, string email, string phone, string street,
            string city, string province)
        {
            List<string> errors = new List<string>();

            // Check for blank fields
            if (string.IsNullOrWhiteSpace(firstName))
                errors.Add("First Name is required.");

            if (string.IsNullOrWhiteSpace(lastName))
                errors.Add("Last Name is required.");

            if (string.IsNullOrWhiteSpace(username))
                errors.Add("Username is required.");
            else if (username.Length < 3 || username.Length > 20)
                errors.Add("Username must be 3-20 characters long.");

            // Password validation
            if (string.IsNullOrEmpty(password))
                errors.Add("Password is required.");
            else if (password.Length < 6)
                errors.Add("Password must be at least 6 characters long.");
            else if (!char.IsLetter(password[0]))
                errors.Add("Password must start with a letter.");

            // Password confirmation
            if (string.IsNullOrEmpty(confirmPassword))
                errors.Add("Please confirm your password.");
            else if (password != confirmPassword)
                errors.Add("Passwords do not match.");

            // Email validation
            if (string.IsNullOrWhiteSpace(email))
                errors.Add("Email is required.");
            else if (!email.Contains("@") || !email.Contains("."))
                errors.Add("Email must contain @ and a domain.");

            // Phone validation
            if (string.IsNullOrWhiteSpace(phone))
                errors.Add("Phone number is required.");
            else
            {
                string digitsOnly = "";
                foreach (char c in phone)
                {
                    if (char.IsDigit(c))
                        digitsOnly += c;
                }
                if (digitsOnly.Length != 10)
                    errors.Add("Phone number must contain exactly 10 digits.");
            }

            // Address validation
            if (string.IsNullOrWhiteSpace(street))
                errors.Add("Street address is required.");

            if (string.IsNullOrWhiteSpace(city))
                errors.Add("City is required.");

            if (string.IsNullOrWhiteSpace(province))
                errors.Add("Please select a province.");

            return errors;
        }

        public bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@") && email.Contains(".");
        }

        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            string digitsOnly = "";
            foreach (char c in phone)
            {
                if (char.IsDigit(c))
                    digitsOnly += c;
            }
            return digitsOnly.Length == 10;
        }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            if (password.Length < 6) return false;
            if (!char.IsLetter(password[0])) return false;

            foreach (char c in password)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }
    }
}