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

    /// <summary>
    /// Gets all tests.
    /// </summary>
    /// <returns>The list of all tests.</returns>
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

    /// <summary>
    /// Creates a test from the current row of the reader.
    /// </summary>
    /// <param name="reader">The reader positioned on a test row.</param>
    /// <param name="testCodeOrdinal">The test code column ordinal.</param>
    /// <param name="nameOrdinal">The name column ordinal.</param>
    /// <param name="lowValueOrdinal">The low value column ordinal.</param>
    /// <param name="highValueOrdinal">The high value column ordinal.</param>
    /// <param name="measurementTypeOrdinal">The measurement type column ordinal.</param>
    /// <returns>The test built from the current row.</returns>
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

    /// <summary>
    /// Gets all tests ordered for an appointment.
    /// </summary>
    /// <param name="appointmentId">The appointment identifier.</param>
    /// <returns>The list of tests ordered for the appointment.</returns>
    public List<Test> GetAllTestOrderedByAppointmentId(int appointmentId)
    {
        var testList = new List<Test>();
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "SELECT t.test_code, t.name, t.low_value, t.high_value, t.measurement_type " +
                    "FROM test_order o " +
                    "JOIN test t ON o.test_code = t.test_code " +
                    "WHERE o.appointment_id = @appointmentId";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
        using var reader = cmd.ExecuteReader();
        var testCodeOrdinal = reader.GetOrdinal("test_code");
        var nameOrdinal = reader.GetOrdinal("name");
        var lowValueOrdinal = reader.GetOrdinal("low_value");
        var highValueOrdinal = reader.GetOrdinal("high_value");
        var measurementTypeOrdinal = reader.GetOrdinal("measurement_type");
        while (reader.Read())
        {
            testList.Add(TestDal.CreateTest(reader, testCodeOrdinal, nameOrdinal, lowValueOrdinal, highValueOrdinal,
                measurementTypeOrdinal));
        }

        return testList;
    }

    #endregion
}