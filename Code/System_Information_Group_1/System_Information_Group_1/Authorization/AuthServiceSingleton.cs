using System;
using System.Runtime.CompilerServices;
using System_Information_Group_1.Constants;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

/// <summary>
/// The authentication singleton class that attempts login and stores the account persistently across the application.
/// @author Matthew & Colby
/// @version Fall 2026
/// </summary>
/// <seealso cref="System_Information_Group_1.Controller.IAuthService" />
public class AuthServiceSingleton : IAuthService
{
    private static Account currentAccount;
    private AccountDal accountDal;
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthServiceSingleton"/> class.
    /// </summary>
    public AuthServiceSingleton() {
        this.accountDal = new AccountDal();
    }
    /// <summary>
    /// Gets the user role.
    /// </summary>
    /// <returns>The users role</returns>
    /// <exception cref="System.AccessViolationException">user is not logged in</exception>
    public AccountType GetUserRole()
    {
        return this.IsLoggedIn()
            ? AuthServiceSingleton.currentAccount.AccountType
            : throw new AccessViolationException("user is not logged in ");
    }

    /// <summary>
    /// Attempts the login.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <returns>True or false based on if the login was successful</returns>
    public bool AttemptLogin(String username, String password)
    {
        
        Account account = this.accountDal.GetAccountWithUserName(username);
        if (account == null)
        {
            return false;
        }

        bool passwordMatches = PasswordHasher.Verify(password, account.HashedPassword);

        if (passwordMatches)
        {
            AuthServiceSingleton.currentAccount = account;
        }

        return passwordMatches;
        
    }
    /// <summary>
    /// Determines whether [is logged in].
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is logged in]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsLoggedIn()
    {
        return AuthServiceSingleton.currentAccount != null;
    }
    /// <summary>
    /// Gets the account.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.AccessViolationException">user is not logged in</exception>
    public static Account GetAccount()
    {
        return AuthServiceSingleton.currentAccount != null ? AuthServiceSingleton.currentAccount 
            : throw new AccessViolationException("user is not logged in ");
    }

}