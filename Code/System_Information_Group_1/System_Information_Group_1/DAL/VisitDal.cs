using System;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL
{
    /// <summary>
    /// The data access layer for the visit.
    /// @author Colby
    /// @version Fall 2026
    /// </summary>
    public class VisitDal
    {
        /// <summary>
        /// Adds the visit.
        /// </summary>
        /// <param name="visit">The visit.</param>
        public void AddVisit(Visit visit)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var typoInsert = "INSERT INTO visit (apointment_id, symptoms, nurse_id, systelic_blood_pressure, diastolic_blood_pressure, temperature, pulse, height, weight, initial_diagnosis, final_diagnosis) " +
                             "VALUES (@appointment_id, @symptoms, @nurse_id, @systolic_blood_pressure, @diastolic_blood_pressure, @temperature, @pulse, @height, @weight, @initial_diagnosis, @final_diagnosis)";
            var standardInsert = "INSERT INTO visit (appointment_id, symptoms, nurse_id, systolic_blood_pressure, diastolic_blood_pressure, temperature, pulse, height, weight, initial_diagnosis, final_diagnosis) " +
                                 "VALUES (@appointment_id, @symptoms, @nurse_id, @systolic_blood_pressure, @diastolic_blood_pressure, @temperature, @pulse, @height, @weight, @initial_diagnosis, @final_diagnosis)";

            this.ExecuteWithFallback(connection, typoInsert, standardInsert, visit);
        }

        public Visit GetVisitByAppointmentId(int appointmentId)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var typoQuery = "select apointment_id as appointment_id, symptoms, nurse_id, systelic_blood_pressure as systolic_blood_pressure, diastolic_blood_pressure, temperature, pulse, height, weight, initial_diagnosis, final_diagnosis from visit where apointment_id = @appointment_id limit 1;";
            var standardQuery = "select appointment_id, symptoms, nurse_id, systolic_blood_pressure, diastolic_blood_pressure, temperature, pulse, height, weight, initial_diagnosis, final_diagnosis from visit where appointment_id = @appointment_id limit 1;";

            using var reader = this.ExecuteReaderWithFallback(connection, typoQuery, standardQuery, appointmentId);
            if (!reader.Read())
            {
                return null;
            }

            return new Visit(
                reader.GetFieldValueCheckNull<int>(reader.GetOrdinal("appointment_id")),
                reader.GetFieldValueCheckNull<string>(reader.GetOrdinal("symptoms")),
                reader.GetFieldValueCheckNull<int>(reader.GetOrdinal("nurse_id")),
                reader.GetFieldValueCheckNull<int>(reader.GetOrdinal("systolic_blood_pressure")),
                reader.GetFieldValueCheckNull<int>(reader.GetOrdinal("diastolic_blood_pressure")),
                reader.GetFieldValueCheckNull<double>(reader.GetOrdinal("temperature")),
                reader.GetFieldValueCheckNull<int>(reader.GetOrdinal("pulse")),
                reader.GetFieldValueCheckNull<double>(reader.GetOrdinal("height")),
                reader.GetFieldValueCheckNull<double>(reader.GetOrdinal("weight")),
                reader.GetFieldValueCheckNull<string>(reader.GetOrdinal("initial_diagnosis")),
                reader.GetFieldValueCheckNull<string>(reader.GetOrdinal("final_diagnosis"))
            );
        }

        public bool IsVisitFinalized(int appointmentId)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var typoQuery = "select final_diagnosis from visit where apointment_id = @appointment_id limit 1;";
            var standardQuery = "select final_diagnosis from visit where appointment_id = @appointment_id limit 1;";

            object finalDiagnosisValue;
            try
            {
                using var command = new MySqlCommand(typoQuery, connection);
                command.Parameters.Add("@appointment_id", MySqlDbType.Int32).Value = appointmentId;
                finalDiagnosisValue = command.ExecuteScalar();
            }
            catch (MySqlException)
            {
                using var fallbackCommand = new MySqlCommand(standardQuery, connection);
                fallbackCommand.Parameters.Add("@appointment_id", MySqlDbType.Int32).Value = appointmentId;
                finalDiagnosisValue = fallbackCommand.ExecuteScalar();
            }

            if (finalDiagnosisValue == null || finalDiagnosisValue == DBNull.Value)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(finalDiagnosisValue.ToString());
        }

        public bool UpdateVisit(Visit visit)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var typoUpdate = "update visit set symptoms = @symptoms, nurse_id = @nurse_id, systelic_blood_pressure = @systolic_blood_pressure, " +
                             "diastolic_blood_pressure = @diastolic_blood_pressure, temperature = @temperature, pulse = @pulse, height = @height, weight = @weight, " +
                             "initial_diagnosis = @initial_diagnosis, final_diagnosis = @final_diagnosis " +
                             "where apointment_id = @appointment_id and (final_diagnosis is null or trim(final_diagnosis) = '');";
            var standardUpdate = "update visit set symptoms = @symptoms, nurse_id = @nurse_id, systolic_blood_pressure = @systolic_blood_pressure, " +
                                 "diastolic_blood_pressure = @diastolic_blood_pressure, temperature = @temperature, pulse = @pulse, height = @height, weight = @weight, " +
                                 "initial_diagnosis = @initial_diagnosis, final_diagnosis = @final_diagnosis " +
                                 "where appointment_id = @appointment_id and (final_diagnosis is null or trim(final_diagnosis) = '');";

            return this.ExecuteWithFallback(connection, typoUpdate, standardUpdate, visit) > 0;
        }

        private int ExecuteWithFallback(MySqlConnection connection, string primarySql, string fallbackSql, Visit visit)
        {
            try
            {
                using var primaryCommand = new MySqlCommand(primarySql, connection);
                this.BindVisitParameters(primaryCommand, visit);
                return primaryCommand.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                using var fallbackCommand = new MySqlCommand(fallbackSql, connection);
                this.BindVisitParameters(fallbackCommand, visit);
                return fallbackCommand.ExecuteNonQuery();
            }
        }

        private MySqlDataReader ExecuteReaderWithFallback(MySqlConnection connection, string primarySql, string fallbackSql, int appointmentId)
        {
            try
            {
                var primaryCommand = new MySqlCommand(primarySql, connection);
                primaryCommand.Parameters.Add("@appointment_id", MySqlDbType.Int32).Value = appointmentId;
                return primaryCommand.ExecuteReader();
            }
            catch (MySqlException)
            {
                var fallbackCommand = new MySqlCommand(fallbackSql, connection);
                fallbackCommand.Parameters.Add("@appointment_id", MySqlDbType.Int32).Value = appointmentId;
                return fallbackCommand.ExecuteReader();
            }
        }

        private void BindVisitParameters(MySqlCommand command, Visit visit)
        {
            command.Parameters.AddWithValue("@appointment_id", visit.AppointmentId);
            command.Parameters.AddWithValue("@symptoms", visit.Symptoms);
            command.Parameters.AddWithValue("@nurse_id", visit.NurseId);
            command.Parameters.AddWithValue("@systolic_blood_pressure", visit.SystolicBloodPressure);
            command.Parameters.AddWithValue("@diastolic_blood_pressure", visit.DiastolicBloodPressure);
            command.Parameters.AddWithValue("@temperature", visit.Temperature);
            command.Parameters.AddWithValue("@pulse", visit.Pulse);
            command.Parameters.AddWithValue("@height", visit.Height);
            command.Parameters.AddWithValue("@weight", visit.Weight);
            command.Parameters.AddWithValue("@initial_diagnosis", visit.InitialDiagnosis);
            command.Parameters.AddWithValue("@final_diagnosis", visit.FinalDiagnosis);
        }
    }
}
