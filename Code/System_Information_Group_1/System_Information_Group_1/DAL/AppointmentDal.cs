using System;
using System.Collections.Generic;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

public class AppointmentDal
{

    private static Appointment createAppointment(MySqlDataReader reader, int appointmentIdOrdinal, int doctorIdOrdinal, int patientIdOrdinal,
        int reasonOrdinal, int appointmentDatetimeOrdinal) 
    {
        return new Appointment(reader.GetFieldValueCheckNull<int>(appointmentIdOrdinal),
            reader.GetFieldValueCheckNull<int>(doctorIdOrdinal),
            reader.GetFieldValueCheckNull<int>(patientIdOrdinal),
            reader.GetFieldValueCheckNull<string>(reasonOrdinal),
            reader.GetFieldValue<DateTime>(appointmentDatetimeOrdinal));
    }


    public Appointment GetAppointmentByAppointmentId(int appointmentId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select * from appointment where appointment_id = @id;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = appointmentId;
        using var reader = command.ExecuteReader();
        var appointmentIdOrdinal = reader.GetOrdinal("appointment_id");
        var doctorIdOrdinal = reader.GetOrdinal("doctor_id");
        var patientIdOrdinal = reader.GetOrdinal("patient_id");
        var reasonOrdinal = reader.GetOrdinal("reason");
        var appointmentDateTimeOrdinal = reader.GetOrdinal("appointment_datetime");
        return createAppointment(reader, appointmentIdOrdinal, doctorIdOrdinal, patientIdOrdinal, reasonOrdinal, appointmentDateTimeOrdinal);
    }

    public IList<Appointment> GetDoctorsAppointmentsByDoctorId(int doctorId)
    {
        IList<Appointment> appointments = new List<Appointment>();
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select * from appointment where doctor_id = @id;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = doctorId;
        using var reader = command.ExecuteReader();
        var appointmentIdOrdinal = reader.GetOrdinal("appointment_id");
        var doctorIdOrdinal = reader.GetOrdinal("doctor_id");
        var patientIdOrdinal = reader.GetOrdinal("patient_id");
        var reasonOrdinal = reader.GetOrdinal("reason");
        var appointmentDateTimeOrdinal = reader.GetOrdinal("appointment_datetime");
        while (reader.Read())
        {
            appointments.Add(createAppointment(reader, appointmentIdOrdinal, doctorIdOrdinal, patientIdOrdinal, reasonOrdinal, appointmentDateTimeOrdinal));
        }
        return appointments;
    }
}