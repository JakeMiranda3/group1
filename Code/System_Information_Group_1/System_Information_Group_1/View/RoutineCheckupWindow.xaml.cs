using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            this.testsDataGrid.ItemsSource = new ObservableCollection<TestOrderDisplay>(this.controller.TestOrders);
        }

        private void OnSubmitCheckupButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.controller.IsReadOnly)
            {
                MessageBox.Show("This visit is finalized and cannot be edited.", "Read Only", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            if (!this.areRoutineCheckInputsValid())
            {
                MessageBox.Show("Please correct the highlighted vitals before submitting.", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(this.controller.Visit.FinalDiagnosis))
            {
                var testsMissingResults = this.controller.GetTestsMissingResults();
                if (testsMissingResults.Count > 0)
                {
                    MessageBox.Show(
                        "A final diagnosis can't be saved until every ordered test has a result. " +
                        "Enter the results and click Submit Test Results first.\n\nMissing results:\n" +
                        string.Join("\n", testsMissingResults),
                        "Missing Test Results", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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

            // Once finalized, tests can't be added or results cleared, or the visit could end up with missing results.
            this.orderTestButton.IsEnabled = false;
            this.testsDataGrid.IsReadOnly = true;
            this.submitTestResultsButton.IsEnabled = false;
        }

        private void OnOrderTestClick(object sender, RoutedEventArgs e)
        {
            if (!this.controller.HasExistingVisit)
            {
                MessageBox.Show("Please save the visit information before ordering tests.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            OrderTestWindow orderTestWindow = new OrderTestWindow(this.controller.Visit.AppointmentId);

            orderTestWindow.TestsSubmitted += (sen, args) =>
            {
                this.RefreshTestOrders();
            };
            orderTestWindow.Show();
        }

        private void RefreshTestOrders()
        {
            this.controller.RefreshTestOrders();

            this.testsDataGrid.ItemsSource =
                new ObservableCollection<TestOrderDisplay>(
                    this.controller.TestOrders);
        }

        private void OnSubmitTestResultClick(object sender, RoutedEventArgs e)
        {
           
            this.testsDataGrid.CommitEdit(DataGridEditingUnit.Row, true);

            IEnumerable<TestOrderDisplay> collection = (IEnumerable<TestOrderDisplay>)this.testsDataGrid.ItemsSource;
            this.controller.SubmitTestResults(collection);
            this.RefreshTestOrders();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool areRoutineCheckInputsValid()
        {
            TextBox[] vitalTextBoxes =
            {
                this.systolicTextBox,
                this.diastolicTextBox,
                this.pulseTextBox,
                this.temperatureTextBox,
                this.heightTextBox,
                this.weightTextBox
            };

            bool areAllValid = true;
            TextBox firstInvalidTextBox = null;

            foreach (TextBox textBox in vitalTextBoxes)
            {
                // Re-runs the binding's validation rules so untouched fields are checked too.
                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

                if (Validation.GetHasError(textBox))
                {
                    areAllValid = false;
                    firstInvalidTextBox ??= textBox;
                }
            }

            firstInvalidTextBox?.Focus();
            return areAllValid;
        }
    }
}
