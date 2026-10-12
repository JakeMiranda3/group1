using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System_Information_Group_1.DAL;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.Controller;

public class PatientController
{
    private readonly PersonDal personDal = new PersonDal();
    private readonly PatientDal patientDal = new PatientDal();

    public Patient RegisterPatient(string lastName, string firstName, DateTime dateOfBirth, string phoneNumber,
        string address, string zip, string city, string state)
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

        phoneNumber = phoneNumber.Trim();
        zip = zip.Trim();

        this.checkFieldLengths(firstName, lastName, address, city, state);

        var person = this.personDal.CreatePerson(
            lastName.Trim(), firstName.Trim(), dateOfBirth.Date, phoneNumber, address.Trim(), zip, city.Trim(), state.Trim());

        var patient = this.patientDal.CreatePatient(person.PersonId);
        return patient;
    }

    public List<Patient> SearchPatients(string firstName, string lastName, DateTime? dateOfBirth)
    {
        return this.patientDal.SearchPatients(firstName, lastName, dateOfBirth);
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
 
}