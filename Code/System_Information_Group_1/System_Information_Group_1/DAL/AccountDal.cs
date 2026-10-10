using System;
using MySqlConnector;
using System_Information_Group_1.Constants;
using System_Information_Group_1.Model;

namespace System_Information_Group_1.DAL
{
    /// <summary>
    /// The account data access layer that serves as an interface to the DB
    /// @author Colby
    /// @version Fall 2026
    /// </summary>
    public class AccountDal
    {
        #region Access Methods        
        /// <summary>
        /// Gets the account with person identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The account with the corresponding person ID</returns>
        /// <exception cref="System.ArgumentNullException">no results were returned from the reader</exception>
        public Account GetAccountWithPersonId(int id)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = "SELECT * FROM account WHERE person_id = @id";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

            using var reader = command.ExecuteReader();

            int personIdOrdinal = reader.GetOrdinal("person_id");
            int accountTypeOrdinal = reader.GetOrdinal("account_type");
            int usernameOrdinal = reader.GetOrdinal("username");
            int hashedPasswordOrdinal = reader.GetOrdinal("hashed_password");

            return reader.Read() ? createAccount(reader, personIdOrdinal, accountTypeOrdinal, usernameOrdinal, hashedPasswordOrdinal) : throw new ArgumentNullException(nameof(reader), "reader returned no results");
        }

        /// <summary>
        /// Gets the name of the account with user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The account with the specified username</returns>
        /// <exception cref="System.ArgumentNullException">reader - reader returned no results</exception>
        public Account GetAccountWithUserName(String username)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();
            var query = "SELECT * FROM account WHERE username = @username";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            using var reader = command.ExecuteReader();
            int personIdOrdinal = reader.GetOrdinal("person_id");
            int accountTypeOrdinal = reader.GetOrdinal("account_type");
            int usernameOrdinal = reader.GetOrdinal("username");
            int hashedPasswordOrdinal = reader.GetOrdinal("hashed_password");
            return reader.Read() ? createAccount(reader, personIdOrdinal, accountTypeOrdinal, usernameOrdinal, hashedPasswordOrdinal) : throw new ArgumentNullException(nameof(reader), "reader returned no results");
        }

        #endregion

        #region Private Helper

        private static Account createAccount(MySqlDataReader reader, int personIdOrdinal, int accountTypeOrdinal,
            int usernameOrdinal, int hashedPasswordOrdinal)
        {
            return new Account(
                reader.GetFieldValueCheckNull<int>(personIdOrdinal),
                AccountTypeExtensions.FromString(reader.GetFieldValueCheckNull<String>(accountTypeOrdinal)),
                reader.GetFieldValueCheckNull<String>(usernameOrdinal),
                reader.GetFieldValueCheckNull<String>(hashedPasswordOrdinal)
            );
        }

        #endregion
    }
}
