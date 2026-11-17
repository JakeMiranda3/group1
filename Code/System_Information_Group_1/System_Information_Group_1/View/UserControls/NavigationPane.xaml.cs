using System;
using System.Windows;
using System.Windows.Controls;
using System_Information_Group_1.Authorization;
using System_Information_Group_1.Constants;
using System_Information_Group_1.Controller;

namespace System_Information_Group_1.View.UserControls
{
    /// <summary>
    /// The slide-out navigation pane used to move between windows and log out.
    /// </summary>
    public partial class NavigationPane : UserControl
    {
        private LoginController loginController;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationPane"/> class and shows the navigation buttons allowed for the current user's role.
        /// </summary>
        public NavigationPane()
        {
            this.InitializeComponent();
            Loaded += this.NavigationPane_Loaded;
            Unloaded += this.NavigationPane_Unloaded;
            this.loginController = new LoginController();


            // You can only navigate to the nurse window if you are an admin and hide the Patient Page
            AuthServiceSingleton authService = new AuthServiceSingleton();
            AccountType role = authService.GetUserRole();

            if (role == AccountType.Administrator)
            {
                this.nurseNavigationButton.Visibility = Visibility.Visible;
                this.nurseNavigationButton.IsEnabled = true;

                this.patientNavigationButton.Visibility = Visibility.Collapsed;
                this.patientNavigationButton.IsEnabled = false;
            }


        }

        private void NavigationPane_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) ?? Application.Current.MainWindow;
            if (window != null)
            {
                this.partPopup.PlacementTarget = window;
                this.updateSize(window);
                window.SizeChanged += this.Window_SizeChanged;
                window.LocationChanged += this.Window_SizeChanged;
            }
        }

        private void NavigationPane_Unloaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) ?? Application.Current.MainWindow;
            if (window != null)
            {
                window.SizeChanged -= this.Window_SizeChanged;
                window.LocationChanged -= this.Window_SizeChanged;
            }
        }

        private void Window_SizeChanged(object sender, EventArgs e)
        {
            this.updateSize(Window.GetWindow(this) ?? Application.Current.MainWindow);
        }

        private void updateSize(Window window)
        {
            if (window == null)
            {
                return;
            }

            // Make the pane match window height so it can span beyond its parent container.
            this.rootBorder.Height = window.ActualHeight - 37; //adjust for the title bar height
            this.partPopup.HorizontalOffset = 0;
            this.partPopup.VerticalOffset = 0;
        }


        /// <summary>
        /// Gets or sets a value indicating whether the navigation pane has been opened.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the navigation pane has been opened; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen
        {
            get;
            set;
        }

        /// <summary>
        /// Opens the navigation pane if it is closed, or closes it if it is open.
        /// </summary>
        public void Toggle()
        {
            Visibility = Visibility.Visible;
            this.IsOpen = true;
            NavigationPane pane = this;
            pane.partPopup.IsOpen = !pane.partPopup.IsOpen;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e) => this.Toggle();

        private void onLogoutClicked(object sender, RoutedEventArgs e)
        {
            this.loginController.Logout();

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            var parentWindow = Window.GetWindow(this) ?? Application.Current.MainWindow;

            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }

        private void onPatientsButtonClicked(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) ?? Application.Current.MainWindow;

            if (window is not PatientWindow) 
            {
                PatientWindow patientWindow = new PatientWindow();
                patientWindow.Show();
                if (window != null)
                {
                    window.Close();
                }
            }
            else
            {
                this.Toggle();
            }
        }

        private void onNurseButtonClicked(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) ?? Application.Current.MainWindow;

            if (window is not NurseWindow)
            {
                NurseWindow nurseWindow = new NurseWindow();
                nurseWindow.Show();
                if (window != null)
                {
                    window.Close();
                }
            }
            else
            {
                this.Toggle();
            }
        }
    }
}
