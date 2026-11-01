
using System;
using System.Globalization;
using System.Windows;
using System_Information_Group_1.Controller;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View.Dialog;

/// <summary>
///     Interaction logic for AppointmentInfoDialog.xaml
/// </summary>
public partial class AppointmentInfoDialog : Window
{
    #region Fields

    private readonly Appointment appointment;
    private readonly AppointmentController appointmentController;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the
    ///     <see cref="AppointmentInfoDialog"/> class.
    /// </summary>
    /// <param name="appointment">The appointment.</param>
    public AppointmentInfoDialog(Appointment appointment)
    {
        this.InitializeComponent();

        this.appointment = appointment;
        this.appointmentController = new AppointmentController();

        DataContext = appointment;
    }

    #endregion

    #region Methods

    private void EditAppointmentButton_OnClick(
        object sender, RoutedEventArgs e)
    {
        this.loadFields();
        this.setEditMode(true);
    }

    private void SaveChangesButton_OnClick(
        object sender, RoutedEventArgs e)
    {
        try
        {
            if (!this.appointmentDatePicker.SelectedDate.HasValue)
            {
                throw new ArgumentException(
                    "Please select an appointment date.");
            }

            if (string.IsNullOrWhiteSpace(this.reasonTextBox.Text))
            {
                throw new ArgumentException(
                    "Please enter an appointment reason.");
            }

            var timeFormats = new[]
            {
                "h:mm tt",
                "hh:mm tt",
                "H:mm",
                "HH:mm"
            };

            if (!DateTime.TryParseExact(
                    this.appointmentTimeTextBox.Text.Trim(),
                    timeFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var selectedTime))
            {
                throw new ArgumentException(
                    "Please enter a valid appointment time (HH:mm).");
            }

            var appointmentDate =
                this.appointmentDatePicker.SelectedDate.Value.Date
                    .Add(selectedTime.TimeOfDay);

            var editedAppointment = new Appointment(
                this.appointment.AppointmentId,
                this.appointment.DoctorId,
                this.appointment.PatientId,
                this.reasonTextBox.Text.Trim(),
                appointmentDate);

            editedAppointment.Doctor = this.appointment.Doctor;
            editedAppointment.Patient = this.appointment.Patient;

            this.appointmentController.EditAppointment(editedAppointment);

            this.appointment.Reason = editedAppointment.Reason;
            this.appointment.Date = editedAppointment.Date;

            DataContext = null;
            DataContext = this.appointment;

            this.setEditMode(false);

            MessageBox.Show(
                "Appointment information updated successfully.",
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

    private void CancelEditButton_OnClick(
        object sender, RoutedEventArgs e)
    {
        DataContext = null;
        DataContext = this.appointment;

        this.loadFields();
        this.setEditMode(false);
    }

    private void loadFields()
    {
        this.reasonTextBox.Text = this.appointment.Reason;

        this.appointmentDatePicker.SelectedDate =
            this.appointment.Date.Date;

        this.appointmentTimeTextBox.Text =
            this.appointment.Date.ToString("HH:mm");
    }

    private void setEditMode(bool editing)
    {
        this.reasonTextBox.IsReadOnly = !editing;

        this.appointmentDateTextBlock.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

        this.appointmentDatePicker.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.appointmentTimeTextBlock.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

        this.appointmentTimeTextBox.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.editAppointmentButton.Visibility = editing
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
