using DBAccess.DAL;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

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

    public Administrator getAdministratorWithPersonId(int id)
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

    public Administrator getAdministratorWithAdministratorId(int id)
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