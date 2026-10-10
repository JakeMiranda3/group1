using System;
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



        return reader.Read()
            ? createPerson(reader, personIdOrdinal, firstNameOrdinal, lastNameOrdinal, dateOfBirthOrdinal,
                contactPhoneNumberOrdinal, addressOrdinal, zipOrdinal, cityOrdinal, stateOrdinal)
            : throw new ArgumentNullException(nameof(reader), "reader returned no results");
    }

    private static Person createPerson(MySqlDataReader reader, int personIdOrdinal, int firstNameOrdinal,
        int lastNameOrdinal, int dateOfBirthOrdinal,
        int contractPhoneNumberOrdinal, int addressOrdinal, int zipOrdinal, int cityOrdinal, int stateOrdinal)
    {
        return new Person(
            reader.GetFieldValueCheckNull<int>(personIdOrdinal),
            reader.GetFieldValueCheckNull<string>(lastNameOrdinal),
            reader.GetFieldValueCheckNull<string>(firstNameOrdinal),
            reader.GetFieldValueCheckNull<DateTime>(dateOfBirthOrdinal),
            reader.GetFieldValueCheckNull<string>(contractPhoneNumberOrdinal),
            reader.GetFieldValueCheckNull<string>(addressOrdinal),
            reader.GetFieldValueCheckNull<string>(zipOrdinal),
            reader.GetFieldValueCheckNull<string>(cityOrdinal),
            reader.GetFieldValueCheckNull<string>(stateOrdinal)
        );
    }

    #endregion
}