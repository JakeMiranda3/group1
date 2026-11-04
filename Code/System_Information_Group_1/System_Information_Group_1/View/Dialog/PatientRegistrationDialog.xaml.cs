using System;
using System.Windows;
using System.Windows.Controls;
using System_Information_Group_1.Controller;
using System_Information_Group_1.InputValidation;

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
    ///     Initializes a new instance of the <see cref="PatientRegistrationDialog" /> class.
    /// </summary>
    public PatientRegistrationDialog()
    {
        this.InitializeComponent();
        this.patientController = new PatientController();

        foreach (var input in new FrameworkElement[]
                 {
                     this.firstNameTextBox,
                     this.lastNameTextBox,
                     this.dateOfBirthPicker,
                     this.phoneNumberTextBox,
                     this.addressTextBox,
                     this.cityTextBox,
                     this.stateComboBox,
                     this.zipTextBox,
                     this.genderComboBox
                 })
        {
            input.LostFocus += this.Input_LostFocus;
        }
    }

    #endregion

    #region Methods

    private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!this.validateAllFields())
        {
            return;
        }

        try
        {
            var selectedState = (ComboBoxItem)this.stateComboBox.SelectedItem;
            var selectedGender = (ComboBoxItem)this.genderComboBox.SelectedItem;

            var patient = this.patientController.RegisterPatient(
                this.lastNameTextBox.Text,
                this.firstNameTextBox.Text,
                this.dateOfBirthPicker.SelectedDate!.Value,
                this.phoneNumberTextBox.Text,
                this.addressTextBox.Text,
                this.zipTextBox.Text,
                this.cityTextBox.Text,
                selectedState.Content.ToString()!,
                selectedGender.Content.ToString()!);

            MessageBox.Show(
                $"Patient registered successfully. Patient ID: {patient.PatientId}",
                "Registration Successful",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            DialogResult = true;
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

    private static bool setError(TextBlock errorLabel, string? error)
    {
        errorLabel.Text = error ?? "";

        return error == null;
    }

    private bool validateInput(object sender)
    {
        if (Equals(sender, this.firstNameTextBox))
        {
            return setError(
                this.firstNameError,
                PersonValidator.ValidateFirstName(this.firstNameTextBox.Text));
        }

        if (Equals(sender, this.lastNameTextBox))
        {
            return setError(
                this.lastNameError,
                PersonValidator.ValidateLastName(this.lastNameTextBox.Text));
        }

        if (Equals(sender, this.dateOfBirthPicker))
        {
            return setError(
                this.dateOfBirthError,
                PersonValidator.ValidateDateOfBirth(this.dateOfBirthPicker.SelectedDate));
        }

        if (Equals(sender, this.phoneNumberTextBox))
        {
            return setError(
                this.phoneNumberError,
                PersonValidator.ValidatePhoneNumber(this.phoneNumberTextBox.Text));
        }

        if (Equals(sender, this.addressTextBox))
        {
            return setError(
                this.addressError,
                PersonValidator.ValidateAddress(this.addressTextBox.Text));
        }

        if (Equals(sender, this.cityTextBox))
        {
            return setError(
                this.cityError,
                PersonValidator.ValidateCity(this.cityTextBox.Text));
        }

        if (Equals(sender, this.stateComboBox))
        {
            var state = (this.stateComboBox.SelectedItem as ComboBoxItem)?
                .Content?.ToString();

            return setError(
                this.stateError,
                PersonValidator.ValidateState(state));
        }

        if (Equals(sender, this.zipTextBox))
        {
            return setError(
                this.zipError,
                PersonValidator.ValidateZip(this.zipTextBox.Text));
        }

        if (Equals(sender, this.genderComboBox))
        {
            var gender = (this.genderComboBox.SelectedItem as ComboBoxItem)?
                .Content?.ToString();

            return setError(
                this.genderError,
                PersonValidator.ValidateGender(gender));
        }

        return true;
    }

    private void Input_LostFocus(object sender, RoutedEventArgs e)
    {
        this.validateInput(sender);
    }

    private bool validateAllFields()
    {
        return this.validateInput(this.firstNameTextBox)
               & this.validateInput(this.lastNameTextBox)
               & this.validateInput(this.dateOfBirthPicker)
               & this.validateInput(this.phoneNumberTextBox)
               & this.validateInput(this.addressTextBox)
               & this.validateInput(this.cityTextBox)
               & this.validateInput(this.stateComboBox)
               & this.validateInput(this.zipTextBox)
               & this.validateInput(this.genderComboBox);
    }

    #endregion
}