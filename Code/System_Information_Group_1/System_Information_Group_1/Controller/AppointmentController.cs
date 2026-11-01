
using System;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

/// <summary>
///     Controller class for managing appointment related operations,
///     including editing appointment information.
/// </summary>
public class AppointmentController
{
    #region Fields

    private readonly AppointmentDal appointmentDal = new();

    #endregion

    #region Methods

    /// <summary>
    ///     Edits the appointment.
    /// </summary>
    /// <param name="appointment">The appointment.</param>
    /// <exception cref="ArgumentException">
    ///     Invalid appointment.
    ///     or
    ///     Invalid appointment date.
    ///     or
    ///     Appointment reason cannot be empty.
    /// </exception>
    public void EditAppointment(Appointment appointment)
    {
        if (appointment == null)
        {
            throw new ArgumentException("Invalid appointment.");
        }

        appointment.Reason = appointment.Reason?.Trim() ?? "";

        this.checkFields(appointment);

        this.appointmentDal.EditAppointment(appointment);
    }

    /// <summary>
    ///     Validates the appointment fields.
    /// </summary>
    /// <param name="appointment">The appointment.</param>
    private void checkFields(Appointment appointment)
    {
        if (appointment.AppointmentId <= 0)
        {
            throw new ArgumentException("Invalid appointment ID.");
        }

        if (appointment.DoctorId <= 0)
        {
            throw new ArgumentException("Invalid doctor ID.");
        }

        if (appointment.PatientId <= 0)
        {
            throw new ArgumentException("Invalid patient ID.");
        }

        if (string.IsNullOrWhiteSpace(appointment.Reason))
        {
            throw new ArgumentException(
                "Appointment reason cannot be empty.");
        }

        if (appointment.Date == default)
        {
            throw new ArgumentException("Invalid appointment date.");
        }
    }

    #endregion
}
