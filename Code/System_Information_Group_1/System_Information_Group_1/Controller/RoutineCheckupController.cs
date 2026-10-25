using System;
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

        public bool IsReadOnly { get; private set; }

        private readonly VisitDal visitDal;
        private readonly bool hasExistingVisit;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineCheckupController"/> class.
        /// </summary>
        /// <param name="visit">The visit.</param>
        public RoutineCheckupController(Visit visit)
        {
            this.visitDal = new VisitDal();
            var existingVisit = this.visitDal.GetVisitByAppointmentId(visit.AppointmentId);
            if (existingVisit != null)
            {
                this.hasExistingVisit = true;
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
                this.hasExistingVisit = false;
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

            if (this.hasExistingVisit)
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
                this.IsReadOnly = true;
            }
        }
    }
}
