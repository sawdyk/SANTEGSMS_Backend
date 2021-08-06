using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SANTEGSMS.Reusables
{
    public class ReportCardReUsables
    {
        private readonly AppDbContext _context;
        public ReportCardReUsables(AppDbContext context)
        {
            _context = context;
        }

        //-------------------------------------------------------------EXAMINATION SCORES----------------------------------------------------------------------------------------
        public IList<ExaminationScores> getExaminationScores(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, string admissionNumber, long termId, long sessionId)
        {
            IList<ExaminationScores> examinationScoreList = (from s in _context.ExaminationScores
                                                             where
                                                                 s.AdmissionNumber == admissionNumber &&
                                                                 s.StudentId == studentId &&
                                                                 s.SessionId == sessionId &&
                                                                 s.TermId == termId &&
                                                                 s.SchoolId == schoolId &&
                                                                 s.CampusId == campusId &&
                                                                 s.ClassId == classId &&
                                                                 s.ClassGradeId == classGradeId &&
                                                                 s.SubjectId == subjectId
                                                             select s).ToList();
            return examinationScoreList;
        }

        //-------------------------------------------------------------EXAMINATION SCORES PER CATEGORY AND SUBCATEGORY----------------------------------------------------------------------------------------
        public IList<ExaminationScores> getExaminationScoresPerCategory(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, string admissionNumber, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            IList<ExaminationScores> examinationScoreList = (from s in _context.ExaminationScores
                                                             where
                                                                 s.AdmissionNumber == admissionNumber &&
                                                                 s.StudentId == studentId &&
                                                                 s.SessionId == sessionId &&
                                                                 s.TermId == termId &&
                                                                 s.SchoolId == schoolId &&
                                                                 s.CampusId == campusId &&
                                                                 s.ClassId == classId &&
                                                                 s.ClassGradeId == classGradeId &&
                                                                 s.SubjectId == subjectId
                                                             select s).ToList();
            return examinationScoreList;
        }

        //-------------------------------------------------------------CONTINUOUS ASSWSSMENT SCORES----------------------------------------------------------------------------------------
        public IList<ContinousAssessmentScores> getContinuousAssessmentScores(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, string admissionNumber, long termId, long sessionId)
        {
            IList<ContinousAssessmentScores> CAScoreList = (from s in _context.ContinousAssessmentScores
                                                            where
                                                                s.AdmissionNumber == admissionNumber &&
                                                                s.StudentId == studentId &&
                                                                s.SessionId == sessionId &&
                                                                s.TermId == termId &&
                                                                s.SchoolId == schoolId &&
                                                                s.CampusId == campusId &&
                                                                s.ClassId == classId &&
                                                                s.ClassGradeId == classGradeId &&
                                                                s.SubjectId == subjectId
                                                            select s).ToList();
            return CAScoreList;
        }

        //-------------------------------------------------------------CONTINUOUS ASSWSSMENT SCORES PER CATEGORY AND SUBCATEGORY----------------------------------------------------------------------------------------
        public IList<ContinousAssessmentScores> getContinuousAssessmentScoresPerCategory(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, string admissionNumber, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            IList<ContinousAssessmentScores> CAScoreList = (from s in _context.ContinousAssessmentScores
                                                            where
                                                                s.AdmissionNumber == admissionNumber &&
                                                                s.StudentId == studentId &&
                                                                s.SessionId == sessionId &&
                                                                s.TermId == termId &&
                                                                s.SchoolId == schoolId &&
                                                                s.CampusId == campusId &&
                                                                s.ClassId == classId &&
                                                                s.ClassGradeId == classGradeId &&
                                                                s.SubjectId == subjectId &&
                                                                s.CategoryId == categoryId &&
                                                                s.SubCategoryId == subCategoryId
                                                            select s).ToList();
            return CAScoreList;
        }

        //-------------------------------------------------------------SCORE GRADING----------------------------------------------------------------------------------------

        //use this method to get the score grade Remark
        public string getExamGradeRemarks(decimal totalScore, long schoolId, long campusId, long classId)
        {
            try
            {
                string scoreRemark = string.Empty;

                var score = _context.ScoreGrading.Where(s => s.LowestRange <= totalScore &&
                                s.HighestRange >= totalScore &&
                                s.SchoolId == schoolId &&
                                 s.CampusId == campusId &&
                                s.ClassId == classId).FirstOrDefault();

                if (score != null)
                {
                    scoreRemark = score.Remark.ToString();
                }

                return scoreRemark;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //use this method to get the score grade Letter
        public string getExamGradeLetters(decimal totalScore, long schoolId, long campusId, long classId)
        {
            try
            {
                string scoreLetterGrade = string.Empty;

                var score = _context.ScoreGrading.Where(s => s.LowestRange <= totalScore &&
                                s.HighestRange >= totalScore &&
                                s.SchoolId == schoolId &&
                                 s.CampusId == campusId &&
                                s.ClassId == classId).FirstOrDefault();
                if (score != null)
                {
                    scoreLetterGrade = score.Grade.ToString();
                }

                return scoreLetterGrade;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //-------------------------------------------------------------GET THE TOTALSCORE FOR EACH SUBJECT PER TERM----------------------------------------------------------------------------------------
        public ReportCardData getStudentSubjectTotalScorePerTerm(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, long termId, long sessionId)
        {
            try
            {
                ReportCardData totalScore = (from s in _context.ReportCardData
                                             where
                                                            s.StudentId == studentId &&
                                                            s.SessionId == sessionId &&
                                                            s.TermId == termId &&
                                                            s.SchoolId == schoolId &&
                                                            s.CampusId == campusId &&
                                                            s.ClassId == classId &&
                                                            s.ClassGradeId == classGradeId &&
                                                            s.SubjectId == subjectId
                                             select s).FirstOrDefault();
                return totalScore;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Gets the Student Age from the Date of Birth
        public int getAge(DateTime Dob)
        {
            try
            {
                DateTime now = DateTime.Today;
                int age = now.Year - Dob.Year;
                if (Dob > now.AddYears(-age)) age--;

                return age;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Converts the GenderId to the respective Gender (e.g Male, Female)
        public string getGender(int genderId)
        {
            try
            {
                var gender = _context.Gender.SingleOrDefault(x => x.Id == genderId).GenderName;
                return gender.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------------------CLASS NAME------------------------------------------------------------------

        public string getStudentClass(Guid studentId, long schoolId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);

                string studentClass = string.Empty;

                //get School Current Session and Term
                long currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(schoolId);
                if (currentSessionId > 0 && checkSchool == true)
                {
                    //get the Student Class and ClassGrade
                    GradeStudents studentGrade = _context.GradeStudents.Where(x => x.StudentId == studentId && x.SessionId == currentSessionId).FirstOrDefault();

                    if (studentGrade != null)
                    {
                        Classes getClass = _context.Classes.Where(x => x.Id == studentGrade.ClassId).FirstOrDefault();
                        studentClass = getClass.ClassName;
                    }
                }

                return studentClass;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------------------CLASSGRADE NAME------------------------------------------------------------------

        public string getStudentClassGrade(Guid studentId, long schoolId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);

                string studentClassGrade = string.Empty;

                //get School Current Session and Term
                long currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(schoolId);
                if (currentSessionId > 0 && checkSchool == true)
                {
                    //get the Student Class and ClassGrade
                    GradeStudents studentGrade = _context.GradeStudents.Where(x => x.StudentId == studentId && x.SessionId == currentSessionId).FirstOrDefault();

                    if (studentGrade != null)
                    {
                        ClassGrades getClassGrade = _context.ClassGrades.Where(x => x.Id == studentGrade.ClassGradeId && x.ClassId == studentGrade.ClassId).FirstOrDefault();
                        if (getClassGrade != null)
                        {
                            studentClassGrade = getClassGrade.GradeName;
                        }
                    }
                }

                return studentClassGrade;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------------------SCHOOL TYPE NAME------------------------------------------------------------------

        public string getSchoolTypeName(long schoolId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);

                string schoolTypeName = string.Empty;
                if (checkSchool == true)
                {
                    //get the School
                    Schools sch = _context.Schools.Where(rp => rp.Id == schoolId).FirstOrDefault();
                    if (sch != null)
                    {
                        SchoolType schType = _context.SchoolType.Where(x => x.Id == sch.SchoolTypeId).FirstOrDefault();
                        if (schType != null)
                        {
                            schoolTypeName = schType.SchoolTypeName;
                        }
                    }
                }

                return schoolTypeName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------------------BEHAVIOURAL SCORES------------------------------------------------------------------

        public object getStudentBehaviouralScores(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            try
            {
                var result = (from ex in _context.BehavioralScores
                             where ex.SchoolId == schoolId && ex.CampusId == campusId
                               && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                               && ex.CategoryId == categoryId && ex.StudentId == studentId
                             select new
                             {
                                 ex.StudentId,
                                 ex.MarkObtained,
                                 ex.ScoreCategory.CategoryName,
                                 ex.ScoreSubCategoryConfig.SubCategoryName,
                             }).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------------------EXTRACURRICULAR SCORES------------------------------------------------------------------

        public object getStudentExtracurricularScores(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            try
            {
                var result = (from ex in _context.ExtraCurricularScores
                             where ex.SchoolId == schoolId && ex.CampusId == campusId
                              && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                              && ex.CategoryId == categoryId && ex.StudentId == studentId
                             select new
                             {
                                 ex.StudentId,
                                 ex.MarkObtained,
                                 ex.ScoreCategory.CategoryName,
                                 ex.ScoreSubCategoryConfig.SubCategoryName,
                             }).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------CUMULATIVE REPORT CARD DATA----------------------------------------------------------------

        public ReportCardPosition getStudentCumulativeReportCardData(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId)
        {
            try
            {
                ReportCardPosition rptPosition = null;

                ReportCardPosition rptPos = _context.ReportCardPosition.Where(rp => rp.StudentId == studentId && rp.SchoolId == schoolId && rp.CampusId == campusId
                && rp.ClassId == classId && rp.ClassGradeId == classGradeId && rp.TermId == termId && rp.SessionId == sessionId).FirstOrDefault();
                if (rptPos != null)
                {
                    rptPosition = rptPos;
                }

                return rptPosition;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------REPORT CARD LEGEND--------------------------------------------------------------------------

        public object getReportCardLegend(long schoolId, long campusId, long termId)
        {
            try
            {
                //get the legend and the legendList
                var result = (from rp in _context.ReportCardConfigurationLegend.AsNoTracking()
                                .Include(l => l.ReportCardConfigurationLegendList).AsNoTracking()
                             where rp.TermId == termId && rp.SchoolId == schoolId && rp.CampusId == campusId
                             select new
                             {
                                 rp.Id,
                                 rp.LegendName,
                                 rp.SchoolId,
                                 rp.CampusId,
                                 rp.TermId,
                                 rp.Terms.TermName,
                                 rp.CreatedBy,
                                 rp.DateCreated,
                                 rp.LastUpdatedBy,
                                 rp.LastUpdatedDate,
                                 rp.StatusId,
                                 rp.ActiveInActiveStatus.StatusName,
                                 rp.ReportCardConfigurationLegendList,
                             }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------CLASS TEACHER AND PRINCIPALS COMMENT AND REMARK----------------------------------------------------------------
        public ReportCardComments getClassTeacherAndPrincipalCommentsAndRemark(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long commentConfigId, long termId, long sessionId)
        {
            try
            {
                ReportCardComments rptComments = null;

                ReportCardComments rptCom = _context.ReportCardComments.Where(rp => rp.StudentId == studentId && rp.SchoolId == schoolId && rp.CampusId == campusId
                && rp.ClassId == classId && rp.ClassGradeId == classGradeId && rp.CommentConfigId == commentConfigId && rp.TermId == termId && rp.SessionId == sessionId).FirstOrDefault();

                if (rptCom != null)
                {
                    rptComments = rptCom;
                }

                return rptComments;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------CLASS TEACHER FULLNAME-------------------------------------------------------------------------------

        public string getClassTeacherFullName(long schoolId, long campusId, long classId, long classGradeId)
        {
            try
            {
                string classTeacherFullName = string.Empty;

                GradeTeachers grdTeachers = _context.GradeTeachers.Where(x => x.SchoolId == schoolId && x.CampusId == campusId
                && x.ClassId == classId && x.ClassGradeId == classGradeId).FirstOrDefault();

                if (grdTeachers != null)
                {
                    SchoolUsers schUsers = _context.SchoolUsers.Where(x => x.Id == grdTeachers.SchoolUserId
                    && x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefault();

                    classTeacherFullName = schUsers.FirstName + " " + schUsers.LastName;
                }

                return classTeacherFullName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------NUMBER OF STUDENTS IN CLASS-------------------------------------------------------------------------------

        public long getNumberOfStudentInClass(long schoolId, long campusId, long classId, long classGradeId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);

                long noOfStudentInClass = 0;

                //get School Current Session and Term
                long currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(schoolId);
                if (currentSessionId > 0 && checkSchool == true)
                {
                    //get the Student Class and ClassGrade
                    IList<GradeStudents> grdStudents = (_context.GradeStudents.Where(x => x.ClassId == classId && x.ClassGradeId == classGradeId && x.SchoolId == schoolId
                    && x.CampusId == campusId && x.SessionId == currentSessionId)).ToList();

                    if (grdStudents.Count() > 0)
                    {
                        noOfStudentInClass = grdStudents.Count();
                    }
                }

                return noOfStudentInClass;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------------------CURRENT SESSION NAME------------------------------------------------------------------

        public string getCurrentSessionName(long schoolId)
        {
            try
            {
                string sessionName = string.Empty;

                AcademicSessions acdSession = _context.AcademicSessions.Where(x => x.SchoolId == schoolId && x.IsCurrent == true).FirstOrDefault();
                if (acdSession != null)
                {
                    Sessions session = _context.Sessions.Where(x => x.Id == acdSession.SessionId).FirstOrDefault();
                    sessionName = session.SessionName;
                }

                return sessionName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------------------NEXT TERM BEGINS DATE------------------------------------------------------------------

        public string getNextTermBeginsDate(long schoolId, long campusId)
        {
            try
            {
                string nextTermBeginsDate = string.Empty;

                ReportCardNextTermBegins rptNxtTermBegins = _context.ReportCardNextTermBegins.Where(x => x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefault();
                if (rptNxtTermBegins != null)
                {
                    nextTermBeginsDate = rptNxtTermBegins.NextTermBeginsDate.ToString();
                }

                return nextTermBeginsDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //------------------------------------------------------------------TERM NAME------------------------------------------------------------------

        public string getTermName(long termId)
        {
            try
            {
                string termName = string.Empty;

                Terms terms = _context.Terms.Where(x => x.Id == termId).FirstOrDefault();
                if (terms != null)
                {
                    termName = terms.TermName;
                }

                return termName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //------------------------------------------------------------------REPORT CARD SIGNATURE------------------------------------------------------------------

        public string getReportCardSignature(long schoolId, long campusId)
        {
            try
            {
                string signatureURL = string.Empty;

                ReportCardSignature rptSign = _context.ReportCardSignature.Where(x => x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefault();
                if (rptSign != null)
                {
                    signatureURL = rptSign.SignatureUrl;
                }

                return signatureURL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ToOrdinal(int value)
        {
            try
            {
                // Start with the most common extension.
                string extension = "th";

                // Examine the last 2 digits.
                int last_digits = value % 100;

                // If the last digits are 11, 12, or 13, use th. Otherwise:
                if (last_digits < 11 || last_digits > 13)
                {
                    // Check the last digit.
                    switch (last_digits % 10)
                    {
                        case 1:
                            extension = "st";
                            break;
                        case 2:
                            extension = "nd";
                            break;
                        case 3:
                            extension = "rd";
                            break;
                    }
                }

                return extension;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}