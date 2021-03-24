using SANTEGSMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Reusables
{
    public class Converters
    {
        public static EnumUtility.Gender stringToGender(string genderString)
        {
            EnumUtility.Gender enumObj = EnumUtility.Gender.Male;
            switch (genderString.Trim().ToLower())
            {
                case "f":
                    enumObj = EnumUtility.Gender.Female;
                    break;
                case "m":
                    enumObj = EnumUtility.Gender.Male;
                    break;
            }
            return enumObj;
        }
    }
}
