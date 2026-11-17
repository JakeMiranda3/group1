using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller
{
    /// <summary>
    /// THe controller for the routine checkup window.
    /// @author Colby
    /// @version Fall 2026
    /// </summary>
    public class RoutineCheckupController
    {
        /// <summary>
        /// Gets the visit.
        /// </summary>
        /// <value>
        /// The visit.
        /// </value>
        public Visit Visit { get; private set; }
        /// <summary>
        /// Gets the test orders.
        /// </summary>
        /// <value>
        /// The test orders.
        /// </value>
        public List<TestOrderDisplay> TestOrders { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; private set; }

        /// <summary>
        /// Gets the scheduled date and time of the visit's appointment.
        /// </summary>
        /// <value>
        /// The appointment date and time.
        /// </value>
        public DateTime AppointmentDateTime { get; private set; }

        private readonly VisitDal visitDal;
        private readonly TestOrderDal testOrderDal;
        /// <summary>
        /// Gets a value indicating whether this instance has existing visit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has existing visit; otherwise, <c>false</c>.
        /// </value>
        public bool HasExistingVisit { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineCheckupController"/> class.
        /// </summary>
        /// <param name="visit">The visit.</param>
        public RoutineCheckupController(Visit visit)
        {
            this.visitDal = new VisitDal();
            this.testOrderDal = new TestOrderDal();
            this.TestOrders = this.testOrderDal.GetTestOrdersByAppointmentId(visit.AppointmentId);
            this.AppointmentDateTime = new AppointmentDal().GetAppointmentDateTime(visit.AppointmentId);
            var existingVisit = this.visitDal.GetVisitByAppointmentId(visit.AppointmentId);
            if (existingVisit != null)
            {
                this.HasExistingVisit = true;
                this.Visit = new VisitDisplay(existingVisit.AppointmentId, existingVisit.Symptoms, existingVisit.NurseId,
                    existingVisit.SystolicBloodPressure, existingVisit.DiastolicBloodPressure, existingVisit.Temperature,
                    existingVisit.Pulse, existingVisit.Height, existingVisit.Weight, existingVisit.InitialDiagnosis,
                    existingVisit.FinalDiagnosis, existingVisit.FinalDiagnosisEnteredDatetime);
                this.IsReadOnly = !string.IsNullOrWhiteSpace(existingVisit.FinalDiagnosis);
            }
            else
            {
                this.Visit = visit;
                this.IsReadOnly = false;
                this.HasExistingVisit = false;
            }
        }

        /// <summary>
        /// Submits the visit.
        /// </summary>
        public void SubmitVisit()
        {
            if (this.IsReadOnly || this.visitDal.IsVisitFinalized(this.Visit.AppointmentId))
            {
                throw new InvalidOperationException("Final diagnosis is already set. This visit is read-only.");
            }

            var isFinalizing = !string.IsNullOrWhiteSpace(this.Visit.FinalDiagnosis);
            if (isFinalizing && this.GetTestsMissingResults().Count > 0)
            {
                throw new InvalidOperationException(
                    "Cannot finalize visit. All test results must be filled in and submitted.");
            }

            this.Visit.FinalDiagnosisEnteredDatetime = isFinalizing ? DateTime.Now : null;

            if (this.HasExistingVisit)
            {
                var wasUpdated = this.visitDal.UpdateVisit(this.Visit);
                if (!wasUpdated)
                {
                    throw new InvalidOperationException("Unable to update visit. It may already be finalized.");
                }
            }
            else
            {
                this.visitDal.AddVisit(this.Visit);
            }

            // Only lock the visit once the final diagnosis has actually been saved.
            this.IsReadOnly = isFinalizing;
        }

        /// <summary>
        /// Gets the names of the ordered tests that have no saved result.
        /// Results typed into the grid only count once Submit Test Results has saved them, so a visit
        /// can't be finalized with results that were never saved.
        /// </summary>
        /// <returns>The names of the tests missing a saved result; empty if every test has one.</returns>
        public IList<string> GetTestsMissingResults()
        {
            return this.testOrderDal.GetTestOrdersByAppointmentId(this.Visit.AppointmentId)
                .Where(order => string.IsNullOrWhiteSpace(order.TestResult))
                .Select(order => order.Name)
                .ToList();
        }
        /// <summary>
        /// Refreshes the test orders.
        /// </summary>
        public void RefreshTestOrders()
        {
            this.TestOrders = this.testOrderDal.GetTestOrdersByAppointmentId(this.Visit.AppointmentId);
        }

        /// <summary>
        /// Submits the test results.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public void SubmitTestResults(IEnumerable<TestOrderDisplay> collection)
        {
           
            

           
            bool testResultIsNonNumericString = false;
            var testOrderDisplays = collection.ToList();
            foreach (TestOrderDisplay test in testOrderDisplays)
            {
                if (test.LowValue != 0 && !String.IsNullOrEmpty(test.TestResult) && !Double.TryParse(test.TestResult, out _))
                {
                    testResultIsNonNumericString = true;
                    break;
                }
            }

            if (testResultIsNonNumericString)
            {
                MessageBox.Show("You must enter a number for the result of a metric test");
                return;
            }

            var resultEnteredDatetime = DateTime.Now;
            foreach (TestOrderDisplay test in testOrderDisplays)
            {
                if (test.HasTestResultChanged)
                {
                    test.TestResultDatetime = string.IsNullOrWhiteSpace(test.TestResult)
                        ? null
                        : resultEnteredDatetime;
                }
            }

            try
            {
                this.testOrderDal.UpdateTestResults(testOrderDisplays);
                MessageBox.Show("Test Results Submitted Successfully", "Test Results");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Test Results");
            }
        }
    }
}
