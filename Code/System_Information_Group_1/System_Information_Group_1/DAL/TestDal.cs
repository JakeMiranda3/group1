using System.Collections.Generic;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

/// <summary>
///     The test data access layer
///     @author Colby
///     @version Fall 2026
/// </summary>
public class TestDal
{
    #region Methods

    public List<Test> GetAllTest()
    {
        var testList = new List<Test>();

        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "SELECT * FROM Test";

        using var cmd = new MySqlCommand(query, connection);

        using var reader = cmd.ExecuteReader();

        var testCodeOrdinal = reader.GetOrdinal("test_code");
        var nameOrdinal = reader.GetOrdinal("name");
        var lowValueOrdinal = reader.GetOrdinal("low_value");
        var highValueOrdinal = reader.GetOrdinal("high_value");
        var measurementTypeOrdinal = reader.GetOrdinal("measurement_type");

        while (reader.Read())
        {
            testList.Add(CreateTest(reader, testCodeOrdinal, nameOrdinal, lowValueOrdinal, highValueOrdinal,
                measurementTypeOrdinal));
        }

        return testList;
    }

    public static Test CreateTest(MySqlDataReader reader, int testCodeOrdinal, int nameOrdinal, int lowValueOrdinal,
        int highValueOrdinal, int measurementTypeOrdinal)
    {
        var testCode = reader.GetFieldValueCheckNull<int>(testCodeOrdinal);
        var name = reader.GetFieldValueCheckNull<string>(nameOrdinal);
        var lowValue = reader.GetFieldValueCheckNull<double>(lowValueOrdinal);
        var highValue = reader.GetFieldValueCheckNull<double>(highValueOrdinal);
        var measurementType = reader.GetFieldValueCheckNull<string>(measurementTypeOrdinal);
        return new Test(testCode, name, lowValue, highValue, measurementType);
    }

    #endregion
}