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

    public int PersonId { get; set; }

    public int PatientId { get; set; }

    public bool IsActive { get; set; }

    #endregion

    #region Constructors

    public Patient(int personId, int patientId, bool isActive)
    {
        this.PersonId = personId != 0 ? personId : throw new ArgumentException("PersonId cannot be zero.");
        this.PatientId = patientId != 0 ? patientId : throw new ArgumentException("DoctorId cannot be zero.");
        this.IsActive = isActive;
    }

    #endregion
}