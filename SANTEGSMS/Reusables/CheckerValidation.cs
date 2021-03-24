using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using SANTEGSMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Reusables
{
    public class CheckerValidation
    {
        private readonly AppDbContext _context;

        public CheckerValidation(AppDbContext context)
        {
            _context = context;
        }
        public bool checkIfEmailExist(string email, long userCategory)
        {
            try
            {
                bool emailExist = false;
             
                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers))
                {
                    SchoolUsers SchoolUsers = _context.SchoolUsers.Where(u => u.Email == email).FirstOrDefault();
                    if (SchoolUsers != null)
                    {
                        emailExist = true;
                    }
                }

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.Parents))
                {
                    Parents parents = _context.Parents.Where(u => u.Email == email).FirstOrDefault();
                    if (parents != null)
                    {
                        emailExist = true;
                    }
                }
              
                return emailExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //---------------------Checks if an Account exists and not confirmed------------------------------------------

        public bool checkIfAccountExistAndNotConfirmed(string email, long userCategory)
        {
            try
            {
                bool accountExistButNotConfirmed = false;

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers)) //School Users
                {
                    SchoolUsers schoolUsers = _context.SchoolUsers.Where(u => u.Email == email && u.EmailConfirmed == false).FirstOrDefault();
                    if (schoolUsers != null)
                    {
                        accountExistButNotConfirmed = true;
                    }
                }

                return accountExistButNotConfirmed;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }

        }

        //-------------------------------Checks if a School Name exist------------------------------------------

        public bool checkIfSchoolNameExist(string schoolName)
        {
            try
            {
                bool schoolExist = false;

                Schools schInfo = _context.Schools.Where(s => s.SchoolName == schoolName).FirstOrDefault();
                if (schInfo != null)
                {
                    schoolExist = true;
                }

                return schoolExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }


        //-------------------------------Checks if a School Code exist------------------------------------------

        public bool checkIfSchoolCodeExist(string schoolCode)
        {
            try
            {
                bool schoolCodeExist = false;

                Schools schInfo = _context.Schools.Where(s => s.SchoolCode == schoolCode).FirstOrDefault();
                if (schInfo != null)
                {
                    schoolCodeExist = true;
                }

                return schoolCodeExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //-------------------------------Checks if a Campus Name exist------------------------------------------

        public bool checkIfSchoolCampusNameExist(string campusName)
        {
            try
            {
                bool campusExist = false;

                SchoolCampus camp = _context.SchoolCampus.Where(s => s.CampusName == campusName).FirstOrDefault();
                if (camp != null)
                {
                    campusExist = true;
                }

                return campusExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a parent exists
        public bool checkParentById(Guid parentId)
        {
            try
            {
                bool itExists = false;

                var result = _context.Parents.Where(s => s.Id == parentId).FirstOrDefault();
                if (result != null)
                {
                    itExists = true;
                }

                return itExists;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a student exists
        public bool checkStudentById(Guid studentId)
        {
            try
            {
                bool itExists = false;

                var result = _context.Students.Where(s => s.Id == studentId).FirstOrDefault();
                if (result != null)
                {
                    itExists = true;
                }

                return itExists;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a school exists
        public bool checkSchoolById(long schoolId)
        {
            try
            {
                bool itExists = false;

                var result = _context.Schools.Where(s => s.Id == schoolId).FirstOrDefault();
                if (result != null)
                {
                    itExists = true;
                }

                return itExists;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Class exists
        public bool checkClassById(long classId)
        {
            try
            {
                bool classExist = false;

                Classes cls = _context.Classes.Where(s => s.Id == classId).FirstOrDefault();
                if (cls != null)
                {
                    classExist = true;
                }

                return classExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Class Grade exists
        public bool checkClassGradeById(long classGradeId)
        {
            try
            {
                bool classGradeExist = false;

                ClassGrades clsGrd = _context.ClassGrades.Where(s => s.Id == classGradeId).FirstOrDefault();
                if (clsGrd != null)
                {
                    classGradeExist = true;
                }

                return classGradeExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if Session exists
        public bool checkSessionById(long sessionId)
        {
            try
            {
                bool sessionExist = false;

                Sessions crs = _context.Sessions.Where(s => s.Id == sessionId).FirstOrDefault();
                if (crs != null)
                {
                    sessionExist = true;
                }

                return sessionExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if term exists
        public bool checkTermById(long termId)
        {
            try
            {
                bool termExist = false;

                Terms crs = _context.Terms.Where(s => s.Id == termId).FirstOrDefault();
                if (crs != null)
                {
                    termExist = true;
                }

                return termExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a school campus exists
        public bool checkSchoolCampusById(long schoolCampusId)
        {
            try
            {
                bool itExists = false;

                var result = _context.SchoolCampus.Where(s => s.Id == schoolCampusId).FirstOrDefault();
                if (result != null)
                {
                    itExists = true;
                }

                return itExists;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
    }
}
