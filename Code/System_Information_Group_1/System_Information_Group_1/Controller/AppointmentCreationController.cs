using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

public class AppointmentCreationController
{
    private readonly DoctorDal doctorDal;
    private readonly AppointmentDal appointmentDal;
    private readonly PersonDal personDal;
    public int SelectedPatientId { get; private set; }
    public AppointmentCreationController(int patiendId)
    {
        this.doctorDal = new DoctorDal();
        this.appointmentDal = new AppointmentDal();
        this.personDal = new PersonDal();
        this.SelectedPatientId = 1;
        this.SelectedPatientId = patiendId;
    }

    public IList<Doctor> GetAvailableDoctors()
    {
        return this.doctorDal.GetAllDoctors();
    }

#nullable enable
    public IList<Appointment> GetDoctorsAppointments(Doctor? doctor)
    {
        if (doctor == null)
        {
            return new List<Appointment>();
        }

        return this.appointmentDal.GetAppointmentsContainingDoctorAndPatient(doctor.DoctorId);
    }

    public string getDoctorsFullName(Doctor? doctor)
    {
        if (doctor == null)
        {
            return "Doctors name is unavailable...";
        }
        return this.doctorDal.GetDoctorsFullNameByDoctorId(doctor.DoctorId);
    }

    public bool StoreAppointment(DateTime date, int doctorId, int patientId, string reason)
    {
        return this.appointmentDal.StoreNewAppointment(doctorId, patientId, date, reason);
    }
}