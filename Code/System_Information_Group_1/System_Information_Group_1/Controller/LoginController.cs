using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Information_Group_1.Controller
{
    
    public class LoginController
    {
        private AuthServiceSingleton authService;

        public LoginController()
        {
            this.authService = new AuthServiceSingleton();
        }
        public bool AttemptLogin(string username, string password)
        {
            return this.authService.AttemptLogin(username, password);
        }
    }
}
