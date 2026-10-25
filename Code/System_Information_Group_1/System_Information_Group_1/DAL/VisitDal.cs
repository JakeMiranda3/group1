using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// <param name="vist">The visit.</param>
        public void AddVisit(Visit visit)
        {
            var connectionString = Connection.ConnectionString();

            using var connection = new MySqlConnection(connectionString);
            
            connection.Open();
            using var command = new MySqlCommand(
                "INSERT INTO visit (appointment_id, symptoms, nurse_id, systolic_blood_pressure, diastolic_blood_pressure, temperature, pulse, height, weight, initial_diagnosis, final_diagnosis) " +
                "VALUES (@appointment_id, @symptoms, @nurse_id, @systolic_blood_pressure, @diastolic_blood_pressure, @temperature, @pulse, @height, @weight, @initial_diagnosis, @final_diagnosis)", 
                connection);
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
            command.ExecuteNonQuery();
            
        }
    }
}
