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
    /// Initializes a new instance of the <see cref="NurseWindow"/> class.
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
                MessageBox.Show("Please enter at least one search criterion.");
                return;
            }

            var nurses = this.controller.SearchNurses(firstName, lastName, dateOfBirth);
            this.nurseDataGrid.ItemsSource = nurses;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Search Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void MakeNurseButton_OnClick(object sender, RoutedEventArgs e)
    {
        var nurseRegistrationDialog = new CreateNurseDialog();
        nurseRegistrationDialog.ShowDialog();
    }

    private void ViewNurseButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (this.nurseDataGrid.SelectedItem is Nurse selectedNurse)
        {
            var dialog = new NurseInfoDialog(selectedNurse);
            dialog.ShowDialog();

            this.nurseDataGrid.Items.Refresh();
        }
    }

    #endregion
}