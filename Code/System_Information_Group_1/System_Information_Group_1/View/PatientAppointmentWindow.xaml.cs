using System;
using System.Collections.Generic;
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

namespace System_Information_Group_1.View
{
    /// <summary>
    /// Interaction logic for PatientAppointmentWindow.xaml
    /// </summary>
    public partial class PatientAppointmentWindow : Window
    {
        private readonly PatientAppointmentViewController controller;
        public PatientAppointmentWindow(int patientId)
        {
            this.InitializeComponent();
            this.controller = new PatientAppointmentViewController(patientId);
            this.refreshPatientAppointments();
        }

        private void refreshPatientAppointments()
        {
            this.scheduleControl.SetupScheduleWithScheduledAppointments(this.controller.GetPatientAppointments());
        }

        private void PatientAppointmentTimePicker_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
