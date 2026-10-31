using System.Collections.Generic;
using System;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

/// <summary>
///     The test order data access layer
///     @author Colby
///     @version Fall 2026
/// </summary>
public class TestOrderDal
{
    #region Methods

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

    public List<TestOrderDisplay> GetTestOrdersByAppointmentId(int appointmentId)
    {
        var testOrders = new List<TestOrderDisplay>();
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "SELECT o.appointment_id, o.test_code, o.performed_datetime, o.test_result, o.is_abnormal, " +
                    "t.name, t.low_value, t.high_value, t.measurement_type " +
                    "FROM test_order o " +
                    "JOIN test t ON o.test_code = t.test_code " +
                    "WHERE o.appointment_id = @appointmentId";
        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var performedDateTime = reader.IsDBNull(reader.GetOrdinal("performed_datetime"))
                ? (DateTime?)null
                : reader.GetDateTime(reader.GetOrdinal("performed_datetime"));
            var testResult = reader.IsDBNull(reader.GetOrdinal("test_result"))
                ? null
                : reader.GetString(reader.GetOrdinal("test_result"));
            var isAbnormal = !reader.IsDBNull(reader.GetOrdinal("is_abnormal")) &&
                             reader.GetBoolean(reader.GetOrdinal("is_abnormal"));

            testOrders.Add(new TestOrderDisplay(
                reader.GetInt32(reader.GetOrdinal("appointment_id")),
                reader.GetInt32(reader.GetOrdinal("test_code")),
                reader.GetString(reader.GetOrdinal("name")),
                reader.GetDouble(reader.GetOrdinal("low_value")),
                reader.GetDouble(reader.GetOrdinal("high_value")),
                reader.GetString(reader.GetOrdinal("measurement_type")),
                performedDateTime,
                testResult,
                isAbnormal));
        }

        return testOrders;
    }

    public void UpdateTestResults(IEnumerable<TestOrderDisplay> testOrders)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "UPDATE test_order SET performed_datetime = @performedDateTime, test_result = @testResult, " +
                    "is_abnormal = @isAbnormal WHERE appointment_id = @appointmentId AND test_code = @testCode";

        foreach (var testOrder in testOrders)
        {
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@performedDateTime", testOrder.PerformedDateTime.HasValue
                ? (object)testOrder.PerformedDateTime.Value
                : DBNull.Value);
            cmd.Parameters.AddWithValue("@testResult", string.IsNullOrWhiteSpace(testOrder.TestResult)
                ? (object)DBNull.Value
                : testOrder.TestResult);
            cmd.Parameters.AddWithValue("@isAbnormal", testOrder.IsAbnormal);
            cmd.Parameters.AddWithValue("@appointmentId", testOrder.AppointmentId);
            cmd.Parameters.AddWithValue("@testCode", testOrder.TestCode);
            cmd.ExecuteNonQuery();
        }
    }

    #endregion

    public void OrderTest(List<Test> orderedTests, int appointmentId)
    {

        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "INSERT INTO test_order (appointment_id, test_code) VALUES (@appointmentId, @testCode)";

        using var cmd = new MySqlCommand(query, connection);


        foreach (var test in orderedTests)
        {
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
            cmd.Parameters.AddWithValue("@testCode", test.TestCode);
            cmd.ExecuteNonQuery();
        }
    }
}