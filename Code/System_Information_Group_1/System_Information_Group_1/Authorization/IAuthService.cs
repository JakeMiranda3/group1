

using System;
using System.Windows.Controls;
using System_Information_Group_1.Constants;

namespace System_Information_Group_1.Controller;

/// <summary>
/// Defines the autherization service interface.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Attempts the login.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <returns>True if the login was valid</returns>
    bool AttemptLogin(String username, String password);

    /// <summary>
    /// Gets the user role.
    /// </summary>
    /// <returns>The users account type</returns>
    AccountType GetUserRole();
}