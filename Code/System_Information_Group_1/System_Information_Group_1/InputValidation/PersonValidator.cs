#nullable enable
using System;
using System.Text.RegularExpressions;

namespace System_Information_Group_1.InputValidation;

/// <summary>
///     Provides validation for person information.
/// </summary>
public static class PersonValidator
{
    #region Methods

    /// <summary>
    ///     Validates the first name.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <returns>String containing the error message, or null if valid.</returns>
    public static string? ValidateFirstName(string? firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return "First name cannot be empty.";
        }

        if (firstName.Trim().Length > 20)
        {
            return "First name cannot exceed 20 characters.";
        }

        return null;
    }

    /// <summary>
    ///     Validates the last name.
    /// </summary>
    /// <param name="lastName">The last name.</param>
    /// <returns>String containing the error message, or null if valid.</returns>
    public static string? ValidateLastName(string? lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            return "Last name cannot be empty.";
        }

        if (lastName.Trim().Length > 20)
        {
            return "Last name cannot exceed 20 characters.";
        }

        return null;
    }

    /// <summary>
    ///     Validates the date of birth.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <returns>String containing the error message, or null if valid.</returns>
    public static string? ValidateDateOfBirth(DateTime? dateOfBirth)
    {
        if (!dateOfBirth.HasValue ||
            dateOfBirth.Value == DateTime.MinValue ||
            dateOfBirth.Value.Date > DateTime.Today)
        {
            return "Invalid date of birth.";
        }

        return null;
    }

    /// <summary>
    ///     Validates the phone number.
    /// </summary>
    /// <param name="phoneNumber">The phone number.</param>
    /// <returns>String containing the error message, or null if valid.</returns>
    public static string? ValidatePhoneNumber(string? phoneNumber)
    {
        if (!Regex.IsMatch(phoneNumber?.Trim() ?? "", @"^[0-9]{10}$"))
        {
            return "Phone number must contain 10 digits.";
        }

        return null;
    }

    /// <summary>
    ///     Validates the zip.
    /// </summary>
    /// <param name="zip">The zip.</param>
    /// <returns>String containing the error message, or null if valid.</returns>
    public static string? ValidateZip(string? zip)
    {
        if (!Regex.IsMatch(zip?.Trim() ?? "", @"^([0-9]{5}|[0-9]{9})$"))
        {
            return "ZIP code must contain 5 or 9 digits.";
        }

        return null;
    }

    /// <summary>
    ///     Validates the address.
    /// </summary>
    /// <param name="address">The address.</param>
    /// <returns>String containing the error message, or null if valid.</returns>
    public static string? ValidateAddress(string? address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return "Address cannot be empty.";
        }

        if (address.Trim().Length > 20)
        {
            return "Address cannot exceed 20 characters.";
        }

        return null;
    }

    /// <summary>
    ///     Validates the city.
    /// </summary>
    /// <param name="city">The city.</param>
    /// <returns>String containing the error message, or null if valid.</returns>
    public static string? ValidateCity(string? city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return "City cannot be empty.";
        }

        if (city.Trim().Length > 20)
        {
            return "City cannot exceed 20 characters.";
        }

        return null;
    }

    /// <summary>
    ///     Validates the state.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>String containing the error message, or null if valid.</returns>
    public static string? ValidateState(string? state)
    {
        if (string.IsNullOrWhiteSpace(state))
        {
            return "Please select a state.";
        }

        if (state.Trim().Length > 20)
        {
            return "State cannot exceed 20 characters.";
        }

        return null;
    }

    /// <summary>
    ///     Validates the gender.
    /// </summary>
    /// <param name="gender">The gender.</param>
    /// <returns>String containing the error message, or null if valid.</returns>
    public static string? ValidateGender(string? gender)
    {
        if (string.IsNullOrWhiteSpace(gender))
        {
            return "Please select a gender.";
        }

        return null;
    }

    /// <summary>
    ///     Validates the person.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <param name="phoneNumber">The phone number.</param>
    /// <param name="zip">The zip.</param>
    /// <param name="address">The address.</param>
    /// <param name="city">The city.</param>
    /// <param name="state">The state.</param>
    /// <param name="gender">The gender.</param>
    /// <returns>String containing the error message, or null if valid.</returns>
    public static string? ValidatePerson(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string phoneNumber,
        string zip,
        string address,
        string city,
        string state,
        string gender)
    {
        return ValidateFirstName(firstName) ??
               ValidateLastName(lastName) ??
               ValidateDateOfBirth(dateOfBirth) ??
               ValidatePhoneNumber(phoneNumber) ??
               ValidateZip(zip) ??
               ValidateAddress(address) ??
               ValidateCity(city) ??
               ValidateState(state) ??
               ValidateGender(gender);
    }

    #endregion
}