
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System_Information_Group_1.Controller;
using System_Information_Group_1.InputValidation;

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

        private void onLoginButtonClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            bool successfulLogin = this.controller.AttemptLogin(this.usernameTextBox.Text, this.passwordTextBox.Text);

            Console.WriteLine("LOGIN: " + successfulLogin);
            if (successfulLogin)
            {
                // Navigate to the next page or perform any other action upon successful login
                PatientWindow appointmentCreationWindow = new PatientWindow();
                appointmentCreationWindow.Show();
                
                //Close the login window
                Window parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow?.Close();
                }

            }
            else
            {
                System.Windows.MessageBox.Show("Login failed. Please check your username and password.");
            }
        }

        private void onUsernameTextChange(object sender, TextChangedEventArgs e)
        {
            String usernameText = this.usernameTextBox.Text;

            UsernameValidator usernameValidator = new UsernameValidator();

            ValidationResult validationResult = usernameValidator.Validate(usernameText, null);

            if (!validationResult.IsValid)
            {
                this.loginErrorMessage.Visibility = System.Windows.Visibility.Visible;
                this.loginErrorMessage.Text = validationResult.ErrorContent.ToString();
            }
            else
            {
                this.loginErrorMessage.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void onPasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            String passwordText = this.passwordTextBox.Text;

            PasswordValidator passwordValidator = new PasswordValidator();

            ValidationResult validationResult = passwordValidator.Validate(passwordText, null);

            if (!validationResult.IsValid)
            {
                this.loginErrorMessage.Visibility = System.Windows.Visibility.Visible;
                this.loginErrorMessage.Text = validationResult.ErrorContent.ToString();
            }
            else
            {
                this.loginErrorMessage.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
