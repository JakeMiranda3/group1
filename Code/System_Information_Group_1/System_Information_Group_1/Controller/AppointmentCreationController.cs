using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

/// <summary>
/// The controller for the appointment creation page.
/// @author Matthew Shirley
/// @version 2026
/// </summary>
public class AppointmentCreationController
{
    private readonly DoctorDal doctorDal;
    private readonly AppointmentDal appointmentDal;
    private readonly PersonDal personDal;

    /// <summary>
    /// Gets the selected patient identifier.
    /// </summary>
    /// <value>
    /// The selected patient identifier.
    /// </value>
    public int SelectedPatientId { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentCreationController"/> class.
    /// </summary>
    /// <param name="patiendId">The patiend identifier.</param>
    public AppointmentCreationController(int patiendId)
    {
        this.doctorDal = new DoctorDal();
        this.appointmentDal = new AppointmentDal();
        this.personDal = new PersonDal();
        this.SelectedPatientId = 1;
        this.SelectedPatientId = patiendId;
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

    public int GetAppointmentId(int patientId, DateTime date)
    {
        return this.appointmentDal.GetAppointmentIdByPatientIdWithDate(patientId, date);
    }
}