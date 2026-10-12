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
    /// Validates the username input for a login user control.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ValidationRule" />
    public class UsernameValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            String username = (String)value;

            if (username == null)
            {
                return new ValidationResult(false, "Username cannot be empty.");
            }
            else if (username.Length < 5)
            {
                return new ValidationResult(false, "Username must be at least 5 characters long.");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
