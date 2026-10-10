

using System;
using System.Windows.Controls;
using System_Information_Group_1.Constants;

namespace System_Information_Group_1.Controller;

public interface IAuthService
{
    bool AttemptLogin(String username, String password);
    AccountType GetUserRole();
}