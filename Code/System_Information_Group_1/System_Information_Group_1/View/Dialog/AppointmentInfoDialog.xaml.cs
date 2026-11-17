using System;
using System.Collections.Generic;
using System.Linq;
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
    private readonly AppointmentEditController appointmentEditController;

    private readonly IList<Doctor> doctors;
    #endregion

    #region Constructors

    /// <summary>
    /// Initializes the appointment info dialog.
    /// </summary>
    /// <param name="appointment">The appointment to display.</param>
    /// <param name="canDelete">if set to <c>true</c> the delete appointment button is shown.</param>
    /// <param name="doctors">The doctors the appointment can be assigned to.</param>
    public AppointmentInfoDialog(Appointment appointment, bool canDelete, ICollection<Doctor> doctors) 
    {
        this.InitializeComponent();
        this.populateTimeSlots();
        this.appointment = appointment;
        this.appointmentEditController = new AppointmentEditController();
        this.doctors = doctors.ToList();

        DataContext = appointment;

        this.doctorComboBox.ItemsSource = this.doctors;
        
        this.deleteAppointmentButton.Visibility = canDelete ? Visibility.Visible : Visibility.Collapsed;
        this.deleteAppointmentButton.IsEnabled = canDelete;

        this.appointmentTimeComboBox.ItemStringFormat = "{0:hh\\:mm}";
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
            if (this.appointmentTimeComboBox.SelectedItem is not TimeSpan selectedTime)
            {
                throw new ArgumentException(
                    "Please select an appointment time.");
            }

            var appointmentDate =
                this.appointmentDatePicker.SelectedDate.Value.Date
                    .Add(selectedTime);

            if (this.doctorComboBox.SelectedItem is not Doctor selectedDoctor)
            {
                throw new ArgumentException("Please select a doctor.");
            }

            var editedAppointment = new Appointment(
                this.appointment.AppointmentId,
                selectedDoctor.DoctorId,
                this.appointment.PatientId,
                this.reasonTextBox.Text.Trim(),
                appointmentDate);

            editedAppointment.Doctor = selectedDoctor;
            editedAppointment.Patient = this.appointment.Patient;

            this.appointmentEditController.EditAppointment(editedAppointment);

            this.appointment.DoctorId = editedAppointment.DoctorId;
            this.appointment.Doctor = editedAppointment.Doctor;
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


        this.doctorComboBox.SelectedItem =
            this.doctors.FirstOrDefault(doctor => doctor.DoctorId == this.appointment.DoctorId);

        var appointmentTime = this.appointment.Date.TimeOfDay;

        if (!this.appointmentTimeComboBox.Items.Contains(appointmentTime))
        {
            this.appointmentTimeComboBox.Items.Add(appointmentTime);
        }

        this.appointmentTimeComboBox.SelectedItem = appointmentTime;
    }

    private void setEditMode(bool editing)
    {
        this.reasonTextBox.IsReadOnly = !editing;


        this.appointmentTimeComboBox.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.appointmentDatePicker.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;

        this.appointmentTimeTextBlock.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

        this.appointmentDateTextBlock.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

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

        this.doctorNameTextBlock.Visibility = editing
            ? Visibility.Collapsed
            : Visibility.Visible;

        this.doctorComboBox.Visibility = editing
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    #endregion

    private void deleteAppointmentButton_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm Deletion",
            MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        try
        {
            this.appointmentEditController.DeleteAppointment(this.appointment.AppointmentId);
            MessageBox.Show("Appointment deleted!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void populateTimeSlots()
    {
        this.appointmentTimeComboBox.Items.Clear();

        var time = TimeSpan.FromHours(8);
        var endTime = TimeSpan.FromHours(17);

        while (time <= endTime)
        {
            this.appointmentTimeComboBox.Items.Add(time);

            time = time.Add(TimeSpan.FromMinutes(15));
        }
    }
}
