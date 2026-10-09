using System;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

/// <summary>
///     Class for accessing patient data from the database.
/// </summary>
public class PatientDal
{
    #region Methods

    /// <summary>
    ///     Gets the patient from reader with person identifier.
    /// </summary>
    /// <param name="personId">The person identifier.</param>
    /// <returns>The patient.</returns>
    /// <exception cref="System.InvalidOperationException">Patient not found.</exception>
    public Patient GetPatientFromReaderWithPersonId(int personId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());

        connection.Open();
        var query = "select * from patient where person_id = @person_id;";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@person_id", MySqlDbType.Int32);
        command.Parameters["@person_id"].Value = personId;

        using var reader = command.ExecuteReader();
        var personIdOrdinal = reader.GetOrdinal("person_id");
        var patientIdOrdinal = reader.GetOrdinal("patient_id");
        var isActiveOrdinal = reader.GetOrdinal("is_active");

        if (!reader.Read())
        {
            throw new InvalidOperationException("Patient not found.");
        }

        var patient = readPatient(reader, personIdOrdinal, patientIdOrdinal, isActiveOrdinal);
        var personDal = new PersonDal();
        patient.Person = personDal.GetPersonWithId(patient.PersonId);

        return patient;
    }

    /// <summary>
    ///     Creates the patient.
    /// </summary>
    /// <param name="personId">The person identifier.</param>
    /// <returns>The created patient.</returns>
    public Patient CreatePatient(int personId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var insert = "insert into patient (person_id) values (@personId);";
        using (var cmd = new MySqlCommand(insert, connection))
        {
            cmd.Parameters.Add("@personId", MySqlDbType.Int32).Value = personId;
            cmd.ExecuteNonQuery();
        }

        using var idCmd = new MySqlCommand("select last_insert_id();", connection);
        var newId = Convert.ToInt32(idCmd.ExecuteScalar());

        var patient = new Patient(personId, newId, true);
        var personDal = new PersonDal();
        patient.Person = personDal.GetPersonWithId(patient.PersonId);

        return patient;
    }

    private static Patient readPatient(MySqlDataReader reader, int personIdOrdinal, int patientIdOrdinal,
        int isActiveOrdinal)
    {
        return new Patient(
            reader.GetFieldValueCheckNull<int>(personIdOrdinal),
            reader.GetFieldValueCheckNull<int>(patientIdOrdinal),
            reader.GetFieldValueCheckNull<bool>(isActiveOrdinal));
    }

    #endregion
}