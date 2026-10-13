using System;
using System.Windows;
using System.Windows.Controls;
using System_Information_Group_1.Controller;

namespace System_Information_Group_1.View.Dialog;

/// <summary>
///     Interaction logic for registering a patient in the system.
/// </summary>
public partial class PatientRegistrationDialog : Window
{
    #region Fields

    private readonly PatientController patientController;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientRegistrationDialog"/> class.
    /// </summary>
    public PatientRegistrationDialog()
    {
        this.InitializeComponent();
        this.patientController = new PatientController();
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

            if (this.genderComboBox.SelectedItem is not ComboBoxItem selectedGender)
            {
                throw new ArgumentException("Please select a gender.");
            }

            var patient = this.patientController.RegisterPatient(this.lastNameTextBox.Text, this.firstNameTextBox.Text,
                this.dateOfBirthPicker.SelectedDate.Value, this.phoneNumberTextBox.Text, this.addressTextBox.Text,
                this.zipTextBox.Text, this.cityTextBox.Text,
                selectedState.Content.ToString(), selectedGender.Content.ToString());

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