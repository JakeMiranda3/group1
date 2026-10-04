using System;
namespace System_Information_Group_1.Model
{
    /// <summary>
    /// The administrator class
    /// @author Colby
    /// @version Fall 2026
    /// </summary>
    public class Administrator
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
        /// Gets or sets the administrator identifier.
        /// </summary>
        /// <value>
        /// The administrator identifier.
        /// </value>
        public int AdministratorId { get; set; }
        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Administrator"/> class.
        /// </summary>
        /// <param name="personId">The person identifier.</param>
        /// <param name="administratorId">The administrator identifier.</param>
        /// <exception cref="System.ArgumentNullException">
        /// personId - PersonId cannot be null or zero.
        /// or
        /// administratorId - AdministratorId cannot be null or zero.
        /// </exception>
        public Administrator(int personId, int administratorId)
        { 
            this.PersonId = personId != 0 ? personId : throw new ArgumentNullException(nameof(personId), "PersonId cannot be null or zero.");
            this.AdministratorId = administratorId != 0 ? administratorId : throw new ArgumentNullException(nameof(administratorId), "AdministratorId cannot be null or zero.");
        }
        #endregion
    }
}