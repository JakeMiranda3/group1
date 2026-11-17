using System;
using System.Collections.Generic;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

/// <summary>
/// The lab statistic page controller.
/// </summary>
public class LabStatisticsController
{
    private readonly StatisticsDal statisticsDal;

    /// <summary>
    /// Initializes a new statistic controller.
    /// </summary>
    public LabStatisticsController()
    {
        this.statisticsDal = new StatisticsDal();
    }

    /// <summary>
    /// Gets test statistics between the given dates.
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns>A list of test statistics</returns>
    /// <exception cref="ArgumentException"></exception>
    public IList<TestStatistic> GetStatistics(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new ArgumentException("Start date cannot be after end date");
        }

        return this.statisticsDal.GetTestStatistics(startDate, endDate);
    }
}