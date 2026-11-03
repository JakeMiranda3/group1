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
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;
using System_Information_Group_1.View.Dialog;
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
        private NurseDal nurseDal;

        /// <summary>
        /// Gets or sets the doctors.
        /// </summary>
        /// <value>
        /// The doctors.
        /// </value>
        public ObservableCollection<Doctor> Doctors { get; set; }

        public AppointmentWindowState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentCreationWindow"/> class.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        public AppointmentCreationWindow(int patientId, AppointmentWindowState state)
        {
            this.InitializeComponent();
            this.controller = new AppointmentCreationController(patientId);
            DataContext = this;
            this.nurseDal = new NurseDal();
            this.State = state;
            if (state == AppointmentWindowState.PatientView)
            {
                this.setupComponentsForPatientView();
            } else if (state == AppointmentWindowState.DoctorView)
            {
                this.regenerateDoctorComboBox();
            }


        }

        private void setupComponentsForPatientView()
        {
            this.doctorComboBox.Visibility = Visibility.Collapsed;
            this.createAppointmentButton.Visibility = Visibility.Collapsed;
            this.doctorComboBoxLabel.Visibility = Visibility.Collapsed;
            this.scheduleControl.AddCardsFromAppointments(this.controller.GetPatientAppointments());
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
            if (this.State == AppointmentWindowState.PatientView)
            {
                this.refreshSchedulesByPatientAndDate();
            } else if (this.State == AppointmentWindowState.DoctorView)
            {
                this.updateInformationWithNewDoctor();
            }
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

            if (!this.scheduleControl.IsSelectedCardAvailable())
            {
                MessageBox.Show(
                    "Please Select an appointment that is available",
                    "Appointment Overlap",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
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
                this.scheduleControl.AddCardsWithScheduledAndOpenCards(this.controller.GetDoctorsAppointments(doctor),
                    date.Value,
                    this.controller.GetDoctorsFullName(doctor));
            }
        }

        private void refreshSchedulesByPatientAndDate()
        {
            if (this.appointmentTimePicker.SelectedDate != null)
            {
                this.scheduleControl.AddCardsFromAppointments(this.controller.GetPatientAppointments(), this.appointmentTimePicker.SelectedDate.Value);
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

        private void onVisitInfoButtonClicked(object sender, RoutedEventArgs e)
        {
            // TODO : CHANGE THIS TO BE WHENEVER A BOOKED TIME SLOT IS CLICKED, BECAUSE THEY CANNNOT BE SELECTED IF THEY ARE BOOKED, SO THIS WILL NOT WORK FOR NOW
            if (this.scheduleControl.scheduleListBox.SelectedItem is not ListBoxItem item ||
                item.Content is not TimeCard selectedCard)
            {
                MessageBox.Show("Please select a time slot.", "Missing Information",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ListBoxItem listBoxItem = (ListBoxItem) this.scheduleControl.scheduleListBox.SelectedItem;
            TimeCard timeCardSelected = (TimeCard) listBoxItem.Content;
            DateTime timeSelected = DateTime.MinValue;
            if (timeCardSelected != null)
            {
                timeSelected = timeCardSelected.scheduledTime;
            }

            int appointmentId = this.controller.GetAppointmentId(this.controller.SelectedPatientId, timeSelected);

            int personIdOfNurseLoggedIn = AuthServiceSingleton.GetAccount().PersonId;

            int nurseId = this.nurseDal.GetNurseWithPersonId(personIdOfNurseLoggedIn).NurseId;

            Visit visitDisplay = new VisitDisplay(appointmentId, nurseId);

            RoutineCheckupWindow routineCheckupWindow = new RoutineCheckupWindow(visitDisplay);

            routineCheckupWindow.Show();

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.scheduleControl.AddCardsFromAppointments(this.controller.GetPatientAppointments());
            this.doctorComboBox.IsEnabled = false;
            this.appointmentTimePicker.IsEnabled = false;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.doctorComboBox.IsEnabled = true;
            this.appointmentTimePicker.IsEnabled = true;
            this.scheduleControl.clearScheduleSlots();
            
            if (this.doctorComboBox.SelectedItem != null && this.appointmentTimePicker.SelectedDate != null)
            {
                this.refreshSchedulesWithSelectedDoctor();
            }
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

                if (this.State == AppointmentWindowState.PatientView)
                {
                    if (this.appointmentTimePicker.SelectedDate.HasValue)
                    {
                        this.refreshSchedulesByPatientAndDate();
                    }
                    else
                    {
                        this.scheduleControl.AddCardsFromAppointments(
                            this.controller.GetPatientAppointments());
                    }
                }
                else if (this.State == AppointmentWindowState.DoctorView)
                {
                    this.refreshSchedulesWithSelectedDoctor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open appointment information: {ex.Message}",
                    "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
