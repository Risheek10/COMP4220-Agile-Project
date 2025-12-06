using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ywBookStoreLIB
{
    public class AccountValidationHelper
    {
        // This method validates password complexity
        public bool ValidatePasswordStrength(string password)
        {
            // Check 1: Handle null or empty inputs
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Check 2: Length Requirement (Must be 8 or more)
            if (password.Length < 8)
            {
                return false;
            }

            // Check 3: Uppercase Requirement
            // We compare the password to its lowercase version. 
            // If they are identical, it means no uppercase letters exist.
            if (password == password.ToLower())
            {
                return false;
            }

            // If we survive all checks, the password is valid
            return true;
        }
    }
}