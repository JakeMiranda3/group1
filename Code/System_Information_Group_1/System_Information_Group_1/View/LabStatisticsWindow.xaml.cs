using System;
using System.Collections.ObjectModel;
using System.Windows;
using System_Information_Group_1.Controller;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View
{
    /// <summary>
    /// Interaction logic for LabStatisticsWindow.xaml
    /// </summary>
    public partial class LabStatisticsWindow : Window
    {
        private readonly LabStatisticsController controller;

        /// <summary>
        /// The observable list of statistics to be displayed.
        /// </summary>
        public ObservableCollection<TestStatistic> Statistics { get; private set; }

        /// <summary>
        /// Initializes a new lab statistics window.
        /// </summary>
        public LabStatisticsWindow()
        {
            this.InitializeComponent();
            this.Statistics = new ObservableCollection<TestStatistic>();
            this.controller = new LabStatisticsController();
            DataContext = this;
        }

        private void generateStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.startDatePicker.SelectedDate == null || this.endDatePicker.SelectedDate == null)
            {
                MessageBox.Show(
                    "Please select both dates.",
                    "Generation Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var startDate = this.startDatePicker.SelectedDate.Value;
            var endDate = this.endDatePicker.SelectedDate.Value;

            if (startDate > endDate)
            {
                MessageBox.Show(
                    "Start date cannot be after end date",
                    "Generation Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                var results = this.controller.GetStatistics(startDate, endDate);

                this.Statistics.Clear();

                foreach (var result in results)
                {
                    this.Statistics.Add(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not generate statistics: " + ex.Message,
                    "Generation Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            var nurseWindow = new NurseWindow();
            var parentWindow = Window.GetWindow(this);
            nurseWindow.Show();
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
}
