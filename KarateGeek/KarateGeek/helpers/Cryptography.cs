using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace KarateGeek.helpers
{
    class Cryptography
    {
        public string cryptography(string pass)
        {
            string password = null;
            string salt = "k1k1k1k1";
            string sha1pass = null;
            string md5pass = null;

            pass = salt + pass;
            sha1pass = sha1Encrypt(pass);
            md5pass = md5Encrypt(sha1pass);
            password = md5pass;

            return password;
        }

        public string sha1Encrypt(string data)
        {
            SHA1 sha1 = SHA1.Create();

            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            return returnValue.ToString();
        }

        private string md5Encrypt(string data)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash. 
            byte[] hashData = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < hashData.Length; i++)
            {
                sBuilder.Append(hashData[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }
    }
}
