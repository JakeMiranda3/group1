using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System_Information_Group_1.Model;

/// <summary>
///     A test order combined with the details of the ordered test, used for display.
/// </summary>
public class TestOrderDisplay : TestOrder, INotifyPropertyChanged
{
    /// <summary>
    ///     Gets the name of the ordered test.
    /// </summary>
    /// <value>
    ///     The test name.
    /// </value>
    public string Name { get; private set; }

    /// <summary>
    ///     Gets the low end of the test's normal range.
    /// </summary>
    /// <value>
    ///     The low value.
    /// </value>
    public double LowValue { get; private set; }

    /// <summary>
    ///     Gets the high end of the test's normal range.
    /// </summary>
    /// <value>
    ///     The high value.
    /// </value>
    public double HighValue { get; private set; }

    /// <summary>
    ///     Gets the unit the test result is measured in.
    /// </summary>
    /// <value>
    ///     The measurement type.
    /// </value>
    public string MeasurementType { get; private set; }

    /// <summary>
    ///     Gets the normal range for display, e.g. "70 - 99 mg/dL". Empty for tests without a numeric range.
    /// </summary>
    public string NormalRange =>
        this.LowValue == 0 && this.HighValue == 0
            ? string.Empty
            : $"{this.LowValue:0.##} - {this.HighValue:0.##} {this.MeasurementType}".TrimEnd();

    /// <summary>
    ///     Initializes a new instance of the <see cref="TestOrderDisplay" /> class.
    /// </summary>
    /// <param name="appointmentId">The appointment identifier.</param>
    /// <param name="testCode">The test code.</param>
    /// <param name="name">The test name.</param>
    /// <param name="lowValue">The low end of the normal range.</param>
    /// <param name="highValue">The high end of the normal range.</param>
    /// <param name="measurementType">The unit the result is measured in.</param>
    /// <param name="performedDatetime">The date the test was performed.</param>
    /// <param name="testResult">The test result.</param>
    /// <param name="isAbnormal">if set to <c>true</c> the result is abnormal.</param>
    /// <param name="testResultDatetime">The time that the test result was entered.</param>
    public TestOrderDisplay(int appointmentId, int testCode, string name, double lowValue, double highValue,
        string measurementType, DateTime? performedDatetime, string testResult, bool isAbnormal,
        DateTime? testResultDatetime)
        : base(appointmentId, testCode, performedDatetime, testResult, isAbnormal, testResultDatetime)
    {
        this.Name = name;
        this.LowValue = lowValue;
        this.HighValue = highValue;
        this.MeasurementType = measurementType;
    }

    /// <summary>
    ///     Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    ///     Raises the <see cref="PropertyChanged" /> event.
    /// </summary>
    /// <param name="propertyName">Name of the property that changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    ///     Sets the field to the given value and raises <see cref="PropertyChanged" /> if the value changed.
    /// </summary>
    /// <typeparam name="T">The type of the field.</typeparam>
    /// <param name="field">The backing field.</param>
    /// <param name="value">The new value.</param>
    /// <param name="propertyName">Name of the property being set.</param>
    /// <returns><c>true</c> if the value changed; otherwise, <c>false</c>.</returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        this.OnPropertyChanged(propertyName);
        return true;
    }
}
