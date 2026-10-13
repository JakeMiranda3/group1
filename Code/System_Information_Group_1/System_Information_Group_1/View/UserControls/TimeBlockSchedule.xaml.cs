using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View.UserControls
{
    /// <summary>
    /// Interaction logic for TimeBlockSchedule.xaml
    /// </summary>
    public partial class TimeBlockSchedule : UserControl
    {
        private static readonly TimeSpan ClinicStartTime = new (8, 0, 0);
        private static readonly TimeSpan ClinicEndTime = new (17, 0, 0);
        private const int TimeIntervalInMinutes = 15;
        public TimeBlockSchedule()
        {
            InitializeComponent();
        }



        public void SetupSchedulesWithAppointments(IList<Appointment> appointments, DateTime selectedDay, string doctorName)
        {
            this.clearScheduleSlots();
            this.fillAppointmentCards(appointments, selectedDay);
            var timeSlot = new TimeSpan(ClinicStartTime.Hours, ClinicStartTime.Minutes, ClinicStartTime.Seconds);
            while (timeSlot.Ticks < ClinicEndTime.Ticks)
            {
                if (!appointments.Any(appointment => appointment.Date.TimeOfDay.Equals(timeSlot)))
                {
                    this.addEmptyScheduleSlot(selectedDay.Date.AddTicks(timeSlot.Ticks), doctorName);
                }

                timeSlot = timeSlot.Add(TimeSpan.FromMinutes(TimeIntervalInMinutes));
            }

            this.sortCardsByDate();

        }

        private void sortCardsByDate()
        {
            var sortedCards = this.scheduleListBox.Items
                .Cast<ListBoxItem>()
                .OrderBy(item => ((TimeCard)item.Content).scheduledTime)
                .ToList();

            this.scheduleListBox.Items.Clear();

            foreach (var item in sortedCards)
            {
                this.scheduleListBox.Items.Add(item);
            }
        }


        private void fillAppointmentCards(IList<Appointment> appointments, DateTime selectedDay)
        {
            foreach (Appointment appointment in appointments)
            {
                if (appointment.Date.Date != selectedDay.Date)
                    continue;

                string doctorName =
                    $"{appointment.Doctor.FirstName} {appointment.Doctor.LastName}";

                string patientName =
                    $"{appointment.Patient.FirstName} {appointment.Patient.LastName}";

                this.addScheduleSlotWithAppointmentFilled(
                    appointment.Date,
                    doctorName,
                    patientName,
                    appointment.Patient.ContactPhoneNumber
                );
            }
        }

        private void addScheduleSlotWithAppointmentFilled(DateTime date, String doctorName, String patientName, String patientPhone)
        {
            var item = new ListBoxItem
            {
                Content = new TimeCard(date, doctorName, patientName, patientPhone),
                IsEnabled = false
            };

            this.scheduleListBox.Items.Add(item);
        }

        private void addEmptyScheduleSlot(DateTime date, String doctorName)
        {
            var item = new ListBoxItem
            {
                Content = new TimeCard(date, doctorName),
                IsEnabled = true
            };

            this.scheduleListBox.Items.Add(item);
        }

        public void clearScheduleSlots()
        {
            this.scheduleListBox.Items.Clear();
        }

        public bool IsAnyCardSelected()
        {
            return this.scheduleListBox.SelectedItem is ListBoxItem item && item.IsEnabled;
        }
    }
}
