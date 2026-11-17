using System;
using System.Globalization;
using System.Windows.Controls;

namespace System_Information_Group_1.InputValidation
{
    /// <summary>
    /// This class serves to validate the inputs for the routine-checkup page
    /// @author Colby
    /// @version Spring 2026
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ValidationRule" />
    public class RoutineCheckupValidator : ValidationRule
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        public String FieldName { get; set;  }
        /// <summary>
        /// Determines the minimum of the parameters.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public double Min { get; set;  }
        /// <summary>
        /// Determines the maximum of the parameters.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        public double Max { get; set;  }


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
            string text = value as string;

            if (string.IsNullOrWhiteSpace(text))
            {
                return new ValidationResult(false, this.FieldName + " is required");
            }

            if (!double.TryParse(text, NumberStyles.Float, cultureInfo, out double number))
            {
                return new ValidationResult(false, this.FieldName + " can only be a number");
            }

            if (number < this.Min)
            {
                return new ValidationResult(false, this.FieldName + " must be at least " + this.Min);
            }

            if (number > this.Max)
            {
                return new ValidationResult(false, this.FieldName + " cannot be greater than " + this.Max);
            }

            return ValidationResult.ValidResult;
        }
    }
}
