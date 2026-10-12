using System;
using System.Windows;
using System.Windows.Controls;
using System_Information_Group_1.Controller;

namespace System_Information_Group_1.View.Dialog;

/// <summary>
///     Interaction logic for PatientRegistrationDialog.xaml
/// </summary>
public partial class PatientRegistrationDialog : Window
{
    #region Fields

    private readonly PatientController patientController;

    #endregion

    #region Constructors

    public PatientRegistrationDialog()
    {
        this.patientController = new PatientController();
        this.InitializeComponent();
    }

    #endregion

    #region Methods

    private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!this.dateOfBirthPicker.SelectedDate.HasValue)
            {
                throw new ArgumentException("Please select a date of birth.");
            }

            if (this.stateComboBox.SelectedItem is not ComboBoxItem selectedState)
            {
                throw new ArgumentException("Please select a state.");
            }

            var patient = this.patientController.RegisterPatient(this.lastNameTextBox.Text, this.firstNameTextBox.Text,
                this.dateOfBirthPicker.SelectedDate.Value, this.phoneNumberTextBox.Text, this.addressTextBox.Text,
                this.zipTextBox.Text, this.cityTextBox.Text,
                selectedState.Content.ToString());

            MessageBox.Show(
                $"Patient registered successfully. Patient ID: {patient.PatientId}",
                "Registration Successful",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            DialogResult = true;
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Invalid Patient Information",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Registration Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    #endregion
}