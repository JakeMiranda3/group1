using System_Information_Group_1.Authorization;

namespace System_Information_Group_1.Controller
{

    /// <summary>
    /// The login controller class
    /// @author Colby
    /// @version Fall 2026
    /// </summary>
    public class LoginController
    {
        private AuthServiceSingleton authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        public LoginController()
        {
            this.authService = new AuthServiceSingleton();
        }
        /// <summary>
        /// Attempts the login.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>true if login is successful false otherwise</returns>
        public bool AttemptLogin(string username, string password)
        {
            return this.authService.AttemptLogin(username, password);
        }

        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            return AuthServiceSingleton.LogoutUser();
        }
    }
}
