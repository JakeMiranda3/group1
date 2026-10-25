using System;
using System.Collections.Generic;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

/// <summary>
///     Class for accessing nurse data from the database.
/// </summary>
public class NurseDal
{
    #region Methods

    /// <summary>
    ///     Gets a nurse using the person identifier.
    /// </summary>
    public Nurse GetNurseWithPersonId(int id)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "select * from nurse where person_id = @id;";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

        Nurse nurse;

        using (var reader = command.ExecuteReader())
        {
            var nurseIdOrdinal = reader.GetOrdinal("nurse_id");
            var personIdOrdinal = reader.GetOrdinal("person_id");

            if (!reader.Read())
            {
                throw new InvalidOperationException("Nurse not found.");
            }

            nurse = readNurse(reader, nurseIdOrdinal, personIdOrdinal);
        }

        var personDal = new PersonDal();
        nurse.Person = personDal.GetPersonWithId(nurse.PersonId);

        return nurse;
    }

    /// <summary>
    ///     Gets a nurse using the nurse identifier.
    /// </summary>
    public Nurse GetNurseWithNurseId(int id)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "select * from nurse where nurse_id = @id;";

        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

        Nurse nurse;

        using (var reader = command.ExecuteReader())
        {
            var nurseIdOrdinal = reader.GetOrdinal("nurse_id");
            var personIdOrdinal = reader.GetOrdinal("person_id");

            if (!reader.Read())
            {
                throw new InvalidOperationException("Nurse not found.");
            }

            nurse = readNurse(reader, nurseIdOrdinal, personIdOrdinal);
        }

        var personDal = new PersonDal();
        nurse.Person = personDal.GetPersonWithId(nurse.PersonId);

        return nurse;
    }

    /// <summary>
    ///     Creates a nurse associated with the specified person.
    /// </summary>
    public Nurse CreateNurse(int personId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "insert into nurse (person_id) values (@personId);";

        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.Add("@personId", MySqlDbType.Int32).Value = personId;
            command.ExecuteNonQuery();
        }

        using var idCommand = new MySqlCommand("select last_insert_id();", connection);
        var nurseId = Convert.ToInt32(idCommand.ExecuteScalar());

        var nurse = new Nurse(personId, nurseId);

        var personDal = new PersonDal();
        nurse.Person = personDal.GetPersonWithId(personId);

        return nurse;
    }

    /// <summary>
    ///     Searches nurses using the provided search criteria.
    /// </summary>
    public List<Nurse> SearchNurses(string firstName, string lastName, DateTime? dateOfBirth)
    {
        var nurses = new List<Nurse>();

        if (string.IsNullOrWhiteSpace(firstName) &&
            string.IsNullOrWhiteSpace(lastName) &&
            !dateOfBirth.HasValue)
        {
            return nurses;
        }

        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query =
            "select nurse.* from nurse " +
            "inner join person on nurse.person_id = person.person_id " +
            "where 1=1";

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
            command.Parameters.Add("@firstName", MySqlDbType.VarChar)
                    .Value = $"{firstName.Trim()}%";
        }

        if (!string.IsNullOrWhiteSpace(lastName))
        {
            command.Parameters.Add("@lastName", MySqlDbType.VarChar)
                    .Value = $"{lastName.Trim()}%";
        }

        if (dateOfBirth.HasValue)
        {
            command.Parameters.Add("@dateOfBirth", MySqlDbType.Date)
                .Value = dateOfBirth.Value.Date;
        }

        using var reader = command.ExecuteReader();

        var nurseIdOrdinal = reader.GetOrdinal("nurse_id");
        var personIdOrdinal = reader.GetOrdinal("person_id");

        var personDal = new PersonDal();

        while (reader.Read())
        {
            var nurse = readNurse(reader, nurseIdOrdinal, personIdOrdinal);

            nurse.Person = personDal.GetPersonWithId(nurse.PersonId);

            nurses.Add(nurse);
        }

        return nurses;
    }

    /// <summary>
    ///     Updates the person identifier for an existing nurse.
    /// </summary>
    public bool UpdateNursePersonId(int nurseId, int newPersonId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query =
            "update nurse set person_id = @personId where nurse_id = @nurseId;";

        using var command = new MySqlCommand(query, connection);

        command.Parameters.Add("@personId", MySqlDbType.Int32).Value = newPersonId;
        command.Parameters.Add("@nurseId", MySqlDbType.Int32).Value = nurseId;

        return command.ExecuteNonQuery() > 0;
    }

    /// <summary>
    ///     Creates a Nurse object from a database reader.
    /// </summary>
    private static Nurse readNurse(MySqlDataReader reader,
        int nurseIdOrdinal, int personIdOrdinal)
    {
        return new Nurse(
            reader.GetFieldValueCheckNull<int>(personIdOrdinal),
            reader.GetFieldValueCheckNull<int>(nurseIdOrdinal)
        );
    }

    #endregion
}