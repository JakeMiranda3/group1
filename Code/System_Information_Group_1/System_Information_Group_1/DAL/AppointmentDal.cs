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

    private static Appointment createAppointment(MySqlDataReader reader, PersonDal personDal, int appointmentIdOrdinal, int doctorIdOrdinal, int patientIdOrdinal,
        int reasonOrdinal, int appointmentDatetimeOrdinal, int doctorPersonIdOrdinal, int patientPersonIdOrdinal)
    {
        var appointment = AppointmentDal.createAppointment(reader, appointmentIdOrdinal, doctorIdOrdinal, patientIdOrdinal,
            reasonOrdinal, appointmentDatetimeOrdinal);
        var doctorPersonId = reader.GetFieldValueCheckNull<int>(doctorPersonIdOrdinal);
        var patientPersonId = reader.GetFieldValueCheckNull<int>(patientPersonIdOrdinal);
        appointment.Patient = personDal.GetPersonWithId(patientPersonId);
        appointment.Doctor = personDal.GetPersonWithId(doctorPersonId);
        return appointment;
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

    public IList<Appointment> GetAppointmentsContainingDoctorAndPatient(int doctorId)
    {
        IList<Appointment> appointments = new List<Appointment>();
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select a.*, d.person_id as doctor_person_id, p.person_id as patient_person_id from appointment a " +
                    "join doctor d on a.doctor_id = d.doctor_id join patient p on a.patient_id = p.patient_id where a.doctor_id = @id;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@id", MySqlDbType.Int32).Value = doctorId;
        using var reader = command.ExecuteReader();
        var appointmentIdOrdinal = reader.GetOrdinal("appointment_id");
        var doctorIdOrdinal = reader.GetOrdinal("doctor_id");
        var patientIdOrdinal = reader.GetOrdinal("patient_id");
        var reasonOrdinal = reader.GetOrdinal("reason");
        var appointmentDateTimeOrdinal = reader.GetOrdinal("appointment_datetime");
        var doctorPersonIdOrdinal = reader.GetOrdinal("doctor_person_id");
        var patientPersonIdOrdinal = reader.GetOrdinal("patient_person_id");
        var personDal = new PersonDal();
        while (reader.Read())
        {
            var currentAppointment = createAppointment(reader, personDal,appointmentIdOrdinal, doctorIdOrdinal, patientIdOrdinal,
                reasonOrdinal, appointmentDateTimeOrdinal, doctorPersonIdOrdinal, patientPersonIdOrdinal);
            appointments.Add(currentAppointment);
        }
        return appointments;
    }

    public bool StoreNewAppointment(int doctorId, int patientId, DateTime appointmentDateTime, string reason)
    {
    
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var insert = "insert into appointment (doctor_id, patient_id, reason, appointment_datetime) values (@doctorId, @patientId, @reason, @appointmentDateTime);";
        using var cmd = new MySqlCommand(insert, connection);
        cmd.Parameters.Add("@doctorId", MySqlDbType.Int32).Value = doctorId;
        cmd.Parameters.Add("@patientId", MySqlDbType.Int32).Value = patientId;
        cmd.Parameters.Add("@reason", MySqlDbType.VarChar).Value = reason;
        cmd.Parameters.Add("@appointmentDateTime", MySqlDbType.DateTime).Value = appointmentDateTime;
        return cmd.ExecuteNonQuery() > 0;
    }



}