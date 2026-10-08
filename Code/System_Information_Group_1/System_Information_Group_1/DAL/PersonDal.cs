using System;
using System.Collections.Generic;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

/// <summary>
///     Ther personal data access layer class
///     @author Colby
///     @version Fall 2026
/// </summary>
public class PersonDal
{
    #region Methods    
    /// <summary>
    /// Gets the person with identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>The person with the identifier</returns>
    public Person GetPersonWithId(int id)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "select * from person where person_id = @id;";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

        using var reader = command.ExecuteReader();
        var personIdOrdinal = reader.GetOrdinal("person_id");
        var firstNameOrdinal = reader.GetOrdinal("first_name");
        var lastNameOrdinal = reader.GetOrdinal("last_name");
        var dateOfBirthOrdinal = reader.GetOrdinal("date_of_birth");
        var contactPhoneNumberOrdinal = reader.GetOrdinal("contact_phone_number");
        var addressOrdinal = reader.GetOrdinal("address");
        var zipOrdinal = reader.GetOrdinal("zip");
        var cityOrdinal = reader.GetOrdinal("city");
        var stateOrdinal = reader.GetOrdinal("state");

        // this should only return one person since this ID is a primary key

        return createPerson(reader, personIdOrdinal, firstNameOrdinal, lastNameOrdinal, dateOfBirthOrdinal,
            contactPhoneNumberOrdinal, addressOrdinal, zipOrdinal, cityOrdinal, stateOrdinal);
    }

    /// <summary>
    /// Creates a new person row and returns the created Person with generated person id.
    /// </summary>
    /// <returns>Person with generated PersonId.</returns>
    public Person CreatePerson(string lastName, string firstName, DateTime dateOfBirth, string contactPhoneNumber, string address, string zip, string city, string state)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var insert = "insert into person (last_name, first_name, date_of_birth, contact_phone_number, address, zip, city, state) values (@last, @first, @dob, @phone, @address, @zip, @city, @state);";
        using (var command = new MySqlCommand(insert, connection))
        {
            command.Parameters.Add("@last", MySqlDbType.VarChar).Value = lastName;
            command.Parameters.Add("@first", MySqlDbType.VarChar).Value = firstName;
            command.Parameters.Add("@dob", MySqlDbType.DateTime).Value = dateOfBirth;
            command.Parameters.Add("@phone", MySqlDbType.String).Value = contactPhoneNumber;
            command.Parameters.Add("@address", MySqlDbType.VarChar).Value = address;
            command.Parameters.Add("@zip", MySqlDbType.VarChar).Value = zip;
            command.Parameters.Add("@city", MySqlDbType.VarChar).Value = city;
            command.Parameters.Add("@state", MySqlDbType.VarChar).Value = state;

            command.ExecuteNonQuery();
        }

        using var idCmd = new MySqlCommand("select last_insert_id();", connection);
        var newId = Convert.ToInt32(idCmd.ExecuteScalar());

        return new Person(newId, lastName, firstName, dateOfBirth, contactPhoneNumber, address, zip, city, state);
    }

    /// <summary>
    /// Gets all persons from the person table.
    /// </summary>
    /// <returns>List of Person</returns>
    public List<Person> GetAllPersons()
    {
        var list = new List<Person>();
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select * from person;";
        using var command = new MySqlCommand(query, connection);
        using var reader = command.ExecuteReader();

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
            list.Add(createPerson(reader, personIdOrdinal, firstNameOrdinal, lastNameOrdinal, dateOfBirthOrdinal,
                contactPhoneNumberOrdinal, addressOrdinal, zipOrdinal, cityOrdinal, stateOrdinal));
        }

        return list;
    }

    /// <summary>
    /// Returns which roles the person has (administrator, doctor, nurse).
    /// </summary>
    /// <param name="personId">person id to check.</param>
    /// <returns>Array of role names the person belongs to. Empty if none.</returns>
    public string[] GetPersonRoles(int personId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var roles = new List<string>();

        using (var cmd = new MySqlCommand("select count(*) from administrator where person_id = @id;", connection))
        {
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = personId;
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0) roles.Add("administrator");
        }

        using (var cmd = new MySqlCommand("select count(*) from doctor where person_id = @id;", connection))
        {
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = personId;
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0) roles.Add("doctor");
        }

        using (var cmd = new MySqlCommand("select count(*) from nurse where person_id = @id;", connection))
        {
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = personId;
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0) roles.Add("nurse");
        }

        return roles.ToArray();
    }

    private static Person createPerson(MySqlDataReader reader, int personIdOrdinal, int firstNameOrdinal,
        int lastNameOrdinal, int dateOfBirthOrdinal,
        int contactPhoneNumberOrdinal, int addressOrdinal, int zipOrdinal, int cityOrdinal, int stateOrdinal)
    {
        return new Person(
            reader.GetFieldValueCheckNull<int>(personIdOrdinal),
            reader.GetFieldValueCheckNull<string>(lastNameOrdinal),
            reader.GetFieldValueCheckNull<string>(firstNameOrdinal),
            reader.GetFieldValueCheckNull<DateTime>(dateOfBirthOrdinal),
            reader.GetFieldValueCheckNull<string>(contactPhoneNumberOrdinal),
            reader.GetFieldValueCheckNull<string>(addressOrdinal),
            reader.GetFieldValueCheckNull<string>(zipOrdinal),
            reader.GetFieldValueCheckNull<string>(cityOrdinal),
            reader.GetFieldValueCheckNull<string>(stateOrdinal)
        );
    }

    #endregion
}