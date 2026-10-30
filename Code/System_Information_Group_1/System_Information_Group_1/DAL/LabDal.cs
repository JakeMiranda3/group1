using MySqlConnector;
using System;
using Dapper;

namespace System_Information_Group_1.DAL;

public class LabDal
{
    internal bool DoesAppointmentHaveAnyLabs(int appointmentId)
    {
    
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "select exists (select 1 from test_order where appointment_id = @id)";

        return connection.ExecuteScalar<bool>(query, new {id = appointmentId});
}
}