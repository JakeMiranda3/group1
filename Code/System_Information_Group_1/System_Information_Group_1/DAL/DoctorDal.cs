using MySqlConnector;
using System_Information_Group_1.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace System_Information_Group_1.DAL;
/// <summary>
/// The doctor data access layer (DAL) class provides methods to interact with the doctor data in the database. It allows retrieving doctor information based on person ID or doctor ID.
/// </summary>
public class DoctorDal
{
    #region Methods

    private static Doctor createDoctorWithoutPerson(MySqlDataReader reader, int doctorIdOrdinal, int personIdOrdinal)
    {
        var personId = reader.GetFieldValueCheckNull<int>(personIdOrdinal);
        var doctor = new Doctor(
            personId,
            reader.GetFieldValueCheckNull<int>(doctorIdOrdinal)
        );

        return doctor;
    }


    /// <summary>
    /// Creates a doctor row for the given person id and returns the created Doctor with generated id.
    /// </summary>
    /// <param name="personId">Person id to assign doctor role to.</param>
    /// <returns>Created Doctor object.</returns>
    public Doctor CreateDoctor(int personId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var insert = "insert into doctor (person_id) values (@personId);";
        using (var cmd = new MySqlCommand(insert, connection))
        {
            cmd.Parameters.Add("@personId", MySqlDbType.Int32).Value = personId;
            cmd.ExecuteNonQuery();
        }

        using var idCmd = new MySqlCommand("select last_insert_id();", connection);
        var newId = System.Convert.ToInt32(idCmd.ExecuteScalar());
        return new Doctor(personId, newId);
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
        return createDoctorWithoutPerson(reader, doctorIdOrdinal, personIdOrdinal);
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
        return createDoctorWithoutPerson(reader, doctorIdOrdinal, personIdOrdinal);
    }

    public IList<Doctor> GetAllDoctors()
    {

        IList<Doctor> doctorList = new List<Doctor>();
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select d.doctor_id, d.person_id, p.last_name, p.first_name, p.date_of_birth," +
                    "p.contact_phone_number, p.address, p.zip, p.city, p.state from doctor d JOIN person p ON d.person_id = p.person_id";
        using var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

        var doctorIdOrdinal = reader.GetOrdinal("doctor_id");
        var personIdOrdinal = reader.GetOrdinal("person_id");
        var firstNameOrdinal = reader.GetOrdinal("first_name");
        var lastNameOrdinal = reader.GetOrdinal("last_name");
        var dateOfBirthOrdinal = reader.GetOrdinal("date_of_birth");
        var contactPhoneNumberOrdinal = reader.GetOrdinal("contact_phone_number");
        var addressOrdinal = reader.GetOrdinal("address");
        var zipOrdinal = reader.GetOrdinal("zip");
        var cityOrdinal = reader.GetOrdinal("city");
        var stateOrdinal = reader.GetOrdinal("state");

        while (reader.Read())
        {
            var doctor = createDoctorWithoutPerson(reader, doctorIdOrdinal, personIdOrdinal);
            doctor.Person = PersonDal.createPerson(reader, personIdOrdinal, firstNameOrdinal, lastNameOrdinal,
                dateOfBirthOrdinal, contactPhoneNumberOrdinal, addressOrdinal, zipOrdinal, cityOrdinal, stateOrdinal);
            doctorList.Add(doctor);
        }

        return doctorList;
    }

    #endregion
}