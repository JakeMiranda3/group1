using System;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;
using System_Information_Group_1.View.UserControls;

namespace System_Information_Group_1.Controller.UserControl;

public class SchedulerController
{
    private readonly VisitDal visitDal = new();
    private readonly TimeDal timeDal = new();
    private readonly LabDal labDal = new();

    public TimeCardState DetermineCardState(Appointment appointment)
    {
        if (appointment == null)
        {
            return TimeCardState.Available;
        }

        if (this.visitDal.IsVisitFinalized(appointment.AppointmentId))
        {
            return TimeCardState.Finalized;
        }

        if (this.labDal.DoesAppointmentHaveAnyLabs(appointment.AppointmentId))
        {
            return TimeCardState.PendingLabs;
        }

        if (appointment.Date <= this.timeDal.GetServerDateTimeAsEST())
        {
            return TimeCardState.PastDeadline;
        }

        return TimeCardState.BeforeDeadline;
    }
}