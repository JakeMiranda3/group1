using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System_Information_Group_1.Controller;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.View;

/// <summary>
///     Interaction logic for OrderTestWindow.xaml
/// </summary>
public partial class OrderTestWindow : Window
{
    #region Data members

    private readonly OrderTestController controller;
    private List<Test> allTests;
    private List<Test> existingOrderedTests; 
    private List<Test> newlyOrderedTests;    
    private int appointmentId;

    /// <summary>
    ///     Occurs when the ordered tests have been submitted.
    /// </summary>
    public event EventHandler? TestsSubmitted;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="OrderTestWindow" /> class.
    /// </summary>
    /// <param name="appointmentId">The appointment to order tests for.</param>
    public OrderTestWindow(int appointmentId)
    {
        this.InitializeComponent();
        this.controller = new OrderTestController();
        this.appointmentId = appointmentId;
        this.LoadAllTests();
        this.addTestButton.Click += this.AddTestButton_Click;
        this.removeTestButton.Click += this.RemoveTestButton_Click;
        this.submitButton.Click += this.SubmitButton_Click;
    }

    #endregion

    #region Methods

    private void LoadAllTests()
    {
        this.allTests = this.controller.GetAllTest();
        this.allTestsListView.ItemsSource = this.allTests;

        this.existingOrderedTests = this.controller.GetTestAlreadyOrderedByAppointmentId(this.appointmentId);
        this.newlyOrderedTests = new List<Test>();
        this.RefreshOrderedTestsListView();
    }

    private void RefreshOrderedTestsListView()
    {
        var orderedTests = this.existingOrderedTests.Concat(this.newlyOrderedTests).ToList();
        var orderedTestCodes = orderedTests.Select(test => test.TestCode).ToHashSet();

        this.orderedTestsListView.ItemsSource = orderedTests;
        this.allTestsListView.ItemsSource = this.allTests.Where(test => !orderedTestCodes.Contains(test.TestCode)).ToList();
    }

    private void AddTestButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.allTestsListView.SelectedItem is Test selectedTest)
        {
            
            bool alreadyOrdered = this.existingOrderedTests.Any(t => t.TestCode == selectedTest.TestCode)
                                  || this.newlyOrderedTests.Any(t => t.TestCode == selectedTest.TestCode);
            if (!alreadyOrdered)
            {
                this.newlyOrderedTests.Add(selectedTest);
                this.RefreshOrderedTestsListView();
            }
        }
    }

    private void RemoveTestButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.orderedTestsListView.SelectedItem is Test selectedTest)
        {
            // Only allow removing from newly added tests
            if (this.newlyOrderedTests.RemoveAll(t => t.TestCode == selectedTest.TestCode) > 0)
            {
                this.RefreshOrderedTestsListView();
            }
            else
            {
                MessageBox.Show(this, "This test has already been ordered and cannot be cancelled.");
            }
            
        }
    }

    private void SubmitButton_Click(object sender, RoutedEventArgs e)
    {
        if (!this.newlyOrderedTests.Any())
        {
            MessageBox.Show(this, "No new tests to order.");
            return;
        }

        try
        {
            this.controller.OrderTest(
                this.newlyOrderedTests, this.appointmentId);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message,
                "Order Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        this.existingOrderedTests.AddRange(this.newlyOrderedTests);
        this.newlyOrderedTests.Clear();

        this.RefreshOrderedTestsListView();

        this.TestsSubmitted?.Invoke(this, EventArgs.Empty);

        MessageBox.Show(this,
            "New tests have been ordered successfully.",
            "Success", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }


    #endregion
}