using System;
using System.Collections.Generic;
using System_Information_Group_1.DAL;
using System_Information_Group_1.InputValidation;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

/// <summary>
///     Controller class for managing patient related operations,
///     including registration, searching, and editing patient information.
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
    public Patient RegisterPatient(
        string lastName,
        string firstName,
        DateTime dateOfBirth,
        string phoneNumber,
        string address,
        string zip,
        string city,
        string state,
        string gender)
    {
        lastName = lastName?.Trim() ?? "";
        firstName = firstName?.Trim() ?? "";
        phoneNumber = phoneNumber?.Trim() ?? "";
        address = address?.Trim() ?? "";
        zip = zip?.Trim() ?? "";
        city = city?.Trim() ?? "";
        state = state?.Trim() ?? "";
        gender = gender?.Trim() ?? "";

        checkFields(firstName, lastName, dateOfBirth,
            phoneNumber, zip, address, city, state);

        var genderError = PersonValidator.ValidateGender(gender);

        if (genderError != null)
        {
            throw new ArgumentException(genderError);
        }

        var patient = this.patientDal.CreatePatientWithPerson(
            lastName, firstName, dateOfBirth,
            phoneNumber, address, zip, city, state, gender);

        return patient;
    }

    /// <summary>
    ///     Searches the patients.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <returns>List of patients</returns>
    public List<Patient> SearchPatients(
        string firstName,
        string lastName,
        DateTime? dateOfBirth)
    {
        return this.patientDal.SearchPatients(
            firstName, lastName, dateOfBirth);
    }

    /// <summary>
    ///     Edits the patient.
    /// </summary>
    public void EditPatient(Patient patient)
    {
        if (patient == null || patient.Person == null)
        {
            throw new ArgumentException("Invalid patient.");
        }

        var person = patient.Person;

        if (patient.PersonId != person.PersonId)
        {
            throw new ArgumentException(
                "Data Mismatch: Person ID does not match.");
        }

        person.FirstName = person.FirstName?.Trim() ?? "";
        person.LastName = person.LastName?.Trim() ?? "";
        person.ContactPhoneNumber = person.ContactPhoneNumber?.Trim() ?? "";
        person.Address = person.Address?.Trim() ?? "";
        person.Zip = person.Zip?.Trim() ?? "";
        person.City = person.City?.Trim() ?? "";
        person.State = person.State?.Trim() ?? "";

        checkFields(
            person.FirstName,
            person.LastName,
            person.DateOfBirth,
            person.ContactPhoneNumber,
            person.Zip,
            person.Address,
            person.City,
            person.State);

        this.personDal.EditPerson(person);
        this.patientDal.EditPatient(patient);
    }

    /// <summary>
    ///     Validates all required person fields.
    /// </summary>
    private static void checkFields(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string phoneNumber,
        string zip,
        string address,
        string city,
        string state)
    {
        var error =
            PersonValidator.ValidateFirstName(firstName) ??
            PersonValidator.ValidateLastName(lastName) ??
            PersonValidator.ValidateDateOfBirth(dateOfBirth) ??
            PersonValidator.ValidatePhoneNumber(phoneNumber) ??
            PersonValidator.ValidateZip(zip) ??
            PersonValidator.ValidateAddress(address) ??
            PersonValidator.ValidateCity(city) ??
            PersonValidator.ValidateState(state);

        if (error != null)
        {
            throw new ArgumentException(error);
        }
    }

    #endregion
}