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
using System_Information_Group_1.Controller.UserControl;
using System_Information_Group_1.DAL;
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
        private readonly SchedulerController controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeBlockSchedule"/> class.
        /// </summary>
        public TimeBlockSchedule()
        {
            this.InitializeComponent();
            this.controller = new SchedulerController();
        }


        /// <summary>
        /// Setups the schedules with appointments.
        /// </summary>
        /// <param name="appointments">The appointments.</param>
        /// <param name="selectedDay">The selected day.</param>
        /// <param name="doctorName">Name of the doctor.</param>
        public void AddCardsWithScheduledAndOpenCards(IList<Appointment> appointments, DateTime selectedDay, string doctorName)
        {
            this.clearScheduleSlots();
            var appointmentsOnDate = this.filterAppointmentsByDate(appointments, selectedDay);
            this.fillAppointmentCards(appointmentsOnDate);
            var timeSlot = ClinicStartTime;
            while (timeSlot < ClinicEndTime)
            {
                if (appointmentsOnDate.All(appointment => appointment.Date.TimeOfDay != timeSlot))
                {
                    this.addEmptyScheduleSlot(selectedDay.Date.Add(timeSlot), doctorName);
                }

                timeSlot = timeSlot.Add(TimeSpan.FromMinutes(TimeIntervalInMinutes));
            }

            this.sortCardsByDate();

        }

        public void AddCardsFromAppointments(IList<Appointment> appointments)
        {
            this.clearScheduleSlots();
            this.fillAppointmentCards(appointments);
            this.sortCardsByDate();
        }

        public void AddCardsFromAppointments(IList<Appointment> appointments, DateTime time)
        {
            var filteredAppointments = this.filterAppointmentsByDate(appointments, time);
            this.clearScheduleSlots();
            this.fillAppointmentCards(filteredAppointments);
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


        private void fillAppointmentCards(IList<Appointment> appointments)
        {
            foreach (Appointment appointment in appointments)
            {
                var doctorName =
                    $"{appointment.Doctor.Person.FirstName} {appointment.Doctor.Person.LastName}";

                var patientName =
                    $"{appointment.Patient.Person.FirstName} {appointment.Patient.Person.LastName}";

                this.addAppointmentCard(
                    appointment.Date,
                    doctorName,
                    patientName,
                    appointment.Patient.Person.ContactPhoneNumber,
                    this.controller.DetermineCardState(appointment)
                );
            }
        }




        private void addAppointmentCard(DateTime date, String doctorName, String patientName, String patientPhone, TimeCardState cardState)
        {
            var card = new TimeCard(date, doctorName, patientName, patientPhone);
            card.State = cardState;
            var item = new ListBoxItem
            {
                Content = card,
            };

            this.scheduleListBox.Items.Add(item);
        }

        private void addEmptyScheduleSlot(DateTime date, String doctorName)
        {
            var card = new TimeCard(date, doctorName);
            card.State = TimeCardState.Available;
            var item = new ListBoxItem
            {
                Content = card,
            };

            this.scheduleListBox.Items.Add(item);
        }

        /// <summary>
        /// Clears the schedule slots.
        /// </summary>
        public void clearScheduleSlots()
        {
            this.scheduleListBox.Items.Clear();
        }

        /// <summary>
        /// Determines whether [is any card selected].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is any card selected]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAnyCardSelected()
        {
            return this.scheduleListBox.SelectedItem is ListBoxItem item && item.IsEnabled;
        }

        public bool IsSelectedCardAvailable()
        {
            return this.scheduleListBox.SelectedItem is ListBoxItem item && item.Content is TimeCard card &&
                   card.State == TimeCardState.Available;
        }

        private IList<Appointment> filterAppointmentsByDate(IList<Appointment> appointments, DateTime selectedDay)
        {
            return appointments.Where(a => a.Date.Date == selectedDay.Date).ToList();
        }

    }
}
