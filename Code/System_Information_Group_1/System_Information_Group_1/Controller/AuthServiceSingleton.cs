using System;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

public class AuthServiceSingleton : IAuthService
{
    private static Account currentAccount;
    private AccountDal currentAccountDal;
    public AuthServiceSingleton(String username, String password)
    {
        this.currentAccount = new Account();
        this.currentAccountDal;
    }

    public bool getUserRole()
    {
        throw new System.NotImplementedException();
    }

    public bool isValidUser()
    {
        throw new System.NotImplementedException();
    }

    public bool updateUserInfo()
    {
        if (this.isValidUser())
        {
            updateAccount
        }
    }

}