
using System;

namespace System_Information_Group_1.Model
{
    /// <summary>
    /// The Doctor class represents a doctor in the DB. 
    /// @author Colby
    /// @version Fall 2026
    /// </summary>
    public class Doctor
    {
        #region Properties        
        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        /// <value>
        /// The person identifier.
        /// </value>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the doctor identifier.
        /// </summary>
        /// <value>
        /// The doctor identifier.
        /// </value>
        public int DoctorId { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Doctor"/> class.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="doctorId">The doctor identifier.</param>
        public Doctor(int personId, int doctorId) 
        {
            this.PersonId = personId != 0 ? personId : throw new ArgumentException("PersonId cannot be zero.");
            this.DoctorId = doctorId != 0 ? doctorId: throw new ArgumentException("DoctorId cannot be zero");
        }
        #endregion
    }
}