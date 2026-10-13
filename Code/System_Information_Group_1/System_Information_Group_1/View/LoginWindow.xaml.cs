using System;
using System.Linq;
using System.Windows;


namespace System_Information_Group_1.View;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
    #region Constructors        


    /// <summary>
    /// Initializes a new instance of the <see cref="LoginWindow"/> class.
    /// </summary>
    public LoginWindow()
    {
        this.InitializeComponent();

    }


    #endregion

    private void onNurseRadioButtonClicked(object sender, RoutedEventArgs e)
    {
        this.LoginWindowFrame.Navigate(new NurseLoginPage());
    }

    private void onAdminRadioButtonClicked(object sender, RoutedEventArgs e)
    {
        
        this.LoginWindowFrame.Navigate(new AdminLoginPage());
    }


}