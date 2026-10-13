using System_Information_Group_1.Constants;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

public class TopBarController
{
    private readonly Account account;
    private readonly PersonDal personDal;
    public TopBarController()
    {
        this.account = AuthServiceSingleton.GetAccount();
        this.personDal = new PersonDal();
    }


    public string GetAccountTypeAndUsernameString()
    {
        var person = this.personDal.GetPersonWithId(this.account.PersonId);
        var role = this.account.AccountType.ToString();
        var displayName = person != null ? $"{person.FirstName} {person.LastName}" : this.account.Username;
        var loginUsername = this.account.Username;
        return $"{role}: {displayName} ({loginUsername})";
    }
}