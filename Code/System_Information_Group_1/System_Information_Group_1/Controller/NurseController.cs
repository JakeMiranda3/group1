using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System_Information_Group_1.DAL;
using System_Information_Group_1.InputValidation;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

/// <summary>
///     Controller class for managing nurse related operations,
///     including registration, searching, and editing nurse information.
/// </summary>
public class NurseController
{
    #region Fields

    private readonly PersonDal personDal = new();
    private readonly NurseDal nurseDal = new();

    #endregion

    #region Methods

    /// <summary>
    ///     Registers the nurse.
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
    /// <returns>The registered nurse.</returns>
    public Nurse RegisterNurse(string lastName, string firstName,
        DateTime dateOfBirth, string phoneNumber, string address,
        string zip, string city, string state, string gender)
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

        var nurse = this.nurseDal.CreateNurseWithPerson(
            lastName, firstName, dateOfBirth,
            phoneNumber, address, zip, city, state, gender);

        return nurse;
    }

    /// <summary>
    ///     Searches nurses based on user defined parameters.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <returns>List of matching nurses.</returns>
    public List<Nurse> SearchNurses(string firstName, string lastName,
        DateTime? dateOfBirth)
    {
        return this.nurseDal.SearchNurses(firstName, lastName, dateOfBirth);
    }

    /// <summary>
    ///     Edits the nurse's personal information.
    /// </summary>
    /// <param name="nurse">The nurse.</param>
    public void EditNurse(Nurse nurse)
    {
        if (nurse == null || nurse.Person == null)
        {
            throw new ArgumentException("Invalid nurse.");
        }

        var person = nurse.Person;

        if (nurse.PersonId != person.PersonId)
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
    }

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