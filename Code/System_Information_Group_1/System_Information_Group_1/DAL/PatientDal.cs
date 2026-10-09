using System.Collections.Generic;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

/// <summary>
/// Class for accessing patient data from the database.
/// </summary>
public class PatientDal
{
    #region Methods

    /// <summary>
    /// Gets a list of patients from the database based on the provided person ID.
    /// </summary>
    /// <param name="personId"></param>
    /// <returns>A list of Patients</returns>
    public List<Patient> GetPatientsFromReaderWithPersonId(int personId)
    {
        var patientList = new List<Patient>();
        using var connection = new MySqlConnection(Connection.ConnectionString());

        connection.Open();
        var query = "select * from person, patient where person_id = @person_id;";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@person_id", MySqlDbType.Int32);
        command.Parameters["@person_id"].Value = personId;

        using var reader = command.ExecuteReader();
        var personIdOrdinal = reader.GetOrdinal("person_id");
        var patientIdOrdinal = reader.GetOrdinal("patient_id");
        var isActiveOrdinal = reader.GetOrdinal("is_active");

        while (reader.Read())
        {
            patientList.Add(this.createPatient(reader, personIdOrdinal, patientIdOrdinal, isActiveOrdinal));
        }

        return patientList;
    }

    /// <summary>
    /// Gets a list of patients with the patientId
    /// </summary>
    /// <param name="patientId"></param>
    /// <returns></returns>
    public List<Patient> GetPatientsFromReaderWithPatientId(int patientId)
    {
        var patientList = new List<Patient>();
        using var connection = new MySqlConnection(Connection.ConnectionString());

        connection.Open();
        var query = "select * from patient where patient_id = @patient_id;";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@patient_id", MySqlDbType.Int32);
        command.Parameters["@patient_id"].Value = patientId;

        using var reader = command.ExecuteReader();
        var personIdOrdinal = reader.GetOrdinal("person_id");
        var patientIdOrdinal = reader.GetOrdinal("patient_id");
        var isActiveOrdinal = reader.GetOrdinal("is_active");

        while (reader.Read())
        {
            patientList.Add(this.createPatient(reader, personIdOrdinal, patientIdOrdinal, isActiveOrdinal));
        }

        return patientList;
    }

    public Patient createPatient(MySqlDataReader reader, int personIdOrdinal, int patientIdOrdinal, int isActiveOrdinal)
    {
        return new Patient(
            reader.GetFieldValueCheckNull<int>(personIdOrdinal),
            reader.GetFieldValueCheckNull<int>(patientIdOrdinal),
            reader.GetFieldValueCheckNull<bool>(isActiveOrdinal));
    }

    #endregion
}