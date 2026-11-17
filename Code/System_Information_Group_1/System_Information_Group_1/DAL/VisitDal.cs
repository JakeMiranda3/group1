using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
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

            var insert = "INSERT INTO visit (appointment_id, symptoms, nurse_id, systolic_blood_pressure, diastolic_blood_pressure, temperature, pulse, height, weight, initial_diagnosis, final_diagnosis, final_diagnosis_entered_datetime) " +
                         "VALUES (@appointment_id, @symptoms, @nurse_id, @systolic_blood_pressure, @diastolic_blood_pressure, @temperature, @pulse, @height, @weight, @initial_diagnosis, @final_diagnosis, @final_diagnosis_entered_datetime)";

            using var command = new MySqlCommand(insert, connection);
            bindVisitParameters(command, visit);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Gets the visit for the given appointment.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns>The visit, or null if no visit has been saved for the appointment yet.</returns>
        public Visit GetVisitByAppointmentId(int appointmentId)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = "select appointment_id, symptoms, nurse_id, systolic_blood_pressure, diastolic_blood_pressure, temperature, pulse, height, weight, initial_diagnosis, final_diagnosis, final_diagnosis_entered_datetime from visit where appointment_id = @appointment_id limit 1;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@appointment_id", MySqlDbType.Int32).Value = appointmentId;
            using var reader = command.ExecuteReader();
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
                reader.GetFieldValueCheckNull<string>(reader.GetOrdinal("final_diagnosis")),
                reader.GetFieldValueCheckNull<DateTime?>(reader.GetOrdinal("final_diagnosis_entered_datetime"))
            );
        }

        /// <summary>
        /// Determines whether the visit for the given appointment has a final diagnosis.
        /// </summary>
        /// <param name="appointmentId">The appointment identifier.</param>
        /// <returns>True if the visit has a final diagnosis; otherwise false.</returns>
        public bool IsVisitFinalized(int appointmentId)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = "select final_diagnosis from visit where appointment_id = @appointment_id limit 1;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@appointment_id", MySqlDbType.Int32).Value = appointmentId;
            var finalDiagnosisValue = command.ExecuteScalar();

            if (finalDiagnosisValue == null || finalDiagnosisValue == DBNull.Value)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(finalDiagnosisValue.ToString());
        }

        /// <summary>
        /// Updates the visit, as long as it has not already been finalized.
        /// </summary>
        /// <param name="visit">The visit.</param>
        /// <returns>True if the visit was updated; false if it was already finalized or does not exist.</returns>
        public bool UpdateVisit(Visit visit)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var update = "update visit set symptoms = @symptoms, nurse_id = @nurse_id, systolic_blood_pressure = @systolic_blood_pressure, " +
                         "diastolic_blood_pressure = @diastolic_blood_pressure, temperature = @temperature, pulse = @pulse, height = @height, weight = @weight, " +
                         "initial_diagnosis = @initial_diagnosis, final_diagnosis = @final_diagnosis, final_diagnosis_entered_datetime = @final_diagnosis_entered_datetime " +
                         "where appointment_id = @appointment_id and (final_diagnosis is null or trim(final_diagnosis) = '');";

            using var command = new MySqlCommand(update, connection);
            bindVisitParameters(command, visit);
            return command.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Gets all appointments ids that have a final diagnosis for the visit
        /// </summary>
        /// <param name="appointmentIds">a list of appointment ids to check if are final</param>
        /// <returns></returns>
        public IList<int> GetFinalizedAppointmentIds(IList<int> appointmentIds)
        {
            if (appointmentIds.Count == 0)
            {
                return [];
            }

            using var connection = new MySqlConnection(Connection.ConnectionString());

            var query = "SELECT DISTINCT appointment_id FROM visit WHERE appointment_id IN @appointmentIds AND final_diagnosis IS NOT NULL";

            return connection.Query<int>(query, new { appointmentIds }).ToList();
        }

        private static void bindVisitParameters(MySqlCommand command, Visit visit)
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
            command.Parameters.Add("@final_diagnosis_entered_datetime", MySqlDbType.DateTime).Value =
                visit.FinalDiagnosisEnteredDatetime;
        }
    }
}
