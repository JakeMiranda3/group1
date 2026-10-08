using DBAccess.DAL;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

public class DoctorDal
{
    #region Methods

    private static Doctor createDoctor(MySqlDataReader reader, int doctorIdOrdinal, int personIdOrdinal)
    {
        return new Doctor(
            reader.GetFieldValueCheckNull<int>(personIdOrdinal),
            reader.GetFieldValueCheckNull<int>(doctorIdOrdinal)
        );
    }

    #endregion

    #region access methods

    public Doctor getDoctorWithPersonId(int id)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select * from doctor where person_id = @id;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
        using var reader = command.ExecuteReader();
        var doctorIdOrdinal = reader.GetOrdinal("doctor_id");
        var personIdOrdinal = reader.GetOrdinal("person_id");
        return createDoctor(reader, doctorIdOrdinal, personIdOrdinal);
    }

    public Doctor getDoctorWithDoctorId(int id)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select * from doctor where doctor_id = @id;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
        using var reader = command.ExecuteReader();
        var doctorIdOrdinal = reader.GetOrdinal("doctor_id");
        var personIdOrdinal = reader.GetOrdinal("person_id");
        return createDoctor(reader, doctorIdOrdinal, personIdOrdinal);
    }

    #endregion
}