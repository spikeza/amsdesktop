using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace AMSDesktop.Helpers
{
    public class CryptographyHelper
    {
        public string GetGeneratedSalt()
        {
            byte[] salt = new byte[256 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        public string GetHashedString(string param)
        {
            HashAlgorithm hashAlg = new SHA256CryptoServiceProvider();
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(param);
            byte[] bytHash = hashAlg.ComputeHash(bytValue);
            return Convert.ToBase64String(bytHash);
        }
    }
}
