using System;
using System.Globalization;
using System.Windows.Controls;

namespace System_Information_Group_1.InputValidation
{
    /// <summary>
    /// This class is responsible for validating the password input for a login user control.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ValidationRule" />
    public class PasswordValidator : ValidationRule 
    {
        /// <summary>
        /// When overridden in a derived class, performs validation checks on a value.
        /// </summary>
        /// <param name="value">The value from the binding target to check.</param>
        /// <param name="cultureInfo">The culture to use in this rule.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ValidationResult" /> object.
        /// </returns>
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
