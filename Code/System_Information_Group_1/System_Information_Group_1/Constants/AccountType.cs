using System;



namespace System_Information_Group_1.Constants
{
    /// <summary>
    /// The account type class
    /// @author Colby
    /// @version Fall 2026
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// The administrator
        /// </summary>
        Administrator,
        /// <summary>
        /// The nurse
        /// </summary>
        Nurse
    }
    /// <summary>
    /// Class containing extension methods for the AccountType enum.
    /// </summary>
    public static class AccountTypeExtensions
    {
        /// <summary>
        /// Creates the correct enum type from the string.
        /// </summary>
        /// <param name="accountTypeString">The account type string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Invalid account type: {accountTypeString}</exception>
        public static AccountType FromString(string accountTypeString)
        {
            if (Enum.TryParse(accountTypeString, true, out AccountType accountType))
            {
                return accountType;
            }
            else
            {
                throw new ArgumentException($"Invalid account type: {accountTypeString}");
            }
        }

    }

}
