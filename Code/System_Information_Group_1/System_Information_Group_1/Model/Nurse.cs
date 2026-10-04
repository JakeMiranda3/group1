using System;


namespace System_Information_Group_1.Model
{
    /// <summary>
    /// The Nurse class
    /// @author Colby
    /// @version Fall 2026
    /// </summary>
    public class Nurse
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
        /// Gets or sets the nurse identifier.
        /// </summary>
        /// <value>
        /// The nurse identifier.
        /// </value>
        public int NurseId { get; set; }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Nurse"/> class.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="nurseId">The nurse identifier.</param>
        /// <exception cref="System.ArgumentException">
        /// PersonId cannot be zero.
        /// or
        /// NurseId cannot be zero.
        /// </exception>
        public Nurse(int personId, int nurseId) {
            this.PersonId = personId != 0 ? personId : throw new ArgumentException("PersonId cannot be zero."); 
            this.NurseId = nurseId != 0 ? nurseId : throw new ArgumentException("NurseId cannot be zero.");
        }
        #endregion
    }
}