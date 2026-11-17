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
    public string Name { get; private set; }
    public double LowValue { get; private set; }
    public double HighValue { get; private set; }
    public string MeasurementType { get; private set; }

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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
