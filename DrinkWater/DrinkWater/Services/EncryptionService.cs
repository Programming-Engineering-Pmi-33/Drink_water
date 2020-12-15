namespace DrinkWater.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Announces EncryptionService сlass.
    /// </summary>
    public class EncryptionService
    {
        /// <summary>
        /// Creates random salt.
        /// </summary>
        /// <returns>salt.</returns>
        public static long CreateRandomSalt()
        {
            var saltBytes = new byte[4];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltBytes);

            return (((long)saltBytes[0]) << 24) + (((long)saltBytes[1]) << 16) +
              (((long)saltBytes[2]) << 8) + ((long)saltBytes[3]);
        }

        /// <summary>
        /// Computes salted hash.
        /// </summary>
        /// <param name="password">password value.</param>
        /// <param name="salt">salt value.</param>
        /// <returns>salted hash.</returns>
        public static string ComputeSaltedHash(string password, long salt)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            var secretBytes = encoder.GetBytes(password);

            var saltBytes = new byte[4];
            saltBytes[0] = (byte)(salt >> 24);
            saltBytes[1] = (byte)(salt >> 16);
            saltBytes[2] = (byte)(salt >> 8);
            saltBytes[3] = (byte)salt;

            var toHash = new byte[secretBytes.Length + saltBytes.Length];
            Array.Copy(secretBytes, 0, toHash, 0, secretBytes.Length);
            Array.Copy(saltBytes, 0, toHash, secretBytes.Length, saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            byte[] computedHash = sha1.ComputeHash(toHash);

            return Convert.ToBase64String(computedHash);
        }
    }
}
