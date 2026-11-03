


using System;

namespace System_Information_Group_1.Model;

/// <summary>
/// This class represents an appointment in the system
/// @author Matthew Shirley
/// @version 2026, Sprint 2
/// </summary>
public class Appointment
{
    /// <summary>
    /// Gets the appointment identifier.
    /// </summary>
    /// <value>
    /// The appointment identifier.
    /// </value>
    public int AppointmentId { get; set; }

    /// <summary>
    /// Gets or sets the doctor identifier.
    /// </summary>
    /// <value>
    /// The doctor identifier.
    /// </value>
    public int DoctorId { get; set; }

    /// <summary>
    /// Gets or sets the patient identifier.
    /// </summary>
    /// <value>
    /// The patient identifier.
    /// </value>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the reason.
    /// </summary>
    /// <value>
    /// The reason.
    /// </value>
    public string Reason { get; set; }

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>
    /// The date.
    /// </value>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the doctor.
    /// </summary>
    /// <value>
    /// The doctor.
    /// </value>
    public Doctor Doctor { get; set; }

    /// <summary>
    /// Gets or sets the patient.
    /// </summary>
    /// <value>
    /// The patient.
    /// </value>
    public Patient Patient { get; set; }


    public Visit? Visit { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Appointment"/> class.
    /// </summary>
    /// <param name="appointmentId">The appointment identifier.</param>
    /// <param name="doctorId">The doctor identifier.</param>
    /// <param name="patientId">The patient identifier.</param>
    /// <param name="reason">The reason.</param>
    /// <param name="date">The date.</param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// appointmentId cannot be negative
    /// or
    /// doctorId Cannot be negative
    /// </exception>
    public Appointment(int appointmentId, int doctorId, int patientId, string reason, DateTime date)
    {
        if (appointmentId < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(appointmentId),"appointment id cannot be negative");
        }

        if (doctorId < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(doctorId),"doctorId Cannot be negative");
        }

        if (patientId < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(patientId), "appointmentId cannot be negative");
        }

        this.AppointmentId = appointmentId;
        this.DoctorId = doctorId;
        this.PatientId = patientId;
        this.Reason = reason;
        this.Date = date;
    }

    public Appointment() {}
}