using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using Dapper;
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
                    "t.name, t.low_value, t.high_value, t.measurement_type, o.result_entered_datetime " +
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

            var testResultDateTime =
                reader.GetFieldValueCheckNull<DateTime?>(reader.GetOrdinal("result_entered_datetime"));

            testOrders.Add(new TestOrderDisplay(
                reader.GetFieldValueCheckNull<Int32>(reader.GetOrdinal("appointment_id")),
                reader.GetFieldValueCheckNull<Int32>(reader.GetOrdinal("test_code")),
                reader.GetFieldValueCheckNull<String>(reader.GetOrdinal("name")),
                reader.GetFieldValueCheckNull<Double>(reader.GetOrdinal("low_value")),
                reader.GetFieldValueCheckNull<Double>(reader.GetOrdinal("high_value")),
                reader.GetFieldValue<String>(reader.GetOrdinal("measurement_type")),
                performedDateTime,
                testResult,
                isAbnormal,
                testResultDateTime));
        }

        return testOrders;
    }

    public void UpdateTestResults(IEnumerable<TestOrderDisplay> testOrders)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "UPDATE test_order SET performed_datetime = @performedDateTime, " +
                    "result_entered_datetime = @resultEnteredDateTime, test_result = @testResult, " +
                    "is_abnormal = @isAbnormal WHERE appointment_id = @appointmentId AND test_code = @testCode";
        using var transaction = connection.BeginTransaction();

        try
        {

            foreach (var testOrder in testOrders)
            {


                using var cmd = new MySqlCommand(query, connection, transaction);
                cmd.Parameters.AddWithValue("@performedDateTime", testOrder.PerformedDatetime.HasValue
                    ? (object)testOrder.PerformedDatetime.Value
                    : DBNull.Value);
                cmd.Parameters.AddWithValue("@resultEnteredDateTime", testOrder.TestResultDatetime.HasValue
                    ? (object)testOrder.TestResultDatetime.Value
                    : DBNull.Value);
                cmd.Parameters.AddWithValue("@testResult", string.IsNullOrWhiteSpace(testOrder.TestResult)
                    ? (object)DBNull.Value
                    : testOrder.TestResult);
                cmd.Parameters.AddWithValue("@isAbnormal", testOrder.IsAbnormal);
                cmd.Parameters.AddWithValue("@appointmentId", testOrder.AppointmentId);
                cmd.Parameters.AddWithValue("@testCode", testOrder.TestCode);
                cmd.ExecuteNonQuery();

                using var procedureCmd = new MySqlCommand("setAbnormality", connection, transaction);
                procedureCmd.CommandType = CommandType.StoredProcedure;
                procedureCmd.Parameters.Add("p_appointment_id", MySqlDbType.Int32).Value = testOrder.AppointmentId;
                procedureCmd.Parameters.Add("p_test_code", MySqlDbType.Int32).Value = testOrder.TestCode;
                procedureCmd.Parameters.Add("p_test_result", MySqlDbType.String).Value = testOrder.TestResult;
                procedureCmd.ExecuteNonQuery();
            }

            transaction.Commit();

        }
        catch 
        {
            transaction.Rollback();
            throw;
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

    public IList<int> GetAppointmentIdsWithOrders(IList<int> appointmentIds)
    {
        if (appointmentIds.Count == 0)
        {
            return [];
        }

        using var connection = new MySqlConnection(Connection.ConnectionString());

        var query = "SELECT DISTINCT appointment_id FROM test_order WHERE appointment_id IN @appointmentIds;";

        return connection.Query<int>(query, new { appointmentIds }).ToList();
    }


    public static TestOrder CreateOrder(MySqlDataReader reader, int appointmentIdOrdinal, int testCodeOrdinal, int performedDatetimeOrdinal, int testResultOrdinal, int isAbnormalOrdinal, int testResultEnteredDatetimeOrdinal)
    {
        var appointmentId = reader.GetFieldValue<Int32>(appointmentIdOrdinal);
        var testCode = reader.GetFieldValue<Int32>(testCodeOrdinal);
        var performedDatetime = reader.IsDBNull(performedDatetimeOrdinal) ? (DateTime?)null : reader.GetDateTime(performedDatetimeOrdinal);
        var testResult = reader.IsDBNull(testResultOrdinal) ? null : reader.GetString(testResultOrdinal);
        var isAbnormal = !reader.IsDBNull(isAbnormalOrdinal) && reader.GetBoolean(isAbnormalOrdinal);
        var testResultDatetime = reader.GetFieldValueCheckNull<DateTime>(testResultEnteredDatetimeOrdinal);
        return new TestOrder(appointmentId, testCode, performedDatetime ?? DateTime.MinValue, testResult ?? string.Empty, isAbnormal, testResultDatetime);
    }

}