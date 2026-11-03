using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL;

/// <summary>
/// The data access layer for the appointment table. This class provides methods to retrieve and store appointments.
/// </summary>
public class AppointmentDal
{




    /// <summary>
    /// Gets the appointment by appointment identifier.
    /// </summary>
    /// <param name="appointmentId">The appointment identifier.</param>
    /// <returns>The requested appointment</returns>
    //public Appointment GetAppointmentByAppointmentId(int appointmentId)
    //{
    //    using var connection = new MySqlConnection(Connection.ConnectionString());
    //    connection.Open();
    //    var query = "select * from appointment where appointment_id = @id;";
    //    using var command = new MySqlCommand(query, connection);
    //    command.Parameters.Add("@id", MySqlDbType.Int32).Value = appointmentId;
    //    using var reader = command.ExecuteReader();
    //    var appointmentIdOrdinal = reader.GetOrdinal("appointment_id");
    //    var doctorIdOrdinal = reader.GetOrdinal("doctor_id");
    //    var patientIdOrdinal = reader.GetOrdinal("patient_id");
    //    var reasonOrdinal = reader.GetOrdinal("reason");
    //    var appointmentDateTimeOrdinal = reader.GetOrdinal("appointment_datetime");
    //    return createAppointment(reader, appointmentIdOrdinal, doctorIdOrdinal, patientIdOrdinal, reasonOrdinal, appointmentDateTimeOrdinal);
    //}

    /// <summary>
    /// Gets the doctors appointments by doctor identifier.
    /// </summary>
    /// <param name="doctorId">The doctor identifier.</param>
    /// <returns>The list of appointments for the given doctors id</returns>
    //public IList<Appointment> GetDoctorsAppointmentsByDoctorId(int doctorId)
    //{
    //    IList<Appointment> appointments = new List<Appointment>();
    //    using var connection = new MySqlConnection(Connection.ConnectionString());
    //    connection.Open();
    //    var query = "select * from appointment where doctor_id = @id;";
    //    using var command = new MySqlCommand(query, connection);
    //    command.Parameters.Add("@id", MySqlDbType.Int32).Value = doctorId;
    //    using var reader = command.ExecuteReader();
    //    var appointmentIdOrdinal = reader.GetOrdinal("appointment_id");
    //    var doctorIdOrdinal = reader.GetOrdinal("doctor_id");
    //    var patientIdOrdinal = reader.GetOrdinal("patient_id");
    //    var reasonOrdinal = reader.GetOrdinal("reason");
    //    var appointmentDateTimeOrdinal = reader.GetOrdinal("appointment_datetime");
    //    while (reader.Read())
    //    {
    //        appointments.Add(createAppointment(reader, appointmentIdOrdinal, doctorIdOrdinal, patientIdOrdinal, reasonOrdinal, appointmentDateTimeOrdinal));
    //    }
    //    return appointments;
    //}


    public IList<Appointment> GetAllAppointmentsByPatientId(int patientId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "SELECT app.appointment_id AS AppointmentId, app.reason AS Reason, app.appointment_datetime AS `Date`, doc.doctor_id AS DoctorId," +
                    " docPerson.person_id AS PersonId, docPerson.first_name AS FirstName, docPerson.last_name AS LastName, " +
                    "pat.patient_id AS PatientId, patPerson.person_id AS PersonId, patPerson.first_name AS FirstName, patPerson.last_name AS LastName," +
                    " patPerson.contact_phone_number AS ContactPhoneNumber " +
                    "from appointment AS app " +
                    "JOIN doctor doc on app.doctor_id = doc.doctor_id " +
                    "JOIN person docPerson on doc.person_id = docPerson.person_id " +
                    "JOIN patient as pat on app.patient_id = pat.patient_id " +
                    "JOIN person patPerson on pat.person_id = patPerson.person_id " +
                    "WHERE app.patient_id = @id;";
        IList<Appointment> appointments = connection.Query<Appointment, Doctor, Person, Patient, Person, Appointment>(query,
            (appointment, doctor, doctorPerson, patient, patientPerson) =>
            {
                doctor.Person = doctorPerson;
                patient.Person = patientPerson;

                appointment.Doctor = doctor;
                appointment.Patient = patient;

                appointment.DoctorId = doctor.DoctorId;
                appointment.PatientId = patient.PatientId;

                return appointment;
            },
            new { id = patientId },
            splitOn: "DoctorId, PersonId, PatientId, PersonId").ToList();
        return appointments;
    }

    public void EditAppointment(Appointment appointment)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "update appointment " +
                    "set reason = @Reason, appointment_datetime = @Date " +
                    "where appointment_id = @AppointmentId;";

        connection.Execute(query, new
        {
            appointment.Reason,
            appointment.Date,
            appointment.AppointmentId
        });
    }

    /// <summary>
    /// Gets the appointments containing doctor and patient.
    /// </summary>
    /// <param name="doctorId">The doctor identifier.</param>
    /// <returns>Gets the list of appointments by doctor, with patient and doctors values stored</returns>
    public IList<Appointment> GetAppointmentsContainingDoctorAndPatient(int doctorId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());

        var query ="SELECT app.appointment_id AS AppointmentId, app.reason AS Reason, app.appointment_datetime AS `Date`, " +
                   "doc.doctor_id AS DoctorId, docPerson.person_id AS PersonId, docPerson.first_name AS FirstName, docPerson.last_name AS LastName," +
                   " pat.patient_id AS PatientId, patPerson.person_id AS PersonId, patPerson.first_name AS FirstName, patPerson.last_name AS LastName," +
                   " patPerson.contact_phone_number AS ContactPhoneNumber FROM appointment AS app" +
                   " JOIN doctor doc ON app.doctor_id = doc.doctor_id " +
                   "JOIN person docPerson ON doc.person_id = docPerson.person_id " +
                   "JOIN patient pat ON app.patient_id = pat.patient_id " +
                   "JOIN person patPerson ON pat.person_id = patPerson.person_id" +
                   " WHERE app.doctor_id = @id;";

        return connection.Query<Appointment, Doctor, Person, Patient, Person, Appointment>(
            query,
            (appointment, doctor, doctorPerson, patient, patientPerson) =>
            {
                doctor.Person = doctorPerson;
                patient.Person = patientPerson;

                appointment.Doctor = doctor;
                appointment.Patient = patient;

                appointment.DoctorId = doctor.DoctorId;
                appointment.PatientId = patient.PatientId;

                return appointment;
            },
            new { id = doctorId },
            splitOn: "DoctorId, PersonId, PatientId, PersonId"
        ).ToList();
    }

    /// <summary>
    /// Gets the appointments for a patient including doctor and patient people details.
    /// </summary>
    /// <param name="patientId">The patient identifier.</param>
    /// <returns>List of appointments for the selected patient.</returns>
    public IList<Appointment> GetAppointmentsContainingDoctorAndPatientByPatientId(int patientId)
    {
        IList<Appointment> appointments = new List<Appointment>();
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "SELECT app.appointment_id AS AppointmentId, app.reason AS Reason, app.appointment_datetime AS `Date`," +
                    " doc.doctor_id AS DoctorId, docPerson.person_id AS PersonId, docPerson.first_name AS FirstName, docPerson.last_name AS LastName," +
                    " pat.patient_id AS PatientId, patPerson.person_id AS PersonId, patPerson.first_name AS FirstName, patPerson.last_name AS LastName," +
                    " patPerson.contact_phone_number AS ContactPhoneNumber FROM appointment AS app" +
                    " JOIN doctor doc ON app.doctor_id = doc.doctor_id" +
                    " JOIN person docPerson ON doc.person_id = docPerson.person_id" +
                    " JOIN patient pat ON app.patient_id = pat.patient_id" +
                    " JOIN person patPerson ON pat.person_id = patPerson.person_id" +
                    " WHERE app.patient_id = @id" +
                    " ORDER BY app.appointment_datetime DESC;";
        return connection.Query<Appointment, Doctor, Person, Patient, Person, Appointment>(
            query,
            (appointment, doctor, doctorPerson, patient, patientPerson) =>
            {
                doctor.Person = doctorPerson;
                patient.Person = patientPerson;

                appointment.Doctor = doctor;
                appointment.Patient = patient;

                appointment.DoctorId = doctor.DoctorId;
                appointment.PatientId = patient.PatientId;

                return appointment;
            },
            new { id = patientId },
            splitOn: "DoctorId, PersonId, PatientId, PersonId"
        ).ToList();
    }

    /// <summary>
    /// Stores the new appointment.
    /// </summary>
    /// <param name="doctorId">The doctor identifier.</param>
    /// <param name="patientId">The patient identifier.</param>
    /// <param name="appointmentDateTime">The appointment date time.</param>
    /// <param name="reason">The reason.</param>
    /// <returns>True if the appointment was stored</returns>
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

    public bool HasDoctorAppointmentConflict(int doctorId, DateTime date, int appointmentId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());

        var query = "SELECT EXISTS(SELECT 1 FROM appointment WHERE doctor_id = @doctorId AND appointment_datetime = @date AND appointment_id != @appointmentId);";
        return connection.ExecuteScalar<bool>(query, new { doctorId, date, appointmentId });
    }

    public int GetAppointmentIdByPatientIdWithDate(int patientId, DateTime date)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();

        var query = "select appointment_id from appointment where patient_id = @patientId and appointment_datetime = @date;";

        using var cmd = new MySqlCommand(query, connection);
        cmd.Parameters.Add("@patientId", MySqlDbType.Int32).Value = patientId;
        cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = date;

        var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return reader.GetFieldValueCheckNull<int>(reader.GetOrdinal("appointment_id"));
        }
        else
        {
            throw new Exception("No appointment found for the given patient ID and date.");
        }
    }


}