using MySqlConnector;
using System_Information_Group_1.Model;
using System;

namespace System_Information_Group_1.DAL;
/// <summary>
/// The data access layer in the system for the administrator table in the database. This class provides methods to retrieve administrator data based on person ID or administrator ID.
/// </summary>
public class AdministratorDal
{
    #region Methods

    private static Administrator createAdministrator(MySqlDataReader reader, int administratorIdOrdinal,
        int personIdOrdinal)
    {
        return new Administrator(
            reader.GetFieldValueCheckNull<int>(personIdOrdinal),
            reader.GetFieldValueCheckNull<int>(administratorIdOrdinal)
        );
    }

    /// <summary>
    /// Creates an administrator row for the given person id and returns the created Administrator with generated id.
    /// </summary>
    /// <param name="personId">Person id to assign administrator role to.</param>
    /// <returns>Created Administrator object.</returns>
    public Administrator CreateAdministrator(int personId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var insert = "insert into administrator (person_id) values (@personId);";
        using (var cmd = new MySqlCommand(insert, connection))
        {
            cmd.Parameters.Add("@personId", MySqlDbType.Int32).Value = personId;
            cmd.ExecuteNonQuery();
        }

        using var idCmd = new MySqlCommand("select last_insert_id();", connection);
        var newId = System.Convert.ToInt32(idCmd.ExecuteScalar());
        return new Administrator(personId, newId);
    }

    #endregion

    #region access methods    
    /// <summary>
    /// Gets the administrator with person identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>the admin with the specified person ID</returns>
    public Administrator GetAdministratorWithPersonId(int id)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select * from administrator where person_id = @id;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
        using var reader = command.ExecuteReader();
        var administratorIdOrdinal = reader.GetOrdinal("administrator_id");
        var personIdOrdinal = reader.GetOrdinal("person_id");
        return createAdministrator(reader, administratorIdOrdinal, personIdOrdinal);
    }
    /// <summary>
    /// Gets the administrator with administrator identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Get the admin with the specified admin ID</returns>
    public Administrator GetAdministratorWithAdministratorId(int id)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select * from administrator where administrator_id = @id;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
        using var reader = command.ExecuteReader();
        var administratorIdOrdinal = reader.GetOrdinal("administrator_id");
        var personIdOrdinal = reader.GetOrdinal("person_id");
        return createAdministrator(reader, administratorIdOrdinal, personIdOrdinal);
    }

    #endregion
}