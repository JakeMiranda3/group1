using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MySqlConnector;

namespace System_Information_Group_1.DAL;

/// <summary>
///     The connection class is responsible for providing the connection string to connect to the MySQL database.
///     @author Mrs.Yang
/// </summary>
public static class Connection
{
    #region Methods

    /// <summary>
    ///     Gets the string to connect to DB from the .env file.
    /// </summary>
    /// <returns>A connection string to MySQL DB</returns>
    public static string ConnectionString()
    {
        var env = loadDotEnv();

        var builder = new MySqlConnectionStringBuilder
        {
            Server = env["DB_SERVER"],
            Database = env["DB_NAME"],
            UserID = env["DB_USER"],
            Password = env["DB_PASSWORD"],
            Port = env.TryGetValue("DB_PORT", out var port) ? uint.Parse(port) : 3306
        };

        return builder.ToString();
    }

    private static Dictionary<string, string> loadDotEnv()
    {
        var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, ".env")))
        {
            directory = directory.Parent;
        }

        if (directory == null)
        {
            throw new FileNotFoundException("Fichier .env introuvable.");
        }

        return File.ReadAllLines(Path.Combine(directory.FullName, ".env"))
            .Select(line => line.Trim())
            .Where(line => line.Length > 0 && !line.StartsWith("#") && line.Contains("="))
            .Select(line => line.Split(new[] { '=' }, 2))
            .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim().Trim('"'));
    }

    #endregion
}