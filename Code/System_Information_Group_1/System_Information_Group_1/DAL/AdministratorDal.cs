using MySqlConnector;
using System_Information_Group_1.Model;

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