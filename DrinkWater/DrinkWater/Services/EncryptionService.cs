﻿namespace DrinkWater.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class EncryptionService
    {
        public static int CreateRandomSalt()
        {
            byte[] _saltBytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(_saltBytes);

            return (((int)_saltBytes[0]) << 24) + (((int)_saltBytes[1]) << 16) +
              (((int)_saltBytes[2]) << 8) + ((int)_saltBytes[3]);
        }

        public static string ComputeSaltedHash(string password, int salt)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] _secretBytes = encoder.GetBytes(password);

            byte[] _saltBytes = new byte[4];
            _saltBytes[0] = (byte)(salt >> 24);
            _saltBytes[1] = (byte)(salt >> 16);
            _saltBytes[2] = (byte)(salt >> 8);
            _saltBytes[3] = (byte)salt;

            byte[] toHash = new byte[_secretBytes.Length + _saltBytes.Length];
            Array.Copy(_secretBytes, 0, toHash, 0, _secretBytes.Length);
            Array.Copy(_saltBytes, 0, toHash, _secretBytes.Length, _saltBytes.Length);

            SHA1 sha1 = SHA1.Create();
            byte[] computedHash = sha1.ComputeHash(toHash);

            return encoder.GetString(computedHash);
        }
    }
}
