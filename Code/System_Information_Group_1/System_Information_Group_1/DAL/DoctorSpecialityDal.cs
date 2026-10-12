using System.Collections.Generic;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;
/// <summary>
///  The doctor speciality data access layer in the system for the doctor_speciality table in the database. This class provides methods to retrieve doctor speciality data based on person ID or speciality name.
/// </summary>
public class DoctorSpecialityDal
{
    #region Methods    
    /// <summary>
    /// Gets the doctor specialities by person identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The list of specialties the doctor with that person ID has</returns>
    public List<DoctorSpeciality> GetDoctorSpecialitiesByPersonId(int id)
    {
        var specialities = new List<DoctorSpeciality>();

        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "select * from doctor_speciality where person_id = @id;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
        using var reader = command.ExecuteReader();
        var personIdOrdinal = reader.GetOrdinal("person_id");
        var specialityNameOrdinal = reader.GetOrdinal("speciality_name");

        while (reader.Read())
        {
            specialities.Add(
                createDoctorSpeciality(reader, personIdOrdinal, specialityNameOrdinal)
            );
        }

        return specialities;
    }

    /// <summary>
    /// Gets the name of the doctor specialities by speciality.
    /// </summary>
    /// <param name="specialityName">Name of the speciality.</param>
    /// <returns>Get the doctor specialties with the specific speciality</returns>
    public List<DoctorSpeciality> GetDoctorSpecialitiesBySpecialityName(string specialityName)
    {
        var specialities = new List<DoctorSpeciality>();
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select * from doctor_speciality where speciality_name = @specialityName;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@specialityName", MySqlDbType.VarChar).Value = specialityName;
        using var reader = command.ExecuteReader();
        var personIdOrdinal = reader.GetOrdinal("person_id");
        var specialityNameOrdinal = reader.GetOrdinal("speciality_name");
        while (reader.Read())
        {
            specialities.Add(
                createDoctorSpeciality(reader, personIdOrdinal, specialityNameOrdinal)
            );
        }

        return specialities;
    }

    private static DoctorSpeciality createDoctorSpeciality(MySqlDataReader reader, int personIdOrdinal,
        int specialityNameOrdinal)
    {
        return new DoctorSpeciality(
            reader.GetFieldValueCheckNull<int>(personIdOrdinal),
            reader.GetFieldValueCheckNull<string>(specialityNameOrdinal)
        );
    }

    #endregion
}