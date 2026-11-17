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
        /// <summary>
        /// Gets the identifier of the appointment the user selected.
        /// </summary>
        /// <value>
        /// The selected appointment identifier, or <c>null</c> if none was selected.
        /// </value>
        public int? SelectedAppointmentId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentSelectionDialog"/> class.
        /// </summary>
        /// <param name="appointments">The appointments to choose from.</param>
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
