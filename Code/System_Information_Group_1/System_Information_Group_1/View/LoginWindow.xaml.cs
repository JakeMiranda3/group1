using System.Windows;


namespace System_Information_Group_1.View;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Constructors    
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    
    
    public MainWindow()
    {
        this.InitializeComponent();
        

    }


    #endregion

    private void onNurseRadioButtonClicked(object sender, RoutedEventArgs e)
    {
        this.mainWindowFrame.Navigate(new NurseLoginPage());
    }

    private void onAdminRadioButtonClicked(object sender, RoutedEventArgs e)
    {
        
        this.mainWindowFrame.Navigate(new AdminLoginPage());
    }


}