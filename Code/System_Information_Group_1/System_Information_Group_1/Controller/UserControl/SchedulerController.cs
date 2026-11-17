using System;
using System.Collections.Generic;
using System.Linq;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;
using System_Information_Group_1.View.UserControls;

namespace System_Information_Group_1.Controller.UserControl;
/// <summary>
/// The scheduler controller
/// @author Matthew
/// @version Fall 2026
/// </summary>
public class SchedulerController
{
    private readonly VisitDal visitDal = new();
    private readonly TimeDal timeDal = new();

    private readonly TestOrderDal orderDal = new();

    /// <summary>
    /// Gets the current time.
    /// </summary>
    /// <returns>Current time of the SQL server</returns>
    public DateTime GetCurrentTime()
    {
        return this.timeDal.GetServerDateTimeAsEst();
    }
    /// <summary>
    /// Determines the card states.
    /// </summary>
    /// <param name="appointments">The appointments.</param>
    /// <returns>The state of each card as a dict</returns>
    public Dictionary<int, TimeCardState> DetermineCardStates(IList<Appointment> appointments)
    {
        if (appointments.Count == 0)
        {
            return new Dictionary<int, TimeCardState>();
        }

        var appointmentIds = appointments.Select(app => app.AppointmentId).Distinct().ToList();

        var finalizedIds = this.visitDal.GetFinalizedAppointmentIds(appointmentIds).ToHashSet();

        var orderIds = this.orderDal.GetAppointmentIdsWithOrders(appointmentIds).ToHashSet();

        var currentTime = this.timeDal.GetServerDateTimeAsEst();

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