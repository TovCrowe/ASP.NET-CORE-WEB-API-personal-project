using System;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication7.Resources
{
    public class Utilities
    {
        public static string EncryptKey(string password)
        {
            using (SHA256 sha256 = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] data = enc.GetBytes(password);
                byte[] hash = sha256.ComputeHash(data);

                // Convierte el hash a una representación hexadecimal
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2")); // "x2" significa formato hexadecimal
                }

                return sb.ToString();
            }
        }
    }
}
