using System;
using System.Windows;
using System.Windows.Controls;
using System_Information_Group_1.Controller;
using System_Information_Group_1.InputValidation;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View.Dialog;

/// <summary>
///     Interaction logic for PatientInfoDialog.xaml
/// </summary>
public partial class PatientInfoDialog : Window
{
    #region Fields

    private readonly Patient patient;
    private readonly PatientController patientController;
    private bool isEditing;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="PatientInfoDialog" /> class.
    /// </summary>
    /// <param name="patient">The patient.</param>
    public PatientInfoDialog(Patient patient)
    {
        this.InitializeComponent();

        this.patient = patient;
        this.patientController = new PatientController();
        DataContext = patient;

        this.stateComboBox.ItemsSource = new[]
        {
            "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE",
            "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS",
            "KY", "LA", "ME", "MD", "MA", "MI", "MN", "MS",
            "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY",
            "NC", "ND", "OH", "OK", "OR", "PA", "RI", "SC",
            "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV",
            "WI", "WY"
        };
        this.genderComboBox.ItemsSource = new[] { "Male", "Female", "Other" };

        this.registerTextBoxValidation(this.firstNameTextBox, this.firstNameError);
        this.registerTextBoxValidation(this.lastNameTextBox, this.lastNameError);
        this.registerTextBoxValidation(this.phoneNumberTextBox, this.phoneNumberError);
        this.registerTextBoxValidation(this.addressTextBox, this.addressError);
        this.registerTextBoxValidation(this.cityTextBox, this.cityError);
        this.registerTextBoxValidation(this.zipTextBox, this.zipError);

        this.dateOfBirthPicker.LostFocus += this.Input_LostFocus;
        this.stateComboBox.LostFocus += this.Input_LostFocus;
        this.genderComboBox.LostFocus += this.Input_LostFocus;

        this.dateOfBirthPicker.SelectedDateChanged += (_, _) =>
            this.revalidateIfErrorVisible(this.dateOfBirthPicker, this.dateOfBirthError);

        this.stateComboBox.SelectionChanged += (_, _) =>
            this.revalidateIfErrorVisible(this.stateComboBox, this.stateError);

        this.genderComboBox.SelectionChanged += (_, _) =>
            this.revalidateIfErrorVisible(this.genderComboBox, this.genderError);
    }

    #endregion

    #region Methods

    private void EditPatientButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.clearErrors();
        this.loadFields();
        this.setEditMode(true);
    }

    private void SaveChangesButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!this.validateAllFields())
        {
            return;
        }

        try
        {
            var state = (string)this.stateComboBox.SelectedItem;
            var gender = (string)this.genderComboBox.SelectedItem;

            var editedPerson = new Person(
                this.patient.PersonId,
                this.lastNameTextBox.Text,
                this.firstNameTextBox.Text,
                this.dateOfBirthPicker.SelectedDate!.Value,
                this.phoneNumberTextBox.Text,
                this.addressTextBox.Text,
                this.zipTextBox.Text,
                this.cityTextBox.Text,
                state,
                gender);

            var editedPatient = new Patient(
                this.patient.PersonId,
                this.patient.PatientId,
                this.isActiveCheckBox.IsChecked == true);

            editedPatient.Person = editedPerson;

            this.patientController.EditPatient(editedPatient);

            this.patient.Person = editedPerson;
            this.patient.IsActive = editedPatient.IsActive;

            this.setEditMode(false);
            this.clearErrors();

            DataContext = null;
            DataContext = this.patient;

            MessageBox.Show(
                "Patient information updated successfully.",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Unable to Save Patient",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Update Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void CancelEditButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.setEditMode(false);
        this.clearErrors();

        DataContext = null;
        DataContext = this.patient;
    }

    private void loadFields()
    {
        this.dateOfBirthPicker.SelectedDate = this.patient.Person.DateOfBirth;
        this.stateComboBox.SelectedItem = this.patient.Person.State;
        this.genderComboBox.SelectedItem = this.patient.Person.Gender;
    }

    private void setEditMode(bool editing)
    {
        this.isEditing = editing;

        this.firstNameTextBox.IsReadOnly = !editing;
        this.lastNameTextBox.IsReadOnly = !editing;
        this.phoneNumberTextBox.IsReadOnly = !editing;
        this.addressTextBox.IsReadOnly = !editing;
        this.cityTextBox.IsReadOnly = !editing;
        this.zipTextBox.IsReadOnly = !editing;

        this.dateOfBirthTextBlock.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

        this.dateOfBirthPicker.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.stateTextBlock.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

        this.stateComboBox.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.genderTextBlock.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

        this.genderComboBox.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.isActiveCheckBox.IsHitTestVisible = editing;
        this.isActiveCheckBox.Focusable = editing;

        this.editPatientButton.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

        this.saveChangesButton.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.cancelEditButton.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.closeButton.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    private void registerTextBoxValidation(TextBox textBox, TextBlock errorLabel)
    {
        textBox.LostFocus += this.Input_LostFocus;

        textBox.TextChanged += (_, _) =>
            this.revalidateIfErrorVisible(textBox, errorLabel);
    }

    private void revalidateIfErrorVisible(object input, TextBlock errorLabel)
    {
        if (this.isEditing && errorLabel.Visibility == Visibility.Visible)
        {
            this.validateInput(input);
        }
    }

    private static bool setError(TextBlock errorLabel, string? error)
    {
        errorLabel.Text = error ?? "";

        errorLabel.Visibility = error == null
            ? Visibility.Collapsed
            : Visibility.Visible;

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
            return setError(
                this.stateError,
                PersonValidator.ValidateState(
                    this.stateComboBox.SelectedItem as string));
        }

        if (Equals(sender, this.zipTextBox))
        {
            return setError(
                this.zipError,
                PersonValidator.ValidateZip(this.zipTextBox.Text));
        }

        if (Equals(sender, this.genderComboBox))
        {
            return setError(
                this.genderError,
                PersonValidator.ValidateGender(
                    this.genderComboBox.SelectedItem as string));
        }

        return true;
    }

    private void Input_LostFocus(object sender, RoutedEventArgs e)
    {
        if (this.isEditing)
        {
            this.validateInput(sender);
        }
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

    private void clearErrors()
    {
        foreach (var errorLabel in new[]
                 {
                     this.firstNameError,
                     this.lastNameError,
                     this.dateOfBirthError,
                     this.phoneNumberError,
                     this.addressError,
                     this.cityError,
                     this.stateError,
                     this.genderError,
                     this.zipError
                 })
        {
            errorLabel.Text = "";
            errorLabel.Visibility = Visibility.Collapsed;
        }
    }

    #endregion
}