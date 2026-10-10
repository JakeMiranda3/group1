

using System.Windows.Controls;

namespace System_Information_Group_1.Controller;

public interface IAuthService
{
    bool isValidUser();
    bool getUserRole();
}