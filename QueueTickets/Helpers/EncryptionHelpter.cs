using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using QueueTickets.Models;

namespace QueueTickets.Helpers
{
    public class EncryptionHelpter
    {
        public static SaltAndHash Encrypt(string text)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var base64Salt = Convert.ToBase64String(salt);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: text,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return new SaltAndHash(base64Salt, hash);
        }


        /// <summary>
        /// Same as <see cref="Encrypt(string)"/> but uses supplied salt.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Encrypt(string text, string salt)
        {
            var byteArraySalt = Convert.FromBase64String(salt);
            
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: text,
                salt: byteArraySalt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hash;
        }
    }
}