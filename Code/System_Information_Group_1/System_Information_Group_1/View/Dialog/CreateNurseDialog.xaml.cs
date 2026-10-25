using System;
using System.Windows;
using System_Information_Group_1.Controller;

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
    /// Initializes a new instance of the <see cref="CreateNurseDialog"/> class.
    /// </summary>
    public CreateNurseDialog()
    {
        this.nurseController = new NurseController();
        this.InitializeComponent();
    }

    #endregion

    #region Methods

    private void CreateButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var firstName = this.firstNameTextBox.Text;
            var lastName = this.lastNameTextBox.Text;
            var dateOfBirth = this.dateOfBirthPicker.SelectedDate;
            var phoneNumber = this.phoneNumberTextBox.Text;
            var address = this.addressTextBox.Text;
            var zip = this.zipTextBox.Text;
            var city = this.cityTextBox.Text;
            var state = this.stateTextBox.Text;
            var gender = this.genderTextBox.Text;

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
                gender);

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

    #endregion
}