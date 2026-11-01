using System;

namespace System_Information_Group_1.Model;

/// <summary>
///     The test order class
///     @author Colby
///     @version Fall 2026
/// </summary>
public class TestOrder
{
    #region Properties

    /// <summary>
    ///     Gets the appointment identifier.
    /// </summary>
    /// <value>
    ///     The appointment identifier.
    /// </value>
    public int AppointmentId { get; private set; }

    /// <summary>
    ///     Gets the test code.
    /// </summary>
    /// <value>
    ///     The test code.
    /// </value>
    public int TestCode { get; private set; }

    /// <summary>
    ///     Gets or sets the performed datetime.
    /// </summary>
    /// <value>
    ///     The performed datetime.
    /// </value>
    public DateTime PerformedDatetime { get; set; }

    /// <summary>
    ///     Gets or sets the test result.
    /// </summary>
    /// <value>
    ///     The test result.
    /// </value>
    public string TestResult { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether this instance is abnormal.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance is abnormal; otherwise, <c>false</c>.
    /// </value>
    public bool IsAbnormal { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="TestOrder" /> class.
    /// </summary>
    /// <param name="appointmentId">The appointment identifier.</param>
    /// <param name="testCode">The test code.</param>
    /// <param name="performedDatetime">The performed datetime.</param>
    /// <param name="testResult">The test result.</param>
    /// <param name="isAbnormal">if set to <c>true</c> [is abnormal].</param>
    public TestOrder(int appointmentId, int testCode, DateTime performedDatetime, string testResult, bool isAbnormal)
    {
        this.AppointmentId = appointmentId;
        this.TestCode = testCode;
        this.PerformedDatetime = performedDatetime;
        this.TestResult = testResult;
        this.IsAbnormal = isAbnormal;
    }

    #endregion
}