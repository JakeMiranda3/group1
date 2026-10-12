using System;

namespace System_Information_Group_1.Model;

/// <summary>
///     The Doctor Speciality class
///     @author Colby
///     @version Fall 2026
/// </summary>
public class DoctorSpeciality
{
    #region Properties

    /// <summary>
    ///     Gets or sets the person identifier.
    /// </summary>
    /// <value>
    ///     The person identifier.
    /// </value>
    public int PersonId { get; set; }

    /// <summary>
    ///     Gets or sets the name of the speciality.
    /// </summary>
    /// <value>
    ///     The name of the speciality.
    /// </value>
    public string SpecialityName { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="DoctorSpeciality" /> class.
    /// </summary>
    /// <param name="personId">The person identifier.</param>
    /// <param name="specialityName">Name of the speciality.</param>
    /// <exception cref="System.ArgumentNullException">
    ///     personId - PersonId cannot be zero.
    ///     or
    ///     specialityName - SpecialityName cannot be null.
    /// </exception>
    public DoctorSpeciality(int personId, string specialityName)
    {
        this.PersonId = personId != 0
            ? personId
            : throw new ArgumentNullException(nameof(personId), "PersonId cannot be zero.");
        this.SpecialityName = specialityName ??
                              throw new ArgumentNullException(nameof(specialityName), "SpecialityName cannot be null.");
    }

    #endregion
}