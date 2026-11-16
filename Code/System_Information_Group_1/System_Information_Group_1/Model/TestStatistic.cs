namespace System_Information_Group_1.Model;

public class TestStatistic
{
    public int TestCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SpecificTestCount { get; set; }
    public int TotalTests { get; set; }
    public decimal Percent { get; set; }
    public int NormalCount { get; set; }
    public int AbnormalCount { get; set; }
}