using System_Information_Group_1.Constants;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

/// <summary>
/// The controller for the top app bar.
/// @author Matthew Shirley
/// @version 2026
/// </summary>
public class TopBarController
{
    private readonly Account account;
    private readonly PersonDal personDal;

    /// <summary>
    /// Initializes a new instance of the <see cref="TopBarController"/> class.
    /// </summary>
    public TopBarController()
    {
        this.account = AuthServiceSingleton.GetAccount();
        this.personDal = new PersonDal();
    }

    /// <summary>
    /// Gets the account type and username string.
    /// </summary>
    /// <returns>A string with role, displayname, and login username</returns>
    public string GetAccountTypeAndUsernameString()
    {
        var person = this.personDal.GetPersonWithId(this.account.PersonId);
        var role = this.account.AccountType.ToString();
        var displayName = person != null ? $"{person.FirstName} {person.LastName}" : this.account.Username;
        var loginUsername = this.account.Username;
        return $"{role}: {displayName} ({loginUsername})";
    }
}