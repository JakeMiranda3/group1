using System.Collections.Generic;
using System.Windows.Documents;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

public class AppointmentCreationController
{
    private readonly DoctorDal doctorDal;
    public AppointmentCreationController()
    {
        this.doctorDal = new DoctorDal();
    }

    public IList<Doctor> getAvailableDoctors()
    {
        return this.doctorDal.GetAllDoctors();
    }
}