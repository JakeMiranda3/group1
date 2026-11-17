#nullable enable
using System;
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
    public Person? GetPersonWithId(int id)
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

        if (reader.Read())
        {
            return MapReaderToPerson(reader, personIdOrdinal, firstNameOrdinal, lastNameOrdinal, dateOfBirthOrdinal,
                contactPhoneNumberOrdinal, addressOrdinal, zipOrdinal, cityOrdinal, stateOrdinal, genderOrdinal);
        }

        return null;
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

    /// <summary>
    ///     Creates the person.
    /// </summary>
    /// <param name="person">The person.</param>
    /// <param name="connection">The connection.</param>
    /// <param name="transaction">The transaction.</param>
    /// <returns>The created person.</returns>
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
    ///     Maps the reader to person.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="personIdOrdinal">The person identifier ordinal.</param>
    /// <param name="firstNameOrdinal">The first name ordinal.</param>
    /// <param name="lastNameOrdinal">The last name ordinal.</param>
    /// <param name="dateOfBirthOrdinal">The date of birth ordinal.</param>
    /// <param name="contactPhoneNumberOrdinal">The contact phone number ordinal.</param>
    /// <param name="addressOrdinal">The address ordinal.</param>
    /// <param name="zipOrdinal">The zip ordinal.</param>
    /// <param name="cityOrdinal">The city ordinal.</param>
    /// <param name="stateOrdinal">The state ordinal.</param>
    /// <param name="genderOrdinal">The gender ordinal.</param>
    /// <returns>The mapped person.</returns>
    public static Person MapReaderToPerson(MySqlDataReader reader, int personIdOrdinal, int firstNameOrdinal,
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