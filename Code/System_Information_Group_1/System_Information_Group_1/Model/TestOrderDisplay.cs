using System;

namespace System_Information_Group_1.Model;

public class TestOrderDisplay
{
    public int AppointmentId { get; private set; }
    public int TestCode { get; private set; }
    public string Name { get; private set; }
    public double LowValue { get; private set; }
    public double HighValue { get; private set; }
    public string MeasurementType { get; private set; }
    public DateTime? PerformedDateTime { get; set; }
    public string TestResult { get; set; }
    public bool IsAbnormal { get; set; }

    public TestOrderDisplay(int appointmentId, int testCode, string name, double lowValue, double highValue,
        string measurementType, DateTime? performedDateTime, string testResult, bool isAbnormal)
    {
        this.AppointmentId = appointmentId;
        this.TestCode = testCode;
        this.Name = name;
        this.LowValue = lowValue;
        this.HighValue = highValue;
        this.MeasurementType = measurementType;
        this.PerformedDateTime = performedDateTime;
        this.TestResult = testResult;
        this.IsAbnormal = isAbnormal;
    }
}
