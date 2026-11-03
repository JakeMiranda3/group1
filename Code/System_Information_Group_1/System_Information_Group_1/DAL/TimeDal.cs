using MySqlConnector;
using System;
using Dapper;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

public class TimeDal
{
    public DateTime GetServerDateTime()
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select UTC_TIMESTAMP()";

        var serverTime = connection.QuerySingle<DateTime>(query);
        return serverTime;
    }

    public DateTime GetServerDateTimeAsEST()
    {
        var serverTime = this.GetServerDateTime();
        var utcTime = DateTime.SpecifyKind(serverTime, DateTimeKind.Utc);
        var easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, easternTimeZone);
    }
}