using System;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;
/// <summary>
/// The doctor data access layer (DAL) class provides methods to interact with the doctor data in the database. It allows retrieving doctor information based on person ID or doctor ID.
/// </summary>
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
    /// <summary>
    /// Gets the doctor with person identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The doctor with the specified person ID</returns>
    public Doctor GetDoctorWithPersonId(int id)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select * from doctor where person_id = @id;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
        using var reader = command.ExecuteReader();
        var doctorIdOrdinal = reader.GetOrdinal("doctor_id");
        var personIdOrdinal = reader.GetOrdinal("person_id");
        return reader.Read() ? createDoctor(reader, doctorIdOrdinal, personIdOrdinal) : throw new ArgumentNullException(nameof(reader), "reader returned no results");
    }
    /// <summary>
    /// Gets the doctor with doctor identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Get the doctor with the specified doctor ID</returns>
    public Doctor GetDoctorWithDoctorId(int id)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select * from doctor where doctor_id = @id;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
        using var reader = command.ExecuteReader();
        var doctorIdOrdinal = reader.GetOrdinal("doctor_id");
        var personIdOrdinal = reader.GetOrdinal("person_id");
        return reader.Read() ? createDoctor(reader, doctorIdOrdinal, personIdOrdinal) : throw new ArgumentNullException(nameof(reader), "reader returned no results");
    }

    #endregion
}