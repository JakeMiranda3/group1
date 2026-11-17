using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

/// <summary>
/// The statistics data access layer that retrieves lab test statistics from the DB.
/// </summary>
public class StatisticsDal
{
    /// <summary>
    /// Gets statistics about test orders.
    /// </summary>
    /// <param name="startDate">The start of the date range.</param>
    /// <param name="endDate">The end of the date range.</param>
    /// <returns>list of test statistics</returns>
    public IList<TestStatistic> GetTestStatistics(DateTime startDate, DateTime endDate)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var result = connection.Query<TestStatistic>("get_most_performed_tests_during_dates",new {start_date = startDate, end_date = endDate},
            commandType: CommandType.StoredProcedure);
        return result.ToList();
    }
}