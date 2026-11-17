using System;
using System.Globalization;
using System.Windows.Controls;

namespace System_Information_Group_1.InputValidation
{
    /// <summary>
    /// Validates the username input for a login user control.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ValidationRule" />
    public class UsernameValidator : ValidationRule
    {
        /// <summary>
        /// Validates that the username is present and at least 5 characters long.
        /// </summary>
        /// <param name="value">The value from the binding target to check.</param>
        /// <param name="cultureInfo">The culture to use in this rule.</param>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ValidationResult" /> object.
        /// </returns>
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
