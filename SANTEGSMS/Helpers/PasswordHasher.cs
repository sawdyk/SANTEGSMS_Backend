using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SANTEGSMS.Helpers
{
    public class PasswordHasher
    {
        //This Method is Used to hash the Password

        private static string getHash(string password)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    // Send a sample text to hash.  
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    // Get the hashed string.  
                    var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                    // return the string.   
                    return hash;
                }
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //This Method is Used to generate a random string to be used as the salt

        public string getSalt()
        {
            try
            {
                byte[] bytes = new byte[128 / 8];
                using (var keyGenerator = RandomNumberGenerator.Create())
                {
                    keyGenerator.GetBytes(bytes);
                    return BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //This Method is Used to concatenate the Password and Salt before hashing

        public string hashedPassword(string password, string salt)
        {
            try
            {
                string hashPassword = getHash(password + salt);

                return hashPassword;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //This Method is Used to Decrpyt a hashed Password
        public string decryptHashedPassword(string password, string salt)
        {
            try
            {
                string hashPassword = getHash(password + salt);

                return hashPassword;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
    }
}
