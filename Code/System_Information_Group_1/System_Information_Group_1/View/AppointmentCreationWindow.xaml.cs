using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System_Information_Group_1.Model;
using System_Information_Group_1.View.Dialogs;
using System_Information_Group_1.View.UserControls;

namespace System_Information_Group_1.View
{
    /// <summary>
    /// Interaction logic for AppointmentCreationWindow.xaml
    /// </summary>
    public partial class AppointmentCreationWindow : Window
    {
        private const int MaxReasonLength = 100;
        private readonly AppointmentCreationController controller;

        /// <summary>
        /// Gets or sets the doctors.
        /// </summary>
        /// <value>
        /// The doctors.
        /// </value>
        public ObservableCollection<Doctor> Doctors { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentCreationWindow"/> class.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        public AppointmentCreationWindow(int patientId)
        {
            this.InitializeComponent();
            this.controller = new AppointmentCreationController(patientId);
            DataContext = this;
            this.regenerateDoctorComboBox();
        }

        private void regenerateDoctorComboBox()
        {
            this.Doctors = [..this.controller.GetAvailableDoctors()];
        }

        private void doctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.updateInformationWithNewDoctor();

        }

        private void appointmentTimePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.updateInformationWithNewDoctor();
        }

        private void updateInformationWithNewDoctor()
        {
            this.refreshSchedulesWithSelectedDoctor();
        }

        private void createAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.scheduleControl.IsAnyCardSelected())
            {
                MessageBox.Show("Please select a time slot.", "Invalid Information", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            if (this.controller.SelectedPatientId < 0)
            {
                MessageBox.Show(
                    "Patient is unavailable at this time...",
                    "Patient Missing",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            if (this.scheduleControl.scheduleListBox.SelectedItem is not ListBoxItem item ||
                item.Content is not TimeCard selectedCard)
            {
                MessageBox.Show("Please select a time slot.", "Missing Information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedDate = selectedCard.scheduledTime;
            if (this.doctorComboBox.SelectedItem is not Doctor currentDoctor)
            {
                MessageBox.Show("Please select a doctor.", "Missing Information", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }



            var reasonDialog = new ReasonDialog
            {
                Owner = this
            };



            if (reasonDialog.ShowDialog() == true)
            {
                var reason = reasonDialog.Reason;

                if (reason.Length > MaxReasonLength)
                {
                    MessageBox.Show("The appointment reason cannot exceed 100 characters", "Invalid Information", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                var wasAdded =
                    this.controller.StoreAppointment(selectedDate, currentDoctor.DoctorId, this.controller.SelectedPatientId, reason);
                if (wasAdded)
                {
                    MessageBox.Show("Appointment booked!", "Success", MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    this.refreshSchedulesWithSelectedDoctor();
                }
                else
                {
                    MessageBox.Show("Failed to book the appointment. Try again...", "Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }

        }

        private void refreshSchedulesWithSelectedDoctor()
        {
            var date = this.appointmentTimePicker.SelectedDate;
            if (date != null && this.doctorComboBox.SelectedItem is Doctor doctor)
            {
                this.scheduleControl.SetupSchedulesWithAppointments(this.controller.GetDoctorsAppointments(doctor),
                    date.Value,
                    this.controller.GetDoctorsFullName(doctor));
            }
        }

        private void exitAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var patientWindow = new PatientWindow();
                patientWindow.Show();


                var parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow?.Close();
                }
            }

        }
    }
}
