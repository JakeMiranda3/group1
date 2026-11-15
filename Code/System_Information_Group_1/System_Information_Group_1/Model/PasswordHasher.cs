using System;
using System.Security.Cryptography;


namespace System_Information_Group_1.Model
{
    /// <summary>
    /// The password hasher class provides methods for hashing and verifying passwords using PBKDF2 with SHA256.
    ///
    /// Credits:
    /// SHA-256 was designed by the U.S. National Security Agency (NSA) and published by the
    /// National Institute of Standards and Technology (NIST) in FIPS 180-2 (2001), now FIPS 180-4,
    /// "Secure Hash Standard (SHS)": https://csrc.nist.gov/pubs/fips/180-4/upd1/final
    /// PBKDF2 was designed by Burt Kaliski of RSA Laboratories and published in
    /// RFC 2898, "PKCS #5: Password-Based Cryptography Specification Version 2.0" (2000):
    /// https://www.rfc-editor.org/rfc/rfc2898
    /// The implementation uses .NET's Rfc2898DeriveBytes class:
    /// https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes
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