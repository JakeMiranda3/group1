using System;
using System.Windows;
using System_Information_Group_1.Controller;
using System_Information_Group_1.Model;
using System_Information_Group_1.View.Dialog;

namespace System_Information_Group_1.View;

/// <summary>
///     Interaction logic for NurseWindow.xaml
/// </summary>
public partial class NurseWindow : Window
{
    #region Fields

    private readonly NurseController controller;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="NurseWindow" /> class.
    /// </summary>
    public NurseWindow()
    {
        this.controller = new NurseController();
        this.InitializeComponent();
    }

    #endregion

    #region Methods

    private void SearchButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var firstName = this.firstNameTextBox.Text;
            var lastName = this.lastNameTextBox.Text;
            var dateOfBirth = this.dateOfBirthPicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(firstName) &&
                string.IsNullOrWhiteSpace(lastName) &&
                !dateOfBirth.HasValue)
            {
                MessageBox.Show(this, "Please enter at least one search criterion.");
                return;
            }

            var nurses = this.controller.SearchNurses(firstName, lastName, dateOfBirth);
            this.nurseDataGrid.ItemsSource = nurses;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Search Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void MakeNurseButton_OnClick(object sender, RoutedEventArgs e)
    {
        var nurseRegistrationDialog = new CreateNurseDialog
        {
            Owner = this,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        nurseRegistrationDialog.ShowDialog();
    }

    private void ViewNurseButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (this.nurseDataGrid.SelectedItem is Nurse selectedNurse)
        {
            var dialog = new NurseInfoDialog(selectedNurse)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            dialog.ShowDialog();

            this.nurseDataGrid.Items.Refresh();
        }
    }

    private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.firstNameTextBox.Clear();
        this.lastNameTextBox.Clear();
        this.dateOfBirthPicker.SelectedDate = null;

        this.nurseDataGrid.ItemsSource = null;
    }

    #endregion

    private void viewStatisticsButton_Click(object sender, RoutedEventArgs e)
    {

        var statisticWindow = new LabStatisticsWindow();
        statisticWindow.Show();

        var parentWindow = GetWindow(this);
        if (parentWindow != null)
        {
            parentWindow.Close();
        }
    }
}