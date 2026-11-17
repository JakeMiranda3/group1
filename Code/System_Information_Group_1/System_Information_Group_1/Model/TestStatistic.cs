namespace System_Information_Group_1.Model;

/// <summary>
/// The test statistics class representing various test information.
/// </summary>
public class TestStatistic
{
    /// <summary>
    /// The test code.
    /// </summary>
    public int TestCode { get; set; }

    /// <summary>
    /// The test name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The number of test performed of the specified type.
    /// </summary>
    public int SpecificTestCount { get; set; }

    /// <summary>
    /// Total test performed across all patients.
    /// </summary>
    public int TotalTests { get; set; }

    /// <summary>
    /// The percentage of performed test of this type.
    /// </summary>
    public decimal Percent { get; set; }

    /// <summary>
    /// The normal number of test results.
    /// </summary>
    public int NormalCount { get; set; }

    /// <summary>
    /// The abnormal number of test results.
    /// </summary>
    public int AbnormalCount { get; set; }
}