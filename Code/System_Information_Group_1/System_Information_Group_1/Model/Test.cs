namespace System_Information_Group_1.Model;

/// <summary>
///     The test class
///     @author Colby
///     @version Fall 2026
/// </summary>
public class Test
{
    #region Properties

    /// <summary>
    ///     Gets the test code.
    /// </summary>
    /// <value>
    ///     The test code.
    /// </value>
    public int TestCode { get; private set; }

    /// <summary>
    ///     Gets the name.
    /// </summary>
    /// <value>
    ///     The name.
    /// </value>
    public string Name { get; private set; }

    /// <summary>
    ///     Gets or sets the low value.
    /// </summary>
    /// <value>
    ///     The low value.
    /// </value>
    public double LowValue { get; set; }

    /// <summary>
    ///     Gets or sets the high value.
    /// </summary>
    /// <value>
    ///     The high value.
    /// </value>
    public double HighValue { get; set; }

    /// <summary>
    ///     Gets the type of the measurement.
    /// </summary>
    /// <value>
    ///     The type of the measurement.
    /// </value>
    public string MeasurementType { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="Test" /> class.
    /// </summary>
    /// <param name="testCode">The test code.</param>
    /// <param name="name">The name.</param>
    /// <param name="lowValue">The low value.</param>
    /// <param name="highValue">The high value.</param>
    /// <param name="measurementType">Type of the measurement.</param>
    public Test(int testCode, string name, double lowValue, double highValue, string measurementType)
    {
        this.TestCode = testCode;
        this.Name = name;
        this.LowValue = lowValue;
        this.HighValue = highValue;
        this.MeasurementType = measurementType;
    }

    #endregion
}