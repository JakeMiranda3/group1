using System;

namespace System_Information_Group_1.Model;

/// <summary>
///     The person class represents the core of our application. It contains the properties that define a person,
///     such as their ID, name, date of birth, contact information, and address details.
///     This class serves as a blueprint for creating person objects and is essential for managing and manipulating
///     person-related data within the application.
///     @author Colby
///     @version Fall 2026
/// </summary>
public class Person
{
    #region Properties

    /// <summary>
    ///     Gets or sets the person identifier.
    /// </summary>
    /// <value>
    ///     The person identifier.
    /// </value>
    public int PersonId { get; set; }

    /// <summary>
    ///     Gets or sets the last name.
    /// </summary>
    /// <value>
    ///     The last name.
    /// </value>
    public string LastName { get; set; }

    /// <summary>
    ///     Gets or sets the first name.
    /// </summary>
    /// <value>
    ///     The first name.
    /// </value>
    public string FirstName { get; set; }
    /// <summary>
    /// Gets or sets the date of birth.
    /// </summary>
    /// <value>
    /// The date of birth.
    /// </value>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    ///     Gets or sets the contact phone number.
    /// </summary>
    /// <value>
    ///     The contact phone number.
    /// </value>
    public string ContactPhoneNumber { get; set; }
    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    /// <value>
    /// The address.
    /// </value>
    public string Address { get; set; }

    /// <summary>
    ///     Gets or sets the zip.
    /// </summary>
    /// <value>
    ///     The zip.
    /// </value>
    public string Zip { get; set; }

    /// <summary>
    ///     Gets or sets the city.
    /// </summary>
    /// <value>
    ///     The city.
    /// </value>
    public string City { get; set; }

    /// <summary>
    ///     Gets or sets the state.
    /// </summary>
    /// <value>
    ///     The state.
    /// </value>
    public string State { get; set; }

    /// <summary>
    ///     Gets or sets roles assigned to the person (comma separated).
    /// </summary>
    public string Roles { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="Person" /> class.
    /// </summary>
    /// <param name="personId">The person identifier.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="dateOfBirth">The date of birth.</param>
    /// <param name="contractPhoneNumber">The contract phone number.</param>
    /// <param name="address">The address.</param>
    /// <param name="zip">The zip.</param>
    /// <param name="city">The city.</param>
    /// <param name="state">The state.</param>
    /// <exception cref="System.ArgumentNullException">
    ///     personId
    ///     or
    ///     lastName
    ///     or
    ///     firstName
    ///     or
    ///     contractPhoneNumber
    ///     or
    ///     address
    ///     or
    ///     zip
    ///     or
    ///     city
    ///     or
    ///     state
    /// </exception>
    public Person(int personId, string lastName, string firstName, DateTime dateOfBirth, string contractPhoneNumber,
        string address, string zip, string city, string state)
    {
        this.PersonId = personId != 0 ? personId : throw new ArgumentNullException(nameof(personId));
        this.LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        this.FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        this.DateOfBirth = dateOfBirth;
        this.ContactPhoneNumber = contractPhoneNumber ?? throw new ArgumentNullException(nameof(contractPhoneNumber));
        this.Address = address ?? throw new ArgumentNullException(nameof(address));
        this.Zip = zip ?? throw new ArgumentNullException(nameof(zip));
        this.City = city ?? throw new ArgumentNullException(nameof(city));
        this.State = state ?? throw new ArgumentNullException(nameof(state));
    }

    #endregion
}