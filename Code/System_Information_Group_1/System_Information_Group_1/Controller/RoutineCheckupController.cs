using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public List<TestOrderDisplay> TestOrders { get; private set; }

        public bool IsReadOnly { get; private set; }

        private readonly VisitDal visitDal;
        private readonly TestOrderDal testOrderDal;
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
            var existingVisit = this.visitDal.GetVisitByAppointmentId(visit.AppointmentId);
            if (existingVisit != null)
            {
                this.HasExistingVisit = true;
                this.Visit = new VisitDisplay(existingVisit.AppointmentId, existingVisit.Symptoms, existingVisit.NurseId,
                    existingVisit.SystolicBloodPressure, existingVisit.DiastolicBloodPressure, existingVisit.Temperature,
                    existingVisit.Pulse, existingVisit.Height, existingVisit.Weight, existingVisit.InitialDiagnosis,
                    existingVisit.FinalDiagnosis);
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

            if (!string.IsNullOrWhiteSpace(this.Visit.FinalDiagnosis))
            {
                IEnumerable<TestOrderDisplay> ordersWhereTestResultsIsEmpty = this.TestOrders.Where(x => String.IsNullOrEmpty(x.TestResult));
                if (ordersWhereTestResultsIsEmpty.GetEnumerator().Current == null)
                {
                    throw new InvalidOperationException("Cannot finalize visit. All test results must be filled in.");
                }

                this.IsReadOnly = true;
            }

            foreach (var testOrder in this.TestOrders)
            {
                if (string.IsNullOrWhiteSpace(testOrder.TestResult))
                {
                    testOrder.PerformedDateTime = DateTime.MinValue;
                }
                else
                {
                    testOrder.PerformedDateTime = DateTime.Now;
                }
            }

            this.testOrderDal.UpdateTestResults(this.TestOrders);
        }

        public void RefreshTestOrders()
        {
            this.TestOrders = this.testOrderDal.GetTestOrdersByAppointmentId(this.Visit.AppointmentId);
        }

        public void SubmitTestResults(IEnumerable<TestOrderDisplay> collection)
        {
            this.testOrderDal.UpdateTestResults(collection);
        }
    }
}
