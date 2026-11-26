using System.Security.Cryptography;
using System.Text;

namespace TrainingShippingSystem.BLL
{
    // Password hashing: PBKDF2 (HMAC-SHA256) with a random salt per user.
    internal static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public static void CreateHash(string password, out byte[] hash, out byte[] salt)
        {
            salt = new byte[SaltSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            hash = Derive(password, salt);
        }

        public static bool Verify(string password, byte[] salt, byte[] expectedHash)
        {
            byte[] actual = Derive(password, salt);
            return FixedTimeEquals(actual, expectedHash);
        }

        // Old accounts were stored as plain SHA-256 with no salt.
        public static bool VerifyLegacy(string password, byte[] expectedHash)
        {
            byte[] actual;

            using (var sha = SHA256.Create())
            {
                actual = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            return FixedTimeEquals(actual, expectedHash);
        }

        private static byte[] Derive(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        // Compares without stopping at the first different byte (timing-attack safe).
        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int diff = 0;

            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];

            return diff == 0;
        }
    }
}