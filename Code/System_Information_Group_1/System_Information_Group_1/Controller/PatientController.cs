using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

/// <summary>
///     Controller class for managing patient related operations, including registration, searching, and editing patient
///     information.
/// </summary>
public class PatientController
{
    #region Fields

    private readonly PersonDal personDal = new();
    private readonly PatientDal patientDal = new();

    #endregion

    #region Methods

    /// <summary>
    ///     Registers the patient.
    /// </summary>
    /// <param name="lastName">The last name.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <param name="phoneNumber">The phone number.</param>
    /// <param name="address">The address.</param>
    /// <param name="zip">The zip.</param>
    /// <param name="city">The city.</param>
    /// <param name="state">The state.</param>
    /// <param name="gender">The gender.</param>
    /// <returns>The registered patient.</returns>
    public Patient RegisterPatient(string lastName, string firstName, DateTime dateOfBirth, string phoneNumber,
        string address, string zip, string city, string state, string gender)
    {
        lastName = lastName?.Trim() ?? "";
        firstName = firstName?.Trim() ?? "";
        phoneNumber = phoneNumber?.Trim() ?? "";
        address = address?.Trim() ?? "";
        zip = zip?.Trim() ?? "";
        city = city?.Trim() ?? "";
        state = state?.Trim() ?? "";
        gender = gender?.Trim() ?? "";

        this.checkFields(firstName, lastName, dateOfBirth, phoneNumber, zip, address, city, state);
        this.checkFieldLengths(firstName, lastName, address, city, state);

        var person = this.personDal.CreatePerson(
            lastName, firstName, dateOfBirth.Date, phoneNumber, address, zip, city, state, gender);

        var patient = this.patientDal.CreatePatient(person.PersonId);
        return patient;
    }

    /// <summary>
    ///     Searches the patients based off user defined parameters.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <returns>List of matching patients.</returns>
    public List<Patient> SearchPatients(string firstName, string lastName, DateTime? dateOfBirth)
    {
        return this.patientDal.SearchPatients(firstName, lastName, dateOfBirth);
    }

    /// <summary>
    ///     Edits the patient.
    /// </summary>
    /// <param name="patient">The patient.</param>
    /// <exception cref="ArgumentException">
    ///     Invalid patient.
    ///     or
    ///     Data Mismatch: Person ID does not match.
    /// </exception>
    public void EditPatient(Patient patient)
    {
        if (patient == null || patient.Person == null)
        {
            throw new ArgumentException("Invalid patient.");
        }

        var person = patient.Person;

        if (patient.PersonId != person.PersonId)
        {
            throw new ArgumentException("Data Mismatch: Person ID does not match.");
        }

        person.FirstName = person.FirstName.Trim();
        person.LastName = person.LastName.Trim();
        person.ContactPhoneNumber = person.ContactPhoneNumber.Trim();
        person.Address = person.Address.Trim();
        person.Zip = person.Zip.Trim();
        person.City = person.City.Trim();
        person.State = person.State.Trim();

        this.checkFields(person.FirstName, person.LastName, person.DateOfBirth, person.ContactPhoneNumber, person.Zip,
            person.Address, person.City, person.State);
        this.checkFieldLengths(person.FirstName, person.LastName, person.Address, person.City, person.State);

        this.personDal.EditPerson(person);
        this.patientDal.EditPatient(patient);
    }

    private void checkFields(string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, string zip,
        string address, string city, string state)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("First name and last name cannot be empty.");
        }

        if (dateOfBirth == default || dateOfBirth.Date > DateTime.Today)
        {
            throw new ArgumentException("Invalid date of birth.");
        }

        if (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^[0-9]{10}$"))
        {
            throw new ArgumentException("Invalid phone number.");
        }

        if (string.IsNullOrWhiteSpace(zip) || !Regex.IsMatch(zip, @"^([0-9]{5}|[0-9]{9})$"))
        {
            throw new ArgumentException("Invalid zip code.");
        }

        if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(state))
        {
            throw new ArgumentException("Address, city, and state cannot be empty.");
        }
    }

    private void checkFieldLengths(string firstName, string lastName, string address, string city, string state)
    {
        if (firstName.Trim().Length > 20)
        {
            throw new ArgumentException("First name cannot exceed 20 characters.");
        }

        if (lastName.Trim().Length > 20)
        {
            throw new ArgumentException("Last name cannot exceed 20 characters.");
        }

        if (address.Trim().Length > 20)
        {
            throw new ArgumentException("Address cannot exceed 20 characters.");
        }

        if (city.Trim().Length > 20)
        {
            throw new ArgumentException("City cannot exceed 20 characters.");
        }

        if (state.Trim().Length > 20)
        {
            throw new ArgumentException("State cannot exceed 20 characters.");
        }
    }

    #endregion
}