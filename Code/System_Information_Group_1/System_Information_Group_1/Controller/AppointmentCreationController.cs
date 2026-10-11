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
    public AppointmentCreationController()
    {
        this.doctorDal = new DoctorDal();
        this.appointmentDal = new AppointmentDal();
        this.personDal = new PersonDal();
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
}