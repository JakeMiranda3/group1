using System.Collections.Generic;
using System.Windows;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View.Dialog
{
    /// <summary>
    /// Interaction logic for AppointmentSelectionDialog.xaml
    /// </summary>
    public partial class AppointmentSelectionDialog : Window
    {
        public int? SelectedAppointmentId { get; private set; }

        public AppointmentSelectionDialog(IList<Appointment> appointments)
        {
            this.InitializeComponent();
            this.appointmentDataGrid.ItemsSource = appointments;
        }

        private void SelectButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.appointmentDataGrid.SelectedItem is not Appointment selectedAppointment)
            {
                MessageBox.Show("Please select an appointment.", "Missing Information", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            this.SelectedAppointmentId = selectedAppointment.AppointmentId;
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
