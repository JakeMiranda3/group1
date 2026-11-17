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
    ///     Gets or sets the date the test was performed. Only the date is kept; any time of day is dropped.
    /// </summary>
    /// <value>
    ///     The date the test was performed.
    /// </value>
    public DateTime? PerformedDatetime
    {
        get => this.performedDatetime;
        set => this.performedDatetime = value?.Date;
    }

    /// <summary>
    /// Gets or sets the test result datetime.
    /// </summary>
    /// <value>
    /// The test result datetime.
    /// </value>
    public DateTime? TestResultDatetime { get; set; }

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

    /// <summary>
    ///     Gets a value indicating whether the test result differs from the one this order was loaded with.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the test result has been changed; otherwise, <c>false</c>.
    /// </value>
    public bool HasTestResultChanged =>
        !string.Equals(this.loadedTestResult ?? string.Empty, this.TestResult ?? string.Empty);

    #endregion

    #region Data members

    private readonly string loadedTestResult;
    private DateTime? performedDatetime;

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
    /// <param name="testResultDatetime">the time that the test result was entered</param>
    public TestOrder(int appointmentId, int testCode, DateTime? performedDatetime, string testResult, bool isAbnormal,
        DateTime? testResultDatetime)
    {
        this.AppointmentId = appointmentId;
        this.TestCode = testCode;
        this.PerformedDatetime = performedDatetime;
        this.TestResult = testResult;
        this.loadedTestResult = testResult;
        this.IsAbnormal = isAbnormal;
        this.TestResultDatetime = testResultDatetime;
    }

    public TestOrder()
    {

    }

    #endregion
}