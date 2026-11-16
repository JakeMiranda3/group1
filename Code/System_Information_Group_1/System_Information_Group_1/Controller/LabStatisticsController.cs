using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

public class LabStatisticsController
{
    private readonly StatisticsDal statisticsDal;

    public LabStatisticsController()
    {
        this.statisticsDal = new StatisticsDal();
    }

    public IList<TestStatistic> GetStatistics(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date cannot be after end date");
        }

        return this.statisticsDal.GetTestStatistics(startDate, endDate);
    }
}