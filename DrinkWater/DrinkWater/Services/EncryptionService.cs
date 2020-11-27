namespace DrinkWater.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class EncryptionService
    {
        // creation of random salt
        public static long CreateRandomSalt()
        {
            var saltBytes = new byte[4];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltBytes);

            return (((long)saltBytes[0]) << 24) + (((long)saltBytes[1]) << 16) +
              (((long)saltBytes[2]) << 8) + ((long)saltBytes[3]);
        }

        // computing of salted hash
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
