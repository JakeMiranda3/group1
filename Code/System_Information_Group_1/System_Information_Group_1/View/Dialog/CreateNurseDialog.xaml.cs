using System;
using System.Windows;
using System.Windows.Controls;
using System_Information_Group_1.Controller;
using System_Information_Group_1.InputValidation;

namespace System_Information_Group_1.View.Dialog;

/// <summary>
///     Interaction logic for CreateNurseDialog.xaml
/// </summary>
public partial class CreateNurseDialog : Window
{
    #region Fields

    private readonly NurseController nurseController;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateNurseDialog" /> class.
    /// </summary>
    public CreateNurseDialog()
    {
        this.nurseController = new NurseController();
        this.InitializeComponent();

        foreach (var input in new FrameworkElement[]
                 {
                     this.firstNameTextBox,
                     this.lastNameTextBox,
                     this.dateOfBirthPicker,
                     this.phoneNumberTextBox,
                     this.addressTextBox,
                     this.cityTextBox,
                     this.stateTextBox,
                     this.zipTextBox,
                     this.genderTextBox
                 })
        {
            input.LostFocus += this.Input_LostFocus;
        }
    }

    #endregion

    #region Methods

    private void CreateButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!this.validateAllFields())
        {
            return;
        }

        try
        {
            var selectedState = (ComboBoxItem)this.stateTextBox.SelectedItem;
            var selectedGender = (ComboBoxItem)this.genderTextBox.SelectedItem;

            this.nurseController.RegisterNurse(
                this.lastNameTextBox.Text,
                this.firstNameTextBox.Text,
                this.dateOfBirthPicker.SelectedDate!.Value,
                this.phoneNumberTextBox.Text,
                this.addressTextBox.Text,
                this.zipTextBox.Text,
                this.cityTextBox.Text,
                selectedState.Content.ToString()!,
                selectedGender.Content.ToString()!);
            var firstName = this.firstNameTextBox.Text;
            var lastName = this.lastNameTextBox.Text;
            var dateOfBirth = this.dateOfBirthPicker.SelectedDate;
            var phoneNumber = this.phoneNumberTextBox.Text;
            var address = this.addressTextBox.Text;
            var zip = this.zipTextBox.Text;
            var city = this.cityTextBox.Text;
            var state = this.stateTextBox.Text;
            var gender = this.genderTextBox.Text;
            var username = this.usernameTextBox.Text;
            var password = this.passwordTextBox.Password;

            if (!dateOfBirth.HasValue)
            {
                MessageBox.Show("Please select a date of birth.");
                return;
            }

            this.nurseController.RegisterNurse(
                lastName,
                firstName,
                dateOfBirth.Value,
                phoneNumber,
                address,
                zip,
                city,
                state,
                gender,
                username,
                password);

            MessageBox.Show(
                "Nurse successfully registered.",
                "Register Nurse",
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

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
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

        if (Equals(sender, this.stateTextBox))
        {
            var state = (this.stateTextBox.SelectedItem as ComboBoxItem)?
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

        if (Equals(sender, this.genderTextBox))
        {
            var gender = (this.genderTextBox.SelectedItem as ComboBoxItem)?
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
               & this.validateInput(this.stateTextBox)
               & this.validateInput(this.zipTextBox)
               & this.validateInput(this.genderTextBox);
    }

    #endregion
}