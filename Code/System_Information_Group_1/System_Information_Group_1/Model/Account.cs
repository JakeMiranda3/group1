using System;

using System_Information_Group_1.Constants;


namespace System_Information_Group_1.Model
{
    /// <summary>
    /// The account class represents a user account in the system.
    /// @author Colby
    /// @version Fall 2026
    /// </summary>
    public class Account
    {
        #region Properties        
        /// <summary>
        /// Gets the person identifier.
        /// </summary>
        /// <value>
        /// The person identifier.
        /// </value>
        public int PersonId { get; private set; }
        /// <summary>
        /// Gets the type of the account.
        /// </summary>
        /// <value>
        /// The type of the account.
        /// </value>
        public AccountType AccountType { get; private set; }
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public String Username { get; private set; }
        /// <summary>
        /// Gets the hashed password.
        /// </summary>
        /// <value>
        /// The hashed password.
        /// </value>
        public String HashedPassword { get; private set; }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="accountType">Type of the account.</param>
        /// <param name="username">The username.</param>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <exception cref="System.ArgumentException">
        /// PersonId cannot be zero.
        /// or
        /// Username cannot be null or whitespace.
        /// or
        /// HashedPassword cannot be null or whitespace.
        /// </exception>
        public Account(int personId, AccountType accountType, String username, String hashedPassword)
        {
            this.PersonId = personId != 0 ? personId : throw new ArgumentException("PersonId cannot be zero.");
            this.AccountType = accountType;
            this.Username = !string.IsNullOrWhiteSpace(username) ? username : throw new ArgumentException("Username cannot be null or whitespace.");
            this.HashedPassword = !string.IsNullOrWhiteSpace(hashedPassword) ? hashedPassword : throw new ArgumentException("HashedPassword cannot be null or whitespace.");
        }

        #endregion
    }
}
