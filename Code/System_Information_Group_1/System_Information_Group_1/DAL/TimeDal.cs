using MySqlConnector;
using System;
using Dapper;

namespace System_Information_Group_1.DAL;

/// <summary>
/// The time data access layer that reads the current time from the DB server.
/// </summary>
public class TimeDal
{
    /// <summary>
    /// Gets the current date and time of the DB server in UTC.
    /// </summary>
    /// <returns>The server's current UTC date and time.</returns>
    public DateTime GetServerDateTime()
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select UTC_TIMESTAMP()";

        var serverTime = connection.QuerySingle<DateTime>(query);
        return serverTime;
    }

    /// <summary>
    /// Gets the current date and time of the DB server converted to Eastern time.
    /// </summary>
    /// <returns>The server's current date and time in Eastern time.</returns>
    public DateTime GetServerDateTimeAsEst()
    {
        var serverTime = this.GetServerDateTime();
        var utcTime = DateTime.SpecifyKind(serverTime, DateTimeKind.Utc);
        var easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, easternTimeZone);
    }
}