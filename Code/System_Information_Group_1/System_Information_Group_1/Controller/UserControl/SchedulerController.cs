using System;
using System.Collections.Generic;
using System.Linq;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;
using System_Information_Group_1.View.UserControls;

namespace System_Information_Group_1.Controller.UserControl;

public class SchedulerController
{
    private readonly VisitDal visitDal = new();
    private readonly TimeDal timeDal = new();

    private readonly TestOrderDal orderDal = new();


    public DateTime GetCurrentTime()
    {
        return this.timeDal.GetServerDateTimeAsEST();
    }

    public Dictionary<int, TimeCardState> DetermineCardStates(IList<Appointment> appointments)
    {
        if (appointments.Count == 0)
        {
            return new Dictionary<int, TimeCardState>();
        }

        var appointmentIds = appointments.Select(app => app.AppointmentId).Distinct().ToList();

        var finalizedIds = this.visitDal.GetFinalizedAppointmentIds(appointmentIds).ToHashSet();

        var orderIds = this.orderDal.GetAppointmentIdsWithOrders(appointmentIds).ToHashSet();

        var currentTime = this.timeDal.GetServerDateTimeAsEST();

        var states = new Dictionary<int, TimeCardState>();

        foreach (var appointment in appointments)
        {
            if (finalizedIds.Contains(appointment.AppointmentId))
            {
                states[appointment.AppointmentId] = TimeCardState.Finalized;
            } else if (orderIds.Contains(appointment.AppointmentId))
            {
                states[appointment.AppointmentId] = TimeCardState.PendingLabs;
            } else if (appointment.Date <= currentTime)
            {
                states[appointment.AppointmentId] = TimeCardState.PastDeadline;
            }
            else
            {
                states[appointment.AppointmentId] = TimeCardState.BeforeDeadline;
            }
        }

        return states;
    }
}