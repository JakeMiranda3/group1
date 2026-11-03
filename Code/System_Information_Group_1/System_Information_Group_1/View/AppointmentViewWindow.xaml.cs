using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System_Information_Group_1.Controller;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;
using System_Information_Group_1.View.Dialog;
using System_Information_Group_1.View.Dialogs;
using System_Information_Group_1.View.UserControls;

namespace System_Information_Group_1.View
{
    /// <summary>
    /// Interaction logic for AppointmentViewWindow.xaml
    /// </summary>
    public partial class AppointmentViewWindow : Window
    {
        private const int MaxReasonLength = 100;
        private readonly ViewAppointmentController controller;

        /// <summary>
        /// Gets or sets the doctors.
        /// </summary>
        /// <value>
        /// The doctors.
        /// </value>
        public ObservableCollection<Doctor> Doctors { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentViewWindow"/> class.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        public AppointmentViewWindow(int patientId)
        {
            this.InitializeComponent();
            this.controller = new ViewAppointmentController(patientId);
            DataContext = this;
            this.regenerateDoctorComboBox();
        }


        private void regenerateDoctorComboBox()
        {
            this.Doctors = [..this.controller.GetAvailableDoctors()];
        }

        private void doctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.refreshSchedules();

        }

        private void appointmentTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.refreshSchedules();
        }


        private void createAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.clearError();

            if (!this.scheduleControl.IsSelectedCardAvailable())
            {
                this.showError("Please select an appointment that is available.");
                return;
            }

            if (this.scheduleControl.scheduleListBox.SelectedItem is not ListBoxItem item ||
                item.Content is not TimeCard selectedCard)
            {
                this.showError("Please select a time slot to create an appointment.");
                return;
            }

            var selectedDate = selectedCard.scheduledTime;
            if (this.doctorComboBox.SelectedItem is not Doctor currentDoctor)
            {
                this.showError("Please first select a doctor.");
                return;
            }

            this.createAppointment(selectedDate, currentDoctor);
        }

        private void createAppointment(DateTime selectedDate, Doctor currentDoctor)
        {
            var reasonDialog = new ReasonDialog
            {
                Owner = this
            };

            if (reasonDialog.ShowDialog() != true)
            {
                return;
            }

            var reason = reasonDialog.Reason;

            var wasAdded = this.controller.StoreAppointment(
                selectedDate,
                currentDoctor.DoctorId,
                this.controller.SelectedPatientId,
                reason);

            if (!wasAdded)
            {
                this.showError("Failed to book the appointment. Please try again.");
                return;
            }

            this.refreshSchedules();

            MessageBox.Show(
                "Appointment booked",
                "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void refreshSchedules()
        {
            this.clearError();
            this.scheduleControl.clearScheduleSlots();

            if (this.filterCheckbox.IsChecked == true)
            {
                this.scheduleControl.AddCardsFromAppointments(this.controller.GetPatientAppointments());
            }
            else
            {
                this.refreshSchedulesWithSelectedDoctor();
            }
        }

        private void refreshSchedulesWithSelectedDoctor()
        {
            var date = this.appointmentTimePicker.SelectedDate;
            if (date != null && this.doctorComboBox.SelectedItem is Doctor doctor)
            {
                this.scheduleControl.AddCardsWithScheduledAndOpenCards(this.controller.GetDoctorsAppointments(doctor),
                    date.Value,
                    this.controller.GetDoctorsFullName(doctor));
            }
        }

        

        private void exitAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            this.clearError();
            var patientWindow = new PatientWindow();
            var parentWindow = Window.GetWindow(this);
            patientWindow.Show();
            if (parentWindow != null)
            {
                parentWindow?.Close();
            }
        }

        private void onVisitClicked()
        {
            this.clearError();

            if (this.scheduleControl.SelectedAppointmentId is not int appointmentId)
            {
                this.showError("Please select a scheduled appointment.");
                return;
            }

            try
            {
                var visit = this.controller.GetVisitForAppointment(appointmentId);

                var routineCheckupWindow = new RoutineCheckupWindow(visit)
                {
                    Owner = this
                };

                this.Hide();

                routineCheckupWindow.ShowDialog();

                this.Show();
                this.refreshSchedules();
                this.Activate();
            }
            catch (Exception ex)
            {
                this.showError($"Unable to open visit information: {ex.Message}");
            }

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.doctorComboBox.IsEnabled = false;
            this.appointmentTimePicker.IsEnabled = false;
            this.createAppointmentButton.IsEnabled = false;
            this.refreshSchedules();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.doctorComboBox.IsEnabled = true;
            this.appointmentTimePicker.IsEnabled = true;
            this.createAppointmentButton.IsEnabled = true;
            this.refreshSchedules();
        }

        private void editAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.scheduleControl.scheduleListBox.SelectedItem is not ListBoxItem item ||
                    item.Content is not TimeCard selectedCard)
                {
                    MessageBox.Show("Please select an appointment first.",
                        "Missing Information", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (selectedCard.State == TimeCardState.Available)
                {
                    MessageBox.Show("Schedule an appointment in this slot first.",
                        "Invalid Operation", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (selectedCard.State == TimeCardState.Finalized)
                {
                    MessageBox.Show("Appointment has already been finalized.",
                        "Invalid Operation", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (selectedCard.scheduledTime <= this.controller.GetCurrentTimeAsEastern())
                {
                    MessageBox.Show("Appointment time has passed and cannot be edited.",
                        "Invalid Operation", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var selectedDate = selectedCard.scheduledTime;

                var appointmentId = this.controller.GetAppointmentId(
                    this.controller.SelectedPatientId, selectedDate);

                var selectedAppointment = this.controller.GetPatientAppointments()
                    .FirstOrDefault(appointment =>
                        appointment.AppointmentId == appointmentId);

                if (selectedAppointment == null)
                {
                    MessageBox.Show("No appointment found for the selected time.",
                        "Missing Information", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var dialog = new AppointmentInfoDialog(selectedAppointment)
                {
                    Owner = this
                };

                dialog.ShowDialog();
                this.refreshSchedules();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open appointment information: {ex.Message}",
                    "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void showError(string message)
        {
            this.errorMessage.Text = message;
            this.errorMessage.Visibility = Visibility.Visible;
        }

        private void clearError()
        {
            this.errorMessage.Text = string.Empty;
            this.errorMessage.Visibility = Visibility.Collapsed;
        }

        private void viewVisitButton_Click(object sender, RoutedEventArgs e)
        {
            this.onVisitClicked();
        }
    }
}
