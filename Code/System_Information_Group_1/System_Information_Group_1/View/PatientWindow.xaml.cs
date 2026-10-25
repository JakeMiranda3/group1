using System;
using System.Data;
using System.Linq;
using System.Windows;
using System_Information_Group_1.Controller;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;
using System_Information_Group_1.View.Dialog;

namespace System_Information_Group_1.View;

/// <summary>
/// Patient window for searching, viewing, editing, and registering patients.
/// </summary>
/// <seealso cref="System.Windows.Window" />
/// <seealso cref="System.Windows.Markup.IComponentConnector" />
public partial class PatientWindow : Window
{
    #region Fields

    private readonly PatientController patientController;
    private readonly AppointmentDal appointmentDal;
    private readonly NurseDal nurseDal;
    private readonly VisitDal visitDal;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientWindow"/> class.
    /// </summary>
    public PatientWindow()
    {
        this.patientController = new PatientController();
        this.appointmentDal = new AppointmentDal();
        this.nurseDal = new NurseDal();
        this.visitDal = new VisitDal();
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

    private void ViewPatientButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (this.patientDataGrid.SelectedItem is Patient selectedPatient)
        {
            var dialog = new PatientInfoDialog(selectedPatient);
            dialog.ShowDialog();

            this.patientDataGrid.Items.Refresh();
        }
    }

    #endregion

    private void CreateAppointmentButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (this.patientDataGrid.SelectedItem is Patient selectedPatient)
        {
            var patientId = selectedPatient.PatientId;
            AppointmentCreationWindow appointmentCreationWindow = new AppointmentCreationWindow(patientId);
            appointmentCreationWindow.Show();


            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow?.Close();
            }
        }
    }

    private void EnterVisitInformationButton_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (this.patientDataGrid.SelectedItem is not Patient selectedPatient)
            {
                MessageBox.Show("Please select a patient first.", "Missing Information", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var appointments = this.appointmentDal.GetAppointmentsContainingDoctorAndPatientByPatientId(selectedPatient.PatientId);
            if (appointments.Count == 0)
            {
                MessageBox.Show("No appointments found for the selected patient.", "Missing Information", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var editableAppointments = appointments
                .Where(appointment => !this.visitDal.IsVisitFinalized(appointment.AppointmentId))
                .ToList();

            if (editableAppointments.Count == 0)
            {
                MessageBox.Show("All visits for this patient are finalized and read-only.", "No Editable Visit", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var dialog = new AppointmentSelectionDialog(editableAppointments)
            {
                Owner = this
            };

            if (dialog.ShowDialog() != true || !dialog.SelectedAppointmentId.HasValue)
            {
                return;
            }

            var personIdOfLoggedInUser = AuthServiceSingleton.GetAccount().PersonId;
            var nurseId = this.nurseDal.GetNurseWithPersonId(personIdOfLoggedInUser).NurseId;

            var visitDisplay = new VisitDisplay(dialog.SelectedAppointmentId.Value, nurseId);
            var routineCheckupWindow = new RoutineCheckupWindow(visitDisplay)
            {
                Owner = this
            };

            routineCheckupWindow.ShowDialog();
        }
        catch (AccessViolationException)
        {
            MessageBox.Show("Only a nurse account can enter visit information.", "Access Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Unable to open visit information: {ex.Message}", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
