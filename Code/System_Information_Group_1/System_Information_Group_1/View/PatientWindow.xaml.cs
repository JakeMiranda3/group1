using System;
using System.Windows;
using System_Information_Group_1.Controller;
using System_Information_Group_1.View.Dialog;

namespace System_Information_Group_1.View;

public partial class PatientWindow : Window
{
    #region Fields

    private readonly PatientController patientController;

    #endregion

    #region Constructors

    public PatientWindow()
    {
        this.patientController = new PatientController();
        this.InitializeComponent();
    }

    #endregion

    #region Methods

    private void SearchButton_OnClick(object sender, RoutedEventArgs e)
    {

        try
        {
            var firstName = this.FirstNameTextBox.Text;
            var lastName = this.LastNameTextBox.Text;
            var dateOfBirth = this.DateOfBirthPicker.SelectedDate;
            if (string.IsNullOrWhiteSpace(firstName) &&
                string.IsNullOrWhiteSpace(lastName) &&
                !dateOfBirth.HasValue)
            {
                MessageBox.Show("Please enter at least one search criterion.");
                return;
            }

            var patients = this.patientController.SearchPatients(
                firstName, lastName, dateOfBirth);

            this.PatientDataGrid.ItemsSource = patients;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Search Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    #endregion

    private void MakePatientButton_OnClick(object sender, RoutedEventArgs e)
    {
        var patientRegistrationDialog = new PatientRegistrationDialog();
        patientRegistrationDialog.ShowDialog();
    }
}