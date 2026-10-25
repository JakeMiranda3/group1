using System;
using System.Windows;
using System_Information_Group_1.Controller;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View.Dialog;

/// <summary>
///     Interaction logic for NurseInfoDialog.xaml
/// </summary>
public partial class NurseInfoDialog : Window
{
    #region Fields

    private readonly Nurse nurse;
    private readonly NurseController nurseController;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the NurseInfoDialog class.
    /// </summary>
    /// <param name="nurse">The nurse.</param>
    public NurseInfoDialog(Nurse nurse)
    {
        this.InitializeComponent();

        this.nurse = nurse ?? throw new ArgumentNullException(nameof(nurse));
        this.nurseController = new NurseController();
        DataContext = nurse;

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

        this.genderComboBox.ItemsSource =
            new[] { "Male", "Female", "Other" };
    }

    #endregion

    #region Methods

    private void EditButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.loadFields();
        this.setEditMode(true);
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (!this.dateOfBirthPicker.SelectedDate.HasValue)
            {
                throw new ArgumentException(
                    "Please select a date of birth.");
            }

            if (this.stateComboBox.SelectedItem is not string state)
            {
                throw new ArgumentException(
                    "Please select a state.");
            }

            if (this.genderComboBox.SelectedItem is not string gender)
            {
                throw new ArgumentException(
                    "Please select a gender.");
            }

            var editedPerson = new Person(
                this.nurse.PersonId,
                this.lastNameTextBox.Text,
                this.firstNameTextBox.Text,
                this.dateOfBirthPicker.SelectedDate.Value,
                this.phoneNumberTextBox.Text,
                this.addressTextBox.Text,
                this.zipTextBox.Text,
                this.cityTextBox.Text,
                state,
                gender);

            var editedNurse = new Nurse(
                this.nurse.PersonId,
                this.nurse.NurseId);

            editedNurse.Person = editedPerson;

            this.nurseController.EditNurse(editedNurse);

            this.nurse.Person = editedPerson;

            DataContext = null;
            DataContext = this.nurse;

            this.setEditMode(false);

            MessageBox.Show(
                "Nurse information updated successfully.",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Invalid Information",
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
        DataContext = null;
        DataContext = this.nurse;

        this.setEditMode(false);
    }

    private void loadFields()
    {
        this.dateOfBirthPicker.SelectedDate =
            this.nurse.Person.DateOfBirth;

        this.stateComboBox.SelectedItem =
            this.nurse.Person.State;

        this.genderComboBox.SelectedItem =
            this.nurse.Person.Gender;
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

        this.genderTextBlock.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

        this.genderComboBox.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.editButton.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

        this.saveButton.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.cancelEditButton.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.closeButton.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    #endregion
}