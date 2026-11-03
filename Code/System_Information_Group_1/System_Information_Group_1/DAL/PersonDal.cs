#nullable enable
using System;
using System.Collections.Generic;
using Dapper;
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
    ///     Gets the person with identifier.
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
        var genderOrdinal = reader.GetOrdinal("gender");

        // this should only return one person since this ID is a primary key
        if (reader.Read())
        {
            return createPerson(reader, personIdOrdinal, firstNameOrdinal, lastNameOrdinal, dateOfBirthOrdinal,
                contactPhoneNumberOrdinal, addressOrdinal, zipOrdinal, cityOrdinal, stateOrdinal, genderOrdinal);
        }

        return null;
    }

    /// <summary>
    ///     Creates a new person row and returns the created Person with generated person id.
    /// </summary>
    /// <returns>Person with generated PersonId.</returns>
    public Person CreatePerson(string lastName, string firstName, DateTime dateOfBirth,
        string contactPhoneNumber, string address, string zip, string city,
        string state, string gender = "")
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        return this.CreatePerson(lastName, firstName, dateOfBirth,
            contactPhoneNumber, address, zip, city, state, gender,
            connection, null);
    }

    /// <summary>
    ///     Creates the person.
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
    /// <param name="connection">The connection.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>Person with generated PersonId.</returns>
    public Person CreatePerson(string lastName, string firstName, DateTime dateOfBirth,
        string contactPhoneNumber, string address, string zip, string city,
        string state, string gender, MySqlConnection connection,
        MySqlTransaction? transaction)
    {
        var insertQuery =
            "INSERT INTO person (last_name, first_name, date_of_birth, contact_phone_number, address, zip, city, state, gender) " +
            "VALUES (@LastName, @FirstName, @DateOfBirth, @ContactPhoneNumber, @Address, @Zip, @City, @State, @Gender); " +
            "SELECT LAST_INSERT_ID();";

        var newId = connection.QuerySingle<int>(insertQuery, new
        {
            LastName = lastName,
            FirstName = firstName,
            DateOfBirth = dateOfBirth,
            ContactPhoneNumber = contactPhoneNumber,
            Address = address,
            Zip = zip,
            City = city,
            State = state,
            Gender = gender ?? string.Empty
        }, transaction);

        return new Person(newId, lastName, firstName, dateOfBirth,
            contactPhoneNumber, address, zip, city, state, gender);
    }

    public Person CreatePerson(Person person, MySqlConnection connection, MySqlTransaction? transaction)
    {
        return this.CreatePerson(
            person.LastName,
            person.FirstName,
            person.DateOfBirth,
            person.ContactPhoneNumber,
            person.Address,
            person.Zip,
            person.City,
            person.State,
            person.Gender ?? string.Empty,
            connection,
            transaction);
    }

    /// <summary>
    ///     Gets all persons from the person table.
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
        var genderOrdinal = reader.GetOrdinal("gender");

        while (reader.Read())
        {
            list.Add(createPerson(reader, personIdOrdinal, firstNameOrdinal, lastNameOrdinal, dateOfBirthOrdinal,
                contactPhoneNumberOrdinal, addressOrdinal, zipOrdinal, cityOrdinal, stateOrdinal, genderOrdinal));
        }

        return list;
    }

    /// <summary>
    ///     Returns which roles the person has (administrator, doctor, nurse).
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
            if (count > 0)
            {
                roles.Add("administrator");
            }
        }

        using (var cmd = new MySqlCommand("select count(*) from doctor where person_id = @id;", connection))
        {
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = personId;
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0)
            {
                roles.Add("doctor");
            }
        }

        using (var cmd = new MySqlCommand("select count(*) from nurse where person_id = @id;", connection))
        {
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = personId;
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0)
            {
                roles.Add("nurse");
            }
        }

        using (var cmd = new MySqlCommand("select count(*) from patient where person_id = @id;", connection))
        {
            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = personId;
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count > 0)
            {
                roles.Add("patient");
            }
        }

        return roles.ToArray();
    }

    public static Person createPerson(MySqlDataReader reader, int personIdOrdinal, int firstNameOrdinal,
        int lastNameOrdinal, int dateOfBirthOrdinal,
        int contactPhoneNumberOrdinal, int addressOrdinal, int zipOrdinal, int cityOrdinal, int stateOrdinal,
        int genderOrdinal)
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
            reader.GetFieldValueCheckNull<string>(stateOrdinal),
            reader.GetFieldValueCheckNull<string>(genderOrdinal)
        );
    }

    /// <summary>
    ///     Edits the person.
    /// </summary>
    /// <param name="person">The person.</param>
    public void EditPerson(Person person)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query =
            "update person set last_name = @last, first_name = @first, date_of_birth = @dob, contact_phone_number = @phone, address = @address, zip = @zip, city = @city, state = @state, gender = @gender where person_id = @id;";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = person.PersonId;
        command.Parameters.Add("@last", MySqlDbType.VarChar).Value = person.LastName;
        command.Parameters.Add("@first", MySqlDbType.VarChar).Value = person.FirstName;
        command.Parameters.Add("@dob", MySqlDbType.DateTime).Value = person.DateOfBirth;
        command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = person.ContactPhoneNumber;
        command.Parameters.Add("@address", MySqlDbType.VarChar).Value = person.Address;
        command.Parameters.Add("@zip", MySqlDbType.VarChar).Value = person.Zip;
        command.Parameters.Add("@city", MySqlDbType.VarChar).Value = person.City;
        command.Parameters.Add("@state", MySqlDbType.VarChar).Value = person.State;
        command.Parameters.Add("@gender", MySqlDbType.VarChar).Value = person.Gender ?? string.Empty;

        command.ExecuteNonQuery();
    }

    #endregion
}