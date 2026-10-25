using System.Collections;
using System.Collections.Generic;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;
using System_Information_Group_1.View.Dialog;

namespace System_Information_Group_1.Controller;

public class PatientAppointmentViewController
{
    private int patientId;
    private AppointmentDal appointmentDal;

    public PatientAppointmentViewController(int patientId)
    {
        this.patientId = patientId;
        this.appointmentDal = new AppointmentDal();
    }

    public IList<Appointment> GetPatientAppointments()
    {
        return this.appointmentDal.GetAllAppointmentsByPatientId(this.patientId);
    }
}