


using System;

namespace System_Information_Group_1.Model;

public class Appointment
{
    public int AppointmentId { get; private set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public string Reason { get; set; }
    public DateTime Date { get; set; }
    public string doctorName { get; set; }
    public string patientName { get; set; }

    public Appointment(int appointmentId, int doctorId, int patientId, string reason, DateTime date)
    {
        if (appointmentId < 0)
        {
            throw new ArgumentOutOfRangeException("appointmentId cannot be negative");
        }

        if (doctorId < 0)
        {
            throw new ArgumentOutOfRangeException("doctorId Cannot be negative");
        }

        if (patientId < 0)
        {
            throw new ArgumentOutOfRangeException("appointmentId cannot be negative");
        }

        this.AppointmentId = appointmentId;
        this.DoctorId = doctorId;
        this.PatientId = patientId;
        this.Reason = reason;
        this.Date = date;
    }
}