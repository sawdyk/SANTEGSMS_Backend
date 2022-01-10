using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Reusables
{
    public class BroadSheetReusables
    {
        private readonly AppDbContext _context;
        public BroadSheetReusables(AppDbContext context)
        {
            _context = context;
        }
       
        public string othersPassedRemark(int countOthersPassed)
        {
            string remark = "Failed";
            if (countOthersPassed >= 4)
            {
                remark = "Passed";
            }
            return remark;
        }

        public string mandatoryPassedRemark(int countMandatoryPassed, int totalMandatoryConfig)
        {
            string remark = "Failed";
            if (countMandatoryPassed == totalMandatoryConfig)
            {
                remark = "Passed";
            }
            return remark;
        }

        public string studentFinalRemark(string mandatoryRemark, string othersRemark)
        {
            string remark = "Failed";
            if (mandatoryRemark.Trim().Equals("Passed") && othersRemark.Trim().Equals("Passed"))
            {
                remark = "Passed";
            }
            return remark;
        }

        public string subjectName(long subjectId)
        {
            string subjectName = string.Empty;
            var schSubject = _context.SchoolSubjects.Where(c => c.Id == subjectId).FirstOrDefault();

            if (schSubject != null)
            {
                subjectName = schSubject.SubjectName;
            }
            return subjectName;
        }

        public string getClass(long classId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkClass = check.checkClassById(classId);
                string className = string.Empty;

                if (checkClass == true)
                {
                    Classes getClass = _context.Classes.Where(x => x.Id == classId).FirstOrDefault();
                    className = getClass.ClassName;
                }
                return className;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getClassGrade(long classGradeId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkClassGrade = check.checkClassGradeById(classGradeId);
                string classGradeName = string.Empty;

                if (checkClassGrade == true)
                {
                    ClassGrades getClassGrade = _context.ClassGrades.Where(x => x.Id == classGradeId).FirstOrDefault();
                    classGradeName = getClassGrade.GradeName;
                }

                return classGradeName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //use this method to get the score grade Remark
        public string getBroadSheetGrade(decimal percentageScore, long schoolId, long campusId, long sessionId)
        {
            try
            {
                string scoreGrade = string.Empty;
                var score = _context.BroadSheetGrading.Where(s => s.LowestRange <= percentageScore && s.HighestRange >= percentageScore 
                && s.SchoolId == schoolId && s.CampusId == campusId && s.SessionId == sessionId).FirstOrDefault();

                if (score != null)
                {
                    scoreGrade = score.Grade.ToString();
                }

                return scoreGrade;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------NUMBER OF STUDENTS IN CLASSBY GENDER-------------------------------------------------------------------------------

        public long getNumberOfStudentInClassByGender(long schoolId, long campusId, long classId, long genderId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);

                long noOfStudentByGender = 0;

                //get School Current Session and Term
                long currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(schoolId);
                if (currentSessionId > 0 && checkSchool == true)    
                {
                    //all students in school, class by gender
                    var students = from std in _context.Students where std.SchoolId == schoolId && std.CampusId == campusId && std.GenderId == genderId select std;

                     //get the Student Class and ClassGrade
                    var grdStudents = from grd in _context.GradeStudents where grd.ClassId == classId && grd.SchoolId == schoolId
                      && grd.CampusId == campusId && grd.SessionId == currentSessionId select grd.StudentId;

                    //subQuery 
                    var stdGenderCount = students.Where(x=> grdStudents.Contains(x.Id));

                    if (stdGenderCount.Count() > 0)
                    {
                        noOfStudentByGender = stdGenderCount.Count();
                    }
                }

                return noOfStudentByGender;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         //------------------------------------------------NUMBER OF MANDATORY/OTHERS SUBJECTS PASSED/FAILED-------------------------------------------------------------------------------

        public long getNumberOfStudentsPassedOrFailedByGender(long schoolId, long campusId, long sessionId, long termId, long classId, long genderId, string remark)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                long noOfStudents = 0;

                if (checkSchool && checkCampus && checkTerm && checkSession)    
                {
                    //students that has passed or failed as remark by gender
                    var studentsCount = from std in _context.BroadSheetData where std.SchoolId == schoolId && std.CampusId == campusId  && std.ClassId == classId 
                                   && std.SessionId == sessionId && std.TermId == termId && std.GenderId == genderId && std.Remark == remark.Trim() select std;                    

                    if (studentsCount.Count() > 0)
                    {
                        noOfStudents = studentsCount.Count();
                    }
                }

                return noOfStudents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
