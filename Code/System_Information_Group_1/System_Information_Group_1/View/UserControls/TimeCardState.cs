namespace System_Information_Group_1.View.UserControls;

/// <summary>
/// The state of a time slot on the schedule, which determines how its card is displayed.
/// </summary>
public enum TimeCardState
{
    /// <summary>
    /// The time slot is open and an appointment can be booked.
    /// </summary>
    Available,

    /// <summary>
    /// The appointment time has passed.
    /// </summary>
    PastDeadline,

    /// <summary>
    /// The appointment is still upcoming and can be edited.
    /// </summary>
    BeforeDeadline,

    /// <summary>
    /// Tests have been ordered for the appointment.
    /// </summary>
    PendingLabs,

    /// <summary>
    /// The appointment's visit has been finalized.
    /// </summary>
    Finalized
}