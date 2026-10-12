using System;
using System.Security.Cryptography;


namespace System_Information_Group_1.Model
{
    /// <summary>
    /// The password hasher class provides methods for hashing and verifying passwords using PBKDF2 with SHA256.
    /// @author Colby
    /// @version Fall 2026
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 210_000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        /// <summary>
        /// Hashes the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>returns the hashed version of the password</returns>
        public static string Hash(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations, Algorithm))
            {
                byte[] key = deriveBytes.GetBytes(KeySize);
                return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
            }
        }

        /// <summary>
        /// Verifies the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="stored">The stored.</param>
        /// <returns>True or false based on if the password was correct</returns>
        public static bool Verify(string password, string stored)
        {
            var parts = stored.Split('.');
            if (parts.Length != 3 || !int.TryParse(parts[0], out int iterations)) {
                return false;
            }
            
        

            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] expected = Convert.FromBase64String(parts[2]);
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, Algorithm))
            {
                byte[] actual = deriveBytes.GetBytes(expected.Length);

                return fixedTimeEquals(actual, expected);

            }
        }

        private static bool fixedTimeEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length)
            {
                return false;
            }

            int diff = 0;
            for (int i = 0; i < left.Length; i++)
            {
                diff |= left[i] ^ right[i];
            }
            return diff == 0;
        }
    }
}