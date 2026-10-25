using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Visit Visit { get; private set;  }

        private VisitDal visitDal;
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineCheckupController"/> class.
        /// </summary>
        /// <param name="visit">The visit.</param>
        public RoutineCheckupController(Visit visit)
        {
            this.Visit = visit;
            this.visitDal = new VisitDal();
        }

        /// <summary>
        /// Submits the visit.
        /// </summary>
        public void SubmitVisit()
        {
            this.visitDal.AddVisit(this.Visit);
        }
    }
}
