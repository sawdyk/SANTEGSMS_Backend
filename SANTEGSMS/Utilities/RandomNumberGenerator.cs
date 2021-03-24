using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Utilities
{
    public class RandomNumberGenerator
    {
        //For generating random numbers for Email Confirmaton Codes
        public string randomCodesGen()
        {
            try
            {
                Random rnd = new Random();
                int codes = rnd.Next(100000, 900000);

                return codes.ToString();
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }

        }

        //For generating the cart referenceId
        private static Random random = new Random();
        public static string RandomString(int length = 10)
        {
            try
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
    }
}
