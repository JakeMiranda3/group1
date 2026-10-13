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
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View
{
    /// <summary>
    /// Interaction logic for NurseWindow.xaml
    /// </summary>
    public partial class NurseWindow : Window
    {
        private NurseController controller;
        public NurseWindow()
        {
            InitializeComponent();
            this.controller = new NurseController();

            this.NurseDataGrid.IsReadOnly = true;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            String firstName;
            String lastName;
            DateTime? date;

            firstName = this.FirstNameTextBox.Text.Trim();
            lastName = this.LastNameTextBox.Text.Trim();
            date = this.DateOfBirthPicker.SelectedDate;

            if (String.IsNullOrEmpty(firstName) && String.IsNullOrEmpty(lastName) && date == null)
            {
                return;
            }

            List<NurseDisplay> nurses = this.controller.HandleNurseSearch(firstName, lastName, date);

            this.NurseDataGrid.ItemsSource = nurses;
        }

        
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = this.NurseDataGrid.SelectedItem as NurseDisplay;
            if (selected == null)
            {
                MessageBox.Show("Please select a nurse to edit.", "Edit Nurse", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var editCopy = new NurseDisplay(
                selected.NurseId,
                selected.PersonId,
                selected.FirstName,
                selected.LastName,
                selected.DateOfBirth,
                selected.Gender,
                selected.PhoneNumber,
                selected.Address, 
                selected.Zip,
                selected.City,
                selected.State
            );

            var dlg = new NurseEditWindow(editCopy) { Owner = this };
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                var list = this.NurseDataGrid.ItemsSource as List<NurseDisplay>;
                if (list != null)
                {
                    int idx = list.IndexOf(selected);
                    if (idx >= 0)
                    {
                        list[idx] = dlg.EditedNurse;
                        this.NurseDataGrid.ItemsSource = null;
                        this.NurseDataGrid.ItemsSource = list;
                    }
                }

                bool success = this.controller.UpdateNurse(dlg.EditedNurse);

                if (success)
                {
                    MessageBox.Show("Nurse successfully edited.", "Edit Nurse", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Nurse failed to edited.", "Edit Nurse", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
