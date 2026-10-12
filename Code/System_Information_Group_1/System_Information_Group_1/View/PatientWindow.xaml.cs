using System;
using System.Windows;
using System_Information_Group_1.Controller;
using System_Information_Group_1.Model;
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

            var patients = this.patientController.SearchPatients(
                firstName, lastName, dateOfBirth);

            this.patientDataGrid.ItemsSource = patients;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Search Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void MakePatientButton_OnClick(object sender, RoutedEventArgs e)
    {
        var patientRegistrationDialog = new PatientRegistrationDialog();
        patientRegistrationDialog.ShowDialog();
    }

    #endregion

    private void ViewPatientButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (this.patientDataGrid.SelectedItem is Patient selectedPatient)
        {
            var dialog = new PatientInfoDialog(selectedPatient);
            dialog.ShowDialog();

            this.patientDataGrid.Items.Refresh();
        }
    }
}