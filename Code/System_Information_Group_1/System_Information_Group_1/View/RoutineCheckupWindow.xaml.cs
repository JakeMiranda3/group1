using System;
using System.Windows;
using System.Windows.Controls;
using System_Information_Group_1.Controller;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View
{
    /// <summary>
    /// Interaction logic for RoutineCheckupWindow.xaml
    /// </summary>
    public partial class RoutineCheckupWindow : Window
    {
        private readonly RoutineCheckupController controller;

        public RoutineCheckupWindow(Visit visitDisplay)
        {
            InitializeComponent();
            this.controller = new RoutineCheckupController(visitDisplay);
            this.DataContext = this.controller.Visit;
            this.applyReadOnlyState();
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.controller.IsReadOnly)
            {
                MessageBox.Show("This visit is finalized and cannot be edited.", "Read Only", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            if (!string.IsNullOrWhiteSpace(this.controller.Visit.FinalDiagnosis))
            {
                var confirmation = MessageBox.Show(
                    "Final diagnosis is filled. After saving, this visit will become read-only and no longer appear for editing. Continue?",
                    "Confirm Final Diagnosis",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (confirmation != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            try
            {
                this.controller.SubmitVisit();
                MessageBox.Show("Visit information saved successfully.", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);

                if (this.controller.IsReadOnly)
                {
                    this.applyReadOnlyState();
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void applyReadOnlyState()
        {
            if (!this.controller.IsReadOnly)
            {
                return;
            }

            this.symptomsTextBox.IsReadOnly = true;
            this.systolicTextBox.IsReadOnly = true;
            this.diastolicTextBox.IsReadOnly = true;
            this.temperatureTextBox.IsReadOnly = true;
            this.pulseTextBox.IsReadOnly = true;
            this.heightTextBox.IsReadOnly = true;
            this.weightTextBox.IsReadOnly = true;
            this.initialDiagnosisTextBox.IsReadOnly = true;
            this.finalDiagnosisTextBox.IsReadOnly = true;
            this.submitButton.IsEnabled = false;
            this.submitButton.Content = "Finalized";
        }

        private void OnOrderTestClick(object sender, RoutedEventArgs e)
        {
            OrderTestWindow orderTestWindow = new OrderTestWindow(this.controller.Visit.AppointmentId);
            orderTestWindow.Show();
        }
    }
}
