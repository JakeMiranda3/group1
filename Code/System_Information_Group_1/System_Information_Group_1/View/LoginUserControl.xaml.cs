
using System;
using System.Windows.Controls;
using System_Information_Group_1.Controller;

namespace System_Information_Group_1.View
{
    /// <summary>
    /// Interaction logic for LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        private LoginController controller;
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginUserControl"/> class.
        /// </summary>
        public LoginUserControl()
        {
            this.InitializeComponent();
            this.controller = new LoginController();
        }

        private void onLoginButtonClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            bool successfulLogin = this.controller.AttemptLogin(this.usernameTextBox.Text, this.passwordTextBox.Text);

            if (successfulLogin)
            {
                // Navigate to the next page or perform any other action upon successful login
                System.Windows.MessageBox.Show("Login successful!");
            }
            else
            {
                // Show an error message or handle failed login attempt
                System.Windows.MessageBox.Show("Login failed. Please check your username and password.");
            }
        }
    }
}
