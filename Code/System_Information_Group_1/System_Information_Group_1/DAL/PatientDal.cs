#nullable enable
using System;
using System.Collections.Generic;
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

        var patient = this.CreatePatient(personId, connection, null);

        var personDal = new PersonDal();
        patient.Person = personDal.GetPersonWithId(patient.PersonId);

        return patient;
    }

    /// <summary>
    /// Creates the patient.
    /// </summary>
    /// <param name="personId">The person identifier.</param>
    /// <param name="connection">The connection.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>The newly created patient.</returns>
    public Patient CreatePatient(int personId, MySqlConnection connection,
        MySqlTransaction? transaction)
    {
        var insert = "insert into patient (person_id) values (@personId);";

        using var command = new MySqlCommand(insert, connection, transaction);

        command.Parameters.Add("@personId", MySqlDbType.Int32).Value = personId;

        command.ExecuteNonQuery();

        var newId = Convert.ToInt32(command.LastInsertedId);

        return new Patient(personId, newId, true);
    }


    /// <summary>
    /// Creates the patient with person.
    /// </summary>
    /// <param name="lastName">The last name.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <param name="contactPhoneNumber">The contact phone number.</param>
    /// <param name="address">The address.</param>
    /// <param name="zip">The zip.</param>
    /// <param name="city">The city.</param>
    /// <param name="state">The state.</param>
    /// <param name="gender">The gender.</param>
    /// <returns>The newly created patient.</returns>
    public Patient CreatePatientWithPerson(string lastName, string firstName,
        DateTime dateOfBirth, string contactPhoneNumber, string address,
        string zip, string city, string state, string gender = "")
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var personDal = new PersonDal();

            var person = personDal.CreatePerson(lastName, firstName,
                dateOfBirth, contactPhoneNumber, address, zip,
                city, state, gender, connection, transaction);

            var patient = this.CreatePatient(person.PersonId, connection, transaction);

            patient.Person = person;

            transaction.Commit();

            return patient;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <summary>
    ///     Searches the patients.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <returns>List of matching patients.</returns>
    public List<Patient> SearchPatients(string firstName, string lastName, DateTime? dateOfBirth)
    {
        var patients = new List<Patient>();

        if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName) && !dateOfBirth.HasValue)
        {
            return patients;
        }

        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "select patient.* from patient inner join person on patient.person_id = person.person_id where 1=1";

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query += " and person.first_name like @firstName";
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query += " and person.last_name like @lastName";
        }

        if (dateOfBirth.HasValue)
        {
            query += " and date(person.date_of_birth) = @dateOfBirth";
        }

        query += " order by person.last_name, person.first_name;";

        using var command = new MySqlCommand(query, connection);

        if (!string.IsNullOrWhiteSpace(firstName))
        {
            command.Parameters.Add("@firstName", MySqlDbType.VarChar).Value = $"{firstName.Trim()}%";
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            command.Parameters.Add("@lastName", MySqlDbType.VarChar).Value = $"{lastName.Trim()}%";
        }

        if (dateOfBirth.HasValue)
        {
            command.Parameters.Add("@dateOfBirth", MySqlDbType.Date)
                .Value = dateOfBirth.Value.Date;
        }

        using var reader = command.ExecuteReader();

        var personIdOrdinal = reader.GetOrdinal("person_id");
        var patientIdOrdinal = reader.GetOrdinal("patient_id");
        var isActiveOrdinal = reader.GetOrdinal("is_active");

        var personDal = new PersonDal();

        while (reader.Read())
        {
            var patient = readPatient(reader, personIdOrdinal, patientIdOrdinal, isActiveOrdinal);

            patient.Person = personDal.GetPersonWithId(patient.PersonId);
            patients.Add(patient);
        }

        return patients;
    }

    /// <summary>
    ///     Edits the patient.
    /// </summary>
    /// <param name="patient">The patient.</param>
    public void EditPatient(Patient patient)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "update patient set is_active = @isActive where person_id = @personId;";
        using var command = new MySqlCommand(query, connection);

        command.Parameters.Add("@personId", MySqlDbType.Int32).Value = patient.PersonId;
        command.Parameters.Add("@isActive", MySqlDbType.Bool).Value = patient.IsActive;

        command.ExecuteNonQuery();
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