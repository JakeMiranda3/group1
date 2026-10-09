using System;

namespace System_Information_Group_1.Model;

/// <summary>
///     The Patient class represents a patient in the DB.
///     @author Jake
///     @version Fall 2026
/// </summary>
public class Patient
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
    /// Gets or sets the patient identifier.
    /// </summary>
    /// <value>
    /// The patient identifier.
    /// </value>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
    /// </value>
    public bool IsActive { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Patient"/> class.
    /// </summary>
    /// <param name="personId">The person identifier.</param>
    /// <param name="patientId">The patient identifier.</param>
    /// <param name="isActive">if set to <c>true</c> [is active].</param>
    /// <exception cref="System.ArgumentException">
    /// PersonId cannot be zero.
    /// or
    /// DoctorId cannot be zero.
    /// </exception>
    public Patient(int personId, int patientId, bool isActive)
    {
        this.PersonId = personId != 0 ? personId : throw new ArgumentException("PersonId cannot be zero.");
        this.PatientId = patientId != 0 ? patientId : throw new ArgumentException("DoctorId cannot be zero.");
        this.IsActive = isActive;
    }

    #endregion
}