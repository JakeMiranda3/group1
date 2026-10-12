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

namespace System_Information_Group_1.View
{
    /// <summary>
    /// Interaction logic for AppointmentCreationWindow.xaml
    /// </summary>
    public partial class AppointmentCreationWindow : Window
    {
        private readonly AppointmentCreationController controller;

        public ObservableCollection<Doctor> Doctors { get; set; }
        public AppointmentCreationWindow()
        {
            this.InitializeComponent();
            this.controller = new AppointmentCreationController();
            DataContext = this;
            this.regenerateDoctorComboBox();
        }

        private void regenerateDoctorComboBox()
        {
            this.Doctors = new ObservableCollection<Doctor>(this.controller.GetAvailableDoctors());
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
            var date = this.appointmentTimePicker.SelectedDate;
            if (date != null && this.doctorComboBox.SelectedItem is Doctor doctor)
            {
                this.scheduleControl.SetupSchedulesWithAppointments(this.controller.GetDoctorsAppointments(doctor),
                    date.Value,
                    this.controller.getDoctorsFullName(doctor));
            }
        }
    }
}
