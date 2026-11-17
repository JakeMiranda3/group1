using System;
using System.Windows;
using System.Windows.Controls;
using System_Information_Group_1.Controller.UserControl;

namespace System_Information_Group_1.View.UserControls
{
    /// <summary>
    /// Interaction logic for TopAppBar.xaml
    /// </summary>
    public partial class TopAppBar : UserControl
    {
        private TopBarController controller;
        private const string DefaultUsernameAndRole = "Unknown User";
        /// <summary>
        /// Occurs when the hamburger menu is clicked.
        /// </summary>

        /// <summary>
        /// Gets or sets the bar header.
        /// </summary>
        /// <value>
        /// The bar header.
        /// </value>
        public string BarHeader
        {
            get => (string)GetValue(BarHeaderProperty);
            set => SetValue(BarHeaderProperty, value);
        }


        /// <summary>
        /// The bar header property
        /// </summary>
        public static readonly DependencyProperty BarHeaderProperty =
            DependencyProperty.Register(
                name: nameof(BarHeader),
                propertyType: typeof(string),
                ownerType: typeof(TopAppBar),
                new PropertyMetadata("Default Title")
            );

        /// <summary>
        /// Initializes a new instance of the <see cref="TopAppBar"/> class.
        /// </summary>
        public TopAppBar()
        {
            this.InitializeComponent();
            this.controller = new TopBarController();
            this.populateComponents();
        }

        private void populateComponents()
        {
            try
            {
                this.roleAndUsernameTextblock.Text = this.controller.GetAccountTypeAndUsernameString();
            }
            catch (Exception)
            {
                this.roleAndUsernameTextblock.Text = DefaultUsernameAndRole;
            }
        }

        private void onHamburgerButtonClicked(object sender, RoutedEventArgs e)
        {
            this.navigationPane.Toggle();
        }
    }
}
