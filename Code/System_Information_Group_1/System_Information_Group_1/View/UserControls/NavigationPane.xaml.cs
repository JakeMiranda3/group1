using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System_Information_Group_1.Constants;
using System_Information_Group_1.Controller;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View.UserControls
{

    public partial class NavigationPane : UserControl
    {
        private LoginController loginController;
        public NavigationPane()
        {
            this.InitializeComponent();
            Loaded += NavigationPane_Loaded;
            Unloaded += NavigationPane_Unloaded;
            this.loginController = new LoginController();


            // You can only navigate to the nurse window if you are an admin. 
            AuthServiceSingleton authService = new AuthServiceSingleton();
            AccountType role = authService.GetUserRole();

            if (role == AccountType.Administrator)
            {
                this.nurseNavigationButton.Visibility = Visibility.Visible;
                this.nurseNavigationButton.IsEnabled = true;
            }


        }

        private void NavigationPane_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) ?? Application.Current.MainWindow;
            if (window != null)
            {
                this.partPopup.PlacementTarget = window;
                UpdateSize(window);
                window.SizeChanged += Window_SizeChanged;
                window.LocationChanged += Window_SizeChanged;
            }
        }

        private void NavigationPane_Unloaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) ?? Application.Current.MainWindow;
            if (window != null)
            {
                window.SizeChanged -= Window_SizeChanged;
                window.LocationChanged -= Window_SizeChanged;
            }
        }

        private void Window_SizeChanged(object sender, EventArgs e)
        {
            UpdateSize(Window.GetWindow(this) ?? Application.Current.MainWindow);
        }

        private void UpdateSize(Window window)
        {
            if (window == null) return;
            // Make the pane match window height so it can span beyond its parent container.
            this.rootBorder.Height = window.ActualHeight - 37; //adjust for the title bar height
            this.partPopup.HorizontalOffset = 0;
            this.partPopup.VerticalOffset = 0;
        }


        public bool IsOpen
        {
            get;
            set;
        }



        public void Toggle()
        {
            this.Visibility = Visibility.Visible;
            this.IsOpen = true;
            NavigationPane pane = this;
            if (pane != null)
            {
                pane.partPopup.IsOpen = !pane.partPopup.IsOpen;
            }
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
                window.Close();
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
                window.Close();
            }
            else
            {
                this.Toggle();
            }
        }
    }
}
