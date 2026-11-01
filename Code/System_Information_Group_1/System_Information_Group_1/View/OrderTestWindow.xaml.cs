using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

    #endregion

    #region Constructors

    public OrderTestWindow(int appointmentId)
    {
        this.InitializeComponent();
        this.controller = new OrderTestController();
        this.appointmentId = appointmentId;
        this.LoadAllTests();
        this.AddTestButton.Click += this.AddTestButton_Click;
        this.RemoveTestButton.Click += this.RemoveTestButton_Click;
        this.SubmitButton.Click += this.SubmitButton_Click;
    }

    #endregion

    #region Methods

    private void LoadAllTests()
    {
        this.allTests = this.controller.GetAllTest();
        this.AllTestsListView.ItemsSource = this.allTests;

        this.existingOrderedTests = this.controller.GetAllTestOrderedByAppointmentId(this.appointmentId);
        this.newlyOrderedTests = new List<Test>();
        this.RefreshOrderedTestsListView();
    }

    private void RefreshOrderedTestsListView()
    {
        var combined = this.existingOrderedTests.Concat(this.newlyOrderedTests).ToList();
        this.OrderedTestsListView.ItemsSource = null;
        this.OrderedTestsListView.ItemsSource = combined;
    }

    private void AddTestButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.AllTestsListView.SelectedItem is Test selectedTest)
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
        if (this.OrderedTestsListView.SelectedItem is Test selectedTest)
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
        if (this.newlyOrderedTests.Any())
        {
            this.controller.OrderTest(this.newlyOrderedTests, this.appointmentId);
            MessageBox.Show(this, "New tests have been ordered successfully");
            this.RefreshOrderedTestsListView();
            this.newlyOrderedTests.Clear();
        }
        else
        {
            MessageBox.Show(this, "No new tests to order.");
        }
    }


    #endregion
}