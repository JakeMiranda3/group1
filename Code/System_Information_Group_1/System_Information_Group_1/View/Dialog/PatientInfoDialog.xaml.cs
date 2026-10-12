using System;
using System.Windows;
using System_Information_Group_1.Controller;
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
    }

    #endregion

    #region Methods

    private void EditPatientButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.loadFields();
        this.setEditMode(true);
    }

    private void SaveChangesButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!this.dateOfBirthPicker.SelectedDate.HasValue)
            {
                throw new ArgumentException("Please select a date of birth.");
            }

            if (this.stateComboBox.SelectedItem is not string state)
            {
                throw new ArgumentException("Please select a state.");
            }

            var editedPerson = new Person(this.patient.PersonId, this.lastNameTextBox.Text, this.firstNameTextBox.Text,
                this.dateOfBirthPicker.SelectedDate.Value, this.phoneNumberTextBox.Text, this.addressTextBox.Text,
                this.zipTextBox.Text, this.cityTextBox.Text,
                state);

            var editedPatient = new Patient(this.patient.PersonId, this.patient.PatientId,
                this.isActiveCheckBox.IsChecked == true);

            editedPatient.Person = editedPerson;

            this.patientController.EditPatient(editedPatient);

            this.patient.Person = editedPerson;
            this.patient.IsActive = editedPatient.IsActive;

            DataContext = null;
            DataContext = this.patient;

            this.setEditMode(false);

            MessageBox.Show("Patient information updated successfully.",
                "Success", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Invalid Information",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Update Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelEditButton_OnClick(object sender, RoutedEventArgs e)
    {
        DataContext = null;
        DataContext = this.patient;

        this.setEditMode(false);
    }

    private void loadFields()
    {
        this.dateOfBirthPicker.SelectedDate = this.patient.Person.DateOfBirth;
        this.stateComboBox.SelectedItem = this.patient.Person.State;
    }

    private void setEditMode(bool editing)
    {
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

    #endregion
}