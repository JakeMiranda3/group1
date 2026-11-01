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
        var query = "select NOW()";

        var serverTime = connection.QuerySingle<DateTime>(query);
        return serverTime;
    }
}