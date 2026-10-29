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
        var appointment = createAppointment(reader, appointmentIdOrdinal, doctorIdOrdinal, patientIdOrdinal,
            reasonOrdinal, appointmentDatetimeOrdinal);
        var doctorPersonId = reader.GetFieldValueCheckNull<int>(doctorPersonIdOrdinal);
        var patientPersonId = reader.GetFieldValueCheckNull<int>(patientPersonIdOrdinal);
        appointment.Patient = new Patient {Person = personDal.GetPersonWithId(patientPersonId) };
        appointment.Doctor = new Doctor {Person = personDal.GetPersonWithId(doctorPersonId) };
        return appointment;
    }


    /// <summary>
    /// Gets the appointment by appointment identifier.
    /// </summary>
    /// <param name="appointmentId">The appointment identifier.</param>
    /// <returns>The requested appointment</returns>
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

    /// <summary>
    /// Gets the doctors appointments by doctor identifier.
    /// </summary>
    /// <param name="doctorId">The doctor identifier.</param>
    /// <returns>The list of appointments for the given doctors id</returns>
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

    public bool DoesPatientHaveAppointments(int patientId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select exists (select 1 from appointment where patient_id = @id)";

        bool exists = connection.ExecuteScalar<bool>(query, new { id = patientId });
        return exists;
    }

    public IList<Appointment> GetAllAppointmentsByPatientId(int patientId)
    {
        using var connection = new MySqlConnection(Connection.ConnectionString());
        connection.Open();
        var query = "select app.appointment_id as AppointmentId, app.reason as Reason, app.appointment_datetime as `Date`, doc.doctor_id as DoctorId, docPerson.person_id as PersonId, docPerson.first_name as FirstName, docPerson.last_name as LastName, " +
                    "pat.patient_id as PatientId, patPerson.person_id as PersonId, patPerson.first_name as FirstName, patPerson.last_name as LastName " +
                    "from appointment as app " +
                    "join doctor doc on app.doctor_id = doc.doctor_id " +
                    "join person docPerson on doc.person_id = docPerson.person_id " +
                    "join patient as pat on app.patient_id = pat.patient_id " +
                    "join person patPerson on pat.person_id = patPerson.person_id " +
                    "where app.patient_id = @id;";
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
        var query = "select a.*, d.person_id as doctor_person_id, p.person_id as patient_person_id from appointment a " +
                    "join doctor d on a.doctor_id = d.doctor_id join patient p on a.patient_id = p.patient_id " +
                    "where a.patient_id = @patientId order by a.appointment_datetime desc;";
        using var command = new MySqlCommand(query, connection);
        command.Parameters.Add("@patientId", MySqlDbType.Int32).Value = patientId;
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
            var currentAppointment = createAppointment(reader, personDal, appointmentIdOrdinal, doctorIdOrdinal, patientIdOrdinal,
                reasonOrdinal, appointmentDateTimeOrdinal, doctorPersonIdOrdinal, patientPersonIdOrdinal);
            appointments.Add(currentAppointment);
        }

        return appointments;
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