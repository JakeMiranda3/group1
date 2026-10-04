

using MySqlConnector;

namespace System_Information_Group_1.DAL
{
    /// <summary>
    /// The connection class is responsible for providing the connection string to connect to the MySQL database. It uses the MySqlConnectionStringBuilder to construct the connection string with the necessary properties such as server address, database name, username, password, and port.
    /// @author Mrs.Yang
    /// </summary>
    public static class Connection
    {
        /// <summary>
        /// Constructs the string to connect to DB.
        /// </summary>
        /// <returns>A connection string to MySQL DB</returns>
         public static string ConnectionString()
        {
            var builder = new MySqlConnectionStringBuilder();

            // Set the connection string properties
            builder.Server = "localhost";    // MySQL server address
            builder.Database = "cs3230f26_g1";        // Database name
            builder.UserID = "cs3230f26_g1";          // MySQL username
            builder.Password = "1X,S<ip+R_;CC>oWv9a5";        // MySQL password
            builder.Port = 3307;                    // MySQL port (default: 3306)

            // Get the constructed connection string
            return builder.ToString();

        }
    }
}
