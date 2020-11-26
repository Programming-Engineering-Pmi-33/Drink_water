using System;
using System.Security.Cryptography;
using System.Text;

namespace DrinkWater.Services
{
    public class EncryptionService
    {
        //creation of random salt
        public static int CreateRandomSalt()
        {
            Byte[] saltBytes = new Byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(saltBytes);

            return ((((int)saltBytes[0]) << 24) + (((int)saltBytes[1]) << 16) +
              (((int)saltBytes[2]) << 8) + ((int)saltBytes[3]));
        }

        //computing of salted hash
        public static string ComputeSaltedHash(string password, int salt)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            Byte[] secretBytes = encoder.GetBytes(password);

            Byte[] saltBytes = new Byte[4];
            saltBytes[0] = (byte)(salt >> 24);
            saltBytes[1] = (byte)(salt >> 16);
            saltBytes[2] = (byte)(salt >> 8);
            saltBytes[3] = (byte)(salt);

            Byte[] toHash = new Byte[secretBytes.Length + saltBytes.Length];
            Array.Copy(secretBytes, 0, toHash, 0, secretBytes.Length);
            Array.Copy(saltBytes, 0, toHash, secretBytes.Length, saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            Byte[] computedHash = sha1.ComputeHash(toHash);

            return Convert.ToBase64String(computedHash);
        }
    }
}
