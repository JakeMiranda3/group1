using System;
using System.Collections.Generic;
using System_Information_Group_1.Authorization;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

/// <summary>
/// The controller for the appointment creation page.
/// @author Matthew Shirley
/// @version 2026
/// </summary>
public class ViewAppointmentController
{
    private readonly DoctorDal doctorDal;
    private readonly AppointmentDal appointmentDal;
    private readonly NurseDal nurseDal;
    private readonly TimeDal timeDal;

    /// <summary>
    /// Gets the selected patient identifier.
    /// </summary>
    /// <value>
    /// The selected patient identifier.
    /// </value>
    public int SelectedPatientId { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewAppointmentController"/> class.
    /// </summary>
    /// <param name="patiendId">The patiend identifier.</param>
    public ViewAppointmentController(int patiendId)
    {
        this.doctorDal = new DoctorDal();
        this.appointmentDal = new AppointmentDal();
        this.SelectedPatientId = 1;
        this.SelectedPatientId = patiendId;
        this.timeDal = new TimeDal();
        this.nurseDal = new NurseDal();
    }

    /// <summary>
    /// Gets the available doctors.
    /// </summary>
    /// <returns>The list of available doctors.</returns>
    public IList<Doctor> GetAvailableDoctors()
    {
        return this.doctorDal.GetAllDoctors();
    }

#nullable enable    
    /// <summary>
    /// Gets the doctors appointments.
    /// </summary>
    /// <param name="doctor">The doctor.</param>
    /// <returns></returns>
    public IList<Appointment> GetDoctorsAppointments(Doctor? doctor)
    {
        if (doctor == null)
        {
            return new List<Appointment>();
        }

        return this.appointmentDal.GetAppointmentsContainingDoctorAndPatient(doctor.DoctorId);
    }
    /// <summary>
    /// Gets the current time as eastern.
    /// </summary>
    /// <returns>Time as eastern</returns>
    public DateTime GetCurrentTimeAsEastern()
    {
        var serverDate = this.timeDal.GetServerDateTime();
        DateTime serverUtcDate = DateTime.SpecifyKind(serverDate, DateTimeKind.Utc);

        TimeZoneInfo easernZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        return TimeZoneInfo.ConvertTimeFromUtc(serverUtcDate, easernZone);
    }

    /// <summary>
    /// Gets the full name of the doctors.
    /// </summary>
    /// <param name="doctor">The doctor.</param>
    /// <returns></returns>
    public string GetDoctorsFullName(Doctor? doctor)
    {
        return doctor == null
            ? "Doctors name is unavailable..."
            : this.doctorDal.GetDoctorsFullNameByDoctorId(doctor.DoctorId);
    }

    /// <summary>
    /// Stores the appointment.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <param name="doctorId">The doctor identifier.</param>
    /// <param name="patientId">The patient identifier.</param>
    /// <param name="reason">The reason.</param>
    /// <returns>True if the appointment was stored</returns>
    public bool StoreAppointment(DateTime date, int doctorId, int patientId, string reason)
    {
        return this.appointmentDal.StoreNewAppointment(doctorId, patientId, date, reason);
    }

    /// <summary>
    /// Gets the patient appointments.
    /// </summary>
    /// <returns>List of patient appointments</returns>
    public IList<Appointment> GetPatientAppointments()
    {
        return this.appointmentDal.GetAllAppointmentsByPatientId(this.SelectedPatientId);
    }
    /// <summary>
    /// Gets the visit for appointment.
    /// </summary>
    /// <param name="appointmentId">The appointment identifier.</param>
    /// <returns>The visit for an appointment</returns>
    /// <exception cref="System.InvalidOperationException">Unable to find the currently logged-in nurse.</exception>
    public Visit GetVisitForAppointment(int appointmentId)
    {
        var personId = AuthServiceSingleton.GetAccount().PersonId;

        var nurse = this.nurseDal.GetNurseWithPersonId(personId);

        if (nurse == null)
        {
            throw new InvalidOperationException(
                "Unable to find the currently logged-in nurse.");
        }

        return new VisitDisplay(appointmentId, nurse.NurseId);
    }
}