using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace System_Information_Group_1.InputValidation
{
    /// <summary>
    /// This class is responsible for validating the password input for a login user control.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ValidationRule" />
    public class PasswordValidator : ValidationRule 
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            String password = (String)value;

            if (password == null)
            {
                return new ValidationResult(false, "Password cannot be empty");
            }

            else if (password.Length < 8)
            {
                return new ValidationResult(false, "Password must be at least 8 characters long.");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
