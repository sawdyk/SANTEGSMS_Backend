using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SANTEGSMS.Reusables
{
    public class AdmissionNumberGenerator
    {
        private readonly AppDbContext _context;
        public AdmissionNumberGenerator(AppDbContext context)
        {
            _context = context;
        }

        //all students in school
        public IList<Students> getAllStudentsInSchool(long schoolId)
        {
            try
            {
                var allStudents = from usr in _context.Students where usr.SchoolId == schoolId select usr;
                return allStudents.ToList();
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //Appends Zero to the SchoolCode
        public static string AppendZeroToNumber(long ActualNumber, long requiredLenght)
        {
            try
            {
                StringBuilder str = new StringBuilder();

                for (int i = 0; i < (requiredLenght - ActualNumber.ToString().Length); ++i)
                {
                    str.Append("0");
                }
                return str.Append(ActualNumber.ToString()).ToString();
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //school
        public Schools RetrieveSchoolCode(long schoolId)
        {
            try
            {
                var sch = _context.Schools.SingleOrDefault(s => s.Id == schoolId);
                return sch;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //Generate the student unique AdmissionNumber/Username
        public string GenerateAdmissionNumber(long schoolId)
        {
            try
            {
                //the schoolCode
                var schoolCode = RetrieveSchoolCode(schoolId).SchoolCode;

                //returns the generated admissionNumber
                return new StringBuilder().Append(schoolCode.ToUpper()).Append(AppendZeroToNumber(getAllStudentsInSchool(schoolId).Count + 1, 6)).ToString();
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
    }
}
