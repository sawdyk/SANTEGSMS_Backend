using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using SANTEGSMS.IRepos;
using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using SANTEGSMS.Reusables;
using SANTEGSMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Repos
{
    public class ReportCardGenerateRepo : IReportCardDataGenerateRepo
    {
        private readonly AppDbContext _context;
        private readonly ReportCardReUsables reportCardReUsables;

        public ReportCardGenerateRepo(AppDbContext context, ReportCardReUsables reportCardReUsables)
        {
            _context = context;
            this.reportCardReUsables = reportCardReUsables;
        }

        //------------------------------------------PRINTING AND GENERATING REPORT CARD INFORMATION--------------------------------------

        public async Task<ReportCardDataRespModel> getReportCardDataByStudentIdAsync(ReportCardDataReqModel obj)
        {
            try
            {
                ReportCardDataRespModel response = new ReportCardDataRespModel();
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);
                var checkTerm = check.checkTermById(obj.TermId);
                var checkSession = check.checkSessionById(obj.SessionId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true && checkTerm == true && checkSession == true)
                {
                    IList<object> responseList = new List<object>();

                    //get the report Card Configuration as configured by each school
                    ReportCardConfiguration rptConfig = _context.ReportCardConfiguration.Where(rp => rp.SchoolId == obj.SchoolId && rp.CampusId == obj.CampusId && rp.TermId == obj.TermId).FirstOrDefault();   //selects all the courses in the cart

                    if (rptConfig != null)
                    {
                        foreach (Guid studentId in obj.StudentIds)
                        {
                            StudentResult studentResult = new StudentResult();
                            IList<Result> ResultList = new List<Result>();

                            //instance of objects to be used to return data
                            ReportCardTemplateInfo ReportCardTemplateInfo = new ReportCardTemplateInfo();
                            SchoolInfo SchoolInfo = new SchoolInfo();
                            StudentInfo StudentInfo = new StudentInfo();
                            ReportCardResult ReportCardResult = new ReportCardResult();
                            IList<object> ExtraCurricularData = new List<object>();
                            IList<object> BehaviouralData = new List<object>();
                            CumulativeReportCardData CumulativeReportCardData = new CumulativeReportCardData();
                            object LegendData = new object();
                            RemarksAndCommentData RemarksAndCommentData = new RemarksAndCommentData();
                            OtherData OtherData = new OtherData();
                            //------------------------------------------REPORT CARD HEADER INFORMATION-------------------------------------------------------

                            ReportCardHeaderInfo rptCardHeaderInfo = new ReportCardHeaderInfo();

                            if (rptConfig.ShowDepartment == true)
                            {
                                rptCardHeaderInfo.Department = "DEPARTMENT";
                            }
                            if (rptConfig.RefFirstTermScoreShow == true && rptConfig.RefSecondTermScoreShow == true)
                            {
                                rptCardHeaderInfo.FirstTermScore = "FIRST TERM SCORE";
                                rptCardHeaderInfo.SecondTermScore = "SECOND TERM SCORE";
                            }
                            if (rptConfig.ShowCA_Cumulative == true)
                            {
                                rptCardHeaderInfo.CA_Cumulative = "CA CUMULATIVE";
                            }

                            //get the score Category Configured by the school to fetch the Continuous Assessments (e.g. CA1, CA2 etc)
                            var CA_SubCategory = (from x in _context.ScoreSubCategoryConfig
                                                  where x.CategoryId == (int)EnumUtility.ScoreCategory.CA
                                                   && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId
                                                   && x.TermId == obj.TermId && x.SessionId == obj.SessionId
                                                  select new
                                                  {
                                                      x.Id,
                                                      x.SubCategoryName
                                                  }).OrderBy(s => s.Id).ToList();

                            //add the CA Subcategory to the report card header information
                            rptCardHeaderInfo.CA_SubCategory = CA_SubCategory;

                            //------------------------------------------REPORT CARD TEMPLATE-------------------------------------------------------

                            ReportCardTemplates rptTemplate = _context.ReportCardTemplates.Where(rp => rp.ClassId == obj.ClassId && rp.SchoolId == obj.SchoolId && rp.CampusId == obj.CampusId).FirstOrDefault();
                            if (rptTemplate != null)
                            {
                                ReportCardTemplateInfo.ClassId = rptTemplate.ClassId;
                                ReportCardTemplateInfo.SchoolSubTypeId = rptTemplate.SchoolSubTypeId;
                                ReportCardTemplateInfo.CampusId = rptTemplate.CampusId;
                                ReportCardTemplateInfo.SchoolId = rptTemplate.SchoolId;
                                ReportCardTemplateInfo.Description = rptTemplate.Description;
                            }

                            //------------------------------------------SCHOOL INFORMATION-------------------------------------------------------

                            Schools rptSchInfo = _context.Schools.Where(rp => rp.Id == obj.SchoolId).FirstOrDefault();
                            if (rptSchInfo != null)
                            {
                                SchoolInfo.SchoolName = rptSchInfo.SchoolName;
                                SchoolInfo.SchoolType = reportCardReUsables.getSchoolTypeName(obj.SchoolId);
                                SchoolInfo.LogoUrl = rptSchInfo.SchoolLogoUrl;
                                SchoolInfo.PhoneNumber = rptSchInfo.PhoneNumber;
                                SchoolInfo.Address = rptSchInfo.Address;
                                SchoolInfo.EmailAddress = rptSchInfo.EmailAddress;
                                SchoolInfo.Motto = rptSchInfo.Motto;
                            }

                            //------------------------------------------STUDENT INFORMATION-------------------------------------------------------

                            Students rptStudentInfo = _context.Students.Where(rp => rp.Id == studentId && rp.SchoolId == obj.SchoolId && rp.CampusId == obj.CampusId).FirstOrDefault();

                            StudentInfo.StudentId = rptStudentInfo.Id;
                            StudentInfo.FirstName = rptStudentInfo.FirstName;
                            StudentInfo.LastName = rptStudentInfo.LastName;
                            StudentInfo.MiddleName = rptStudentInfo.LastName;
                            if (rptStudentInfo.GenderId > 0)
                            {
                                StudentInfo.Gender = reportCardReUsables.getGender((int)rptStudentInfo.GenderId);
                            }
                            StudentInfo.FullName = rptStudentInfo.FirstName + " " + rptStudentInfo.LastName + " " + rptStudentInfo.MiddleName;
                            StudentInfo.DateOfBirth = rptStudentInfo.DateOfBirth;
                            StudentInfo.StudentPassport = rptStudentInfo.ProfilePictureUrl;
                            if (rptStudentInfo.DateOfBirth != null)
                            {
                                StudentInfo.Age = reportCardReUsables.getAge(Convert.ToDateTime(rptStudentInfo.DateOfBirth));
                            }
                            StudentInfo.Class = reportCardReUsables.getStudentClass(studentId, obj.SchoolId);
                            StudentInfo.ClassGrade = reportCardReUsables.getStudentClassGrade(studentId, obj.SchoolId);


                            //check if the school configured the report to show and use department
                            if (rptConfig.ShowDepartment == true)
                            {
                                //get the department created by the schools (for schools using subjects by department)
                                var department = (from s in _context.SubjectDepartment where s.ClassId == obj.ClassId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId select s).ToList();

                                if (department.Count() > 0)
                                {
                                    foreach (SubjectDepartment dept in department)
                                    {
                                        Result result = new Result();
                                        IList<SubjectScores> SubjectScoresList = new List<SubjectScores>();

                                        //the department Name
                                        result.DepartmentName = dept.DepartmentName;

                                        //get the all School Subjects
                                        var schoolSubjects = (from s in _context.SchoolSubjects where s.DepartmentId == dept.Id && s.ClassId == obj.ClassId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId select s).ToList();

                                        if (schoolSubjects.Count() > 0)
                                        {
                                            foreach (SchoolSubjects subj in schoolSubjects)
                                            {
                                                SubjectScores subjectScore = new SubjectScores();
                                                IList<ContinuousAssessments> CA_List = new List<ContinuousAssessments>();

                                                //the subject Name
                                                subjectScore.SubjectName = subj.SubjectName;

                                                //get the score Category Configured by the school to fetch the Continuous Assessments (e.g. CA1, CA2 etc)
                                                var scoreSubCat = (from s in _context.ScoreSubCategoryConfig
                                                                   where s.ClassId == obj.ClassId && s.SchoolId == obj.SchoolId && s.CategoryId == (int)EnumUtility.ScoreCategory.CA
                                                                     && s.CampusId == obj.CampusId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                                                   select s).OrderBy(x => x.Id).ToList();

                                                if (scoreSubCat.Count() > 0)
                                                {
                                                    foreach (ScoreSubCategoryConfig scrCat in scoreSubCat)
                                                    {
                                                        ContinuousAssessments conAss = new ContinuousAssessments();

                                                        //get the scores Obtained by each of the ScoreCategory configured by the school from the continuous Assessment table
                                                        ContinousAssessmentScores CAScores = _context.ContinousAssessmentScores.Where(x => x.DepartmentId == dept.Id && x.SubjectId == subj.Id
                                                        && x.StudentId == studentId && x.SubCategoryId == scrCat.Id && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId
                                                        && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId).FirstOrDefault();

                                                        //check if the CA Scores is not empty/null
                                                        if (CAScores != null)
                                                        {
                                                            conAss.CategoryId = scrCat.Id;
                                                            conAss.CategoryName = scrCat.SubCategoryName;
                                                            conAss.ScoreObtained = CAScores.MarkObtained;

                                                            //add the CA Scores gotten to the CA_List
                                                            CA_List.Add(conAss);
                                                        }
                                                    }
                                                }

                                                //the list of continuous Assessments
                                                subjectScore.ContinuousAssessments = CA_List;

                                                //the reportcarddata table to fetch the results of the student for each subjects
                                                ReportCardData rptData = _context.ReportCardData.Where(x => x.StudentId == studentId && x.SubjectId == subj.Id && x.DepartmentId == dept.Id
                                                && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId
                                                && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                                                if (rptData != null)
                                                {
                                                    subjectScore.CA_Cumulative = rptData.CumulativeCA_Score;
                                                    subjectScore.CA_Total = rptData.CAScore;
                                                    subjectScore.Exam = rptData.ExamScore;
                                                    subjectScore.FirstTermScore = rptData.FirstTermTotalScore;
                                                    subjectScore.SecondTermScore = rptData.SecondTermTotalScore;
                                                    subjectScore.TotalScore = rptData.TotalScore;
                                                    subjectScore.Grade = rptData.Grade;
                                                    subjectScore.Remark = rptData.Remark;
                                                }

                                                //adds the record to the subjectsscore list object
                                                SubjectScoresList.Add(subjectScore);
                                            }

                                        }

                                        result.SubjectScores = SubjectScoresList;
                                        ResultList.Add(result);
                                        ReportCardResult.Result = ResultList;
                                    }
                                }
                            }
                            else //if the school doesnt uses department (Schools that doesnt group therir Subjects per departments)
                            {
                                Result result = new Result();
                                IList<SubjectScores> SubjectScoresList = new List<SubjectScores>();

                                //the department Name is empty and removed from the response
                                result.DepartmentName = "";

                                //get the all School Subjects
                                var schoolSubjects = (from s in _context.SchoolSubjects where s.ClassId == obj.ClassId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId select s).ToList();

                                if (schoolSubjects.Count() > 0)
                                {
                                    foreach (SchoolSubjects subj in schoolSubjects)
                                    {
                                        SubjectScores subjectScore = new SubjectScores();
                                        IList<ContinuousAssessments> CA_List = new List<ContinuousAssessments>();

                                        //the subject Name
                                        subjectScore.SubjectName = subj.SubjectName;

                                        //get the score Category Configured by the school to fetch the Continuous Assessments (e.g. CA1, CA2 etc)
                                        var scoreSubCat = (from s in _context.ScoreSubCategoryConfig
                                                           where s.ClassId == obj.ClassId && s.SchoolId == obj.SchoolId && s.CategoryId == (int)EnumUtility.ScoreCategory.CA
                                                             && s.CampusId == obj.CampusId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                                           select s).OrderBy(x => x.Id).ToList();

                                        if (scoreSubCat.Count() > 0)
                                        {
                                            foreach (ScoreSubCategoryConfig scrCat in scoreSubCat)
                                            {
                                                ContinuousAssessments conAss = new ContinuousAssessments();

                                                //get the scores Obtained by each of the ScoreCategory configured by the school from the continuous Assessment table
                                                ContinousAssessmentScores CAScores = _context.ContinousAssessmentScores.Where(x => x.SubjectId == subj.Id
                                                && x.StudentId == studentId && x.SubCategoryId == scrCat.Id && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId
                                                && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId).FirstOrDefault();

                                                //check if the CA Scores is not empty/null
                                                if (CAScores != null)
                                                {
                                                    conAss.CategoryId = scrCat.Id;
                                                    conAss.CategoryName = scrCat.SubCategoryName;
                                                    conAss.ScoreObtained = CAScores.MarkObtained;

                                                    //add the CA Scores gotten to the CA_List object
                                                    CA_List.Add(conAss);
                                                }
                                            }
                                        }

                                        //the list of continuous Assessments
                                        subjectScore.ContinuousAssessments = CA_List;

                                        //the reportcarddata table to fetch the results of the student for each subjects
                                        ReportCardData rptData = _context.ReportCardData.Where(x => x.StudentId == studentId && x.SubjectId == subj.Id
                                        && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId
                                        && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                                        if (rptData != null)
                                        {
                                            subjectScore.CA_Cumulative = rptData.CumulativeCA_Score;
                                            subjectScore.CA_Total = rptData.CAScore;
                                            subjectScore.Exam = rptData.ExamScore;
                                            subjectScore.FirstTermScore = rptData.FirstTermTotalScore;
                                            subjectScore.SecondTermScore = rptData.SecondTermTotalScore;
                                            subjectScore.TotalScore = rptData.TotalScore;
                                            subjectScore.Grade = rptData.Grade;
                                            subjectScore.Remark = rptData.Remark;
                                        }

                                        //adds the record to the subjectsscore list object
                                        SubjectScoresList.Add(subjectScore);
                                    }

                                }

                                result.SubjectScores = SubjectScoresList;
                                ResultList.Add(result);
                                ReportCardResult.Result = ResultList;
                            }


                            //------------------------------------------CUMULATIVE REPORT CRAD DATA (TotalScore Obtained and Obtainable, Percentage Score and Numbers of Subjects Offered)-------------------------------------------------------

                            ReportCardPosition rptPos = reportCardReUsables.getStudentCumulativeReportCardData(studentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, obj.TermId, obj.SessionId);

                            if (rptPos != null)
                            {
                                CumulativeReportCardData.TotalScoreObtainable = rptPos.TotalScoreObtainable;
                                CumulativeReportCardData.TotalScoreObtained = rptPos.TotalScore;
                                CumulativeReportCardData.PercentageScore = rptPos.PercentageScore;
                                CumulativeReportCardData.NoOfSubjectsOffered = rptPos.SubjectComputed;
                                CumulativeReportCardData.PositionInClass = rptPos.Position + reportCardReUsables.ToOrdinal((int)rptPos.Position); //concatenate the integer position with the ordinal result(e.g 1st, 2nd etc)
                            }

                            //------------------------------------------EXTRACURRICULAR AND BEHAVIOURAL DATA-------------------------------------------------------

                            ExtraCurricularData = reportCardReUsables.getStudentExtracurricularScores(studentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, (int)EnumUtility.ScoreCategory.ExtraCurricular, obj.TermId, obj.SessionId);
                            BehaviouralData = reportCardReUsables.getStudentBehaviouralScores(studentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, (int)EnumUtility.ScoreCategory.Behavioural, obj.TermId, obj.SessionId);


                            //------------------------------------------LEGEND AND LEGEND LISTS-------------------------------------------------------------------

                            LegendData = reportCardReUsables.getReportCardLegend(obj.SchoolId, obj.CampusId, obj.TermId);

                            //------------------------------------------CLASS TEACHER AND PRINCIPAL COMMENTS AND REMARK-------------------------------------------------

                            //-------CLASS TEACHER'S COMMENT AND REMARK
                            ReportCardComments rptClassTeacherComments = reportCardReUsables.getClassTeacherAndPrincipalCommentsAndRemark(studentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, (int)EnumUtility.CommentConfig.ClassTeacher, obj.TermId, obj.SessionId);

                            if (rptClassTeacherComments != null)
                            {
                                RemarksAndCommentData.ClassTeachersComment = rptClassTeacherComments.Comment;
                                RemarksAndCommentData.ClassTeachersRemark = rptClassTeacherComments.Remark;
                            }
                            //-------PRINCIPAL'S COMMENT AND REMARK
                            ReportCardComments rptPrincipalComments = reportCardReUsables.getClassTeacherAndPrincipalCommentsAndRemark(studentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, (int)EnumUtility.CommentConfig.Principal, obj.TermId, obj.SessionId);

                            if (rptPrincipalComments != null)
                            {
                                RemarksAndCommentData.PrincipalsComment = rptPrincipalComments.Comment;
                                RemarksAndCommentData.PrincipalsRemark = rptPrincipalComments.Remark;
                            }


                            //------------------------------------------OTHER DATA (e.g Signature, Next Term Begins etc)-------------------------------------------------

                            OtherData.ClassTeachersFullName = reportCardReUsables.getClassTeacherFullName(obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId);
                            OtherData.NextTermBeginsDate = reportCardReUsables.getNextTermBeginsDate(obj.SchoolId, obj.CampusId);
                            OtherData.NoInClass = reportCardReUsables.getNumberOfStudentInClass(obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId);
                            OtherData.Session = reportCardReUsables.getCurrentSessionName(obj.SchoolId);
                            OtherData.Term = reportCardReUsables.getTermName(obj.TermId);
                            OtherData.ClassTeacherSignature = reportCardReUsables.getReportCardSignature(obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, obj.TermId, obj.SessionId);
                            OtherData.PrincipalSignature = reportCardReUsables.getPrincipalReportCardSignature(obj.SchoolId, obj.CampusId);
                            OtherData.DateGenerated = DateTime.Now.ToString();


                            //------------------------------------------------RESPONSE DATA----------------------------------------------------------------------------

                            studentResult.ReportCardTemplateInfo = ReportCardTemplateInfo;//the Template details
                            studentResult.SchoolInfo = SchoolInfo;//the School Information
                            studentResult.StudentInfo = StudentInfo;//the student Information
                            studentResult.ReportCardHeaderInfo = rptCardHeaderInfo; // The header Information
                            studentResult.ReportCardResult = ReportCardResult;//the student result 
                            studentResult.CumulativeReportCardData = CumulativeReportCardData;//the Cumulative student result (Student Performance)
                            studentResult.ExtraCurricularData = ExtraCurricularData;
                            studentResult.BehaviouralData = BehaviouralData;
                            studentResult.LegendData = LegendData;
                            studentResult.RemarksAndCommentData = RemarksAndCommentData;
                            studentResult.OtherData = OtherData;

                            //add student result to response list
                            responseList.Add(studentResult);
                        }

                        response.StatusCode = 200;
                        response.StatusMessage = "Successful";
                        response.Data = responseList;
                    }
                    else
                    {
                        return new ReportCardDataRespModel { StatusCode = 409, StatusMessage = "Report Card Configurations has not been done" };
                    }
                }
                else
                {
                    return new ReportCardDataRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
                }

                return response;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new ReportCardDataRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        //------------------------------------------PRINTING AND GENERATING REPORT CARD INFORMATION WITH PIN--------------------------------------

        public async Task<ReportCardDataRespModel> getReportCardDataByStudentIdAndPinAsync(ReportCardDataWithPinReqModel obj)
        {
            try
            {
                ReportCardDataRespModel response = new ReportCardDataRespModel();
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);
                var checkTerm = check.checkTermById(obj.TermId);
                var checkSession = check.checkSessionById(obj.SessionId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true && checkTerm == true && checkSession == true)
                {
                    //check pin
                    ReportCardPin rptPin = _context.ReportCardPin.Where(rp => rp.SchoolId == obj.SchoolId && rp.CampusId == obj.CampusId && rp.TermId == obj.TermId
                    && rp.Pin == obj.Pin.Trim().ToUpper()).FirstOrDefault();

                    if (rptPin != null)
                    {
                        //check if the pin can still be used
                        if (rptPin.NoOfTimesUsed < rptPin.NoOfTimesValid)
                        {
                            //Check the class and classgrade the pin has been used for
                            if ((rptPin.ViewedClassId == 0 && rptPin.ViewedClassGradeId == 0) || (rptPin.ViewedClassId == obj.ClassId && rptPin.ViewedClassGradeId == obj.ClassGradeId))
                            {
                                //get the report Card Configuration as configured by each school
                                ReportCardConfiguration rptConfig = _context.ReportCardConfiguration.Where(rp => rp.SchoolId == obj.SchoolId && rp.CampusId == obj.CampusId && rp.TermId == obj.TermId).FirstOrDefault();   //selects all the courses in the cart

                                if (rptConfig != null)
                                {
                                    StudentResult studentResult = new StudentResult();
                                    IList<Result> ResultList = new List<Result>();

                                    //instance of objects to be used to return data
                                    ReportCardTemplateInfo ReportCardTemplateInfo = new ReportCardTemplateInfo();
                                    SchoolInfo SchoolInfo = new SchoolInfo();
                                    StudentInfo StudentInfo = new StudentInfo();
                                    ReportCardResult ReportCardResult = new ReportCardResult();
                                    IList<object> ExtraCurricularData = new List<object>();
                                    IList<object> BehaviouralData = new List<object>();
                                    CumulativeReportCardData CumulativeReportCardData = new CumulativeReportCardData();
                                    object LegendData = new object();
                                    RemarksAndCommentData RemarksAndCommentData = new RemarksAndCommentData();
                                    OtherData OtherData = new OtherData();
                                    //------------------------------------------REPORT CARD HEADER INFORMATION-------------------------------------------------------

                                    ReportCardHeaderInfo rptCardHeaderInfo = new ReportCardHeaderInfo();

                                    if (rptConfig.ShowDepartment == true)
                                    {
                                        rptCardHeaderInfo.Department = "DEPARTMENT";
                                    }
                                    if (rptConfig.RefFirstTermScoreShow == true && rptConfig.RefSecondTermScoreShow == true)
                                    {
                                        rptCardHeaderInfo.FirstTermScore = "FIRST TERM SCORE";
                                        rptCardHeaderInfo.SecondTermScore = "SECOND TERM SCORE";
                                    }
                                    if (rptConfig.ShowCA_Cumulative == true)
                                    {
                                        rptCardHeaderInfo.CA_Cumulative = "CA CUMULATIVE";
                                    }

                                    //get the score Category Configured by the school to fetch the Continuous Assessments (e.g. CA1, CA2 etc)
                                    var CA_SubCategory = (from x in _context.ScoreSubCategoryConfig
                                                          where x.CategoryId == (int)EnumUtility.ScoreCategory.CA
                                                           && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId
                                                           && x.TermId == obj.TermId && x.SessionId == obj.SessionId
                                                          select new
                                                          {
                                                              x.Id,
                                                              x.SubCategoryName
                                                          }).OrderBy(s => s.Id).ToList();

                                    //add the CA Subcategory to the report card header information
                                    rptCardHeaderInfo.CA_SubCategory = CA_SubCategory;

                                    //------------------------------------------REPORT CARD TEMPLATE-------------------------------------------------------

                                    ReportCardTemplates rptTemplate = _context.ReportCardTemplates.Where(rp => rp.ClassId == obj.ClassId && rp.SchoolId == obj.SchoolId && rp.CampusId == obj.CampusId).FirstOrDefault();
                                    if (rptTemplate != null)
                                    {
                                        ReportCardTemplateInfo.ClassId = rptTemplate.ClassId;
                                        ReportCardTemplateInfo.SchoolSubTypeId = rptTemplate.SchoolSubTypeId;
                                        ReportCardTemplateInfo.CampusId = rptTemplate.CampusId;
                                        ReportCardTemplateInfo.SchoolId = rptTemplate.SchoolId;
                                        ReportCardTemplateInfo.Description = rptTemplate.Description;
                                    }

                                    //------------------------------------------SCHOOL INFORMATION-------------------------------------------------------

                                    Schools rptSchInfo = _context.Schools.Where(rp => rp.Id == obj.SchoolId).FirstOrDefault();
                                    if (rptSchInfo != null)
                                    {
                                        SchoolInfo.SchoolName = rptSchInfo.SchoolName;
                                        SchoolInfo.SchoolType = reportCardReUsables.getSchoolTypeName(obj.SchoolId);
                                        SchoolInfo.LogoUrl = rptSchInfo.SchoolLogoUrl;
                                        SchoolInfo.PhoneNumber = "";
                                        SchoolInfo.Address = "";
                                        SchoolInfo.EmailAddress = "";
                                        SchoolInfo.Motto = "";
                                    }

                                    //------------------------------------------STUDENT INFORMATION-------------------------------------------------------

                                    Students rptStudentInfo = _context.Students.Where(rp => rp.Id == obj.StudentId && rp.SchoolId == obj.SchoolId && rp.CampusId == obj.CampusId).FirstOrDefault();

                                    StudentInfo.StudentId = rptStudentInfo.Id;
                                    StudentInfo.FirstName = rptStudentInfo.FirstName;
                                    StudentInfo.LastName = rptStudentInfo.LastName;
                                    StudentInfo.MiddleName = rptStudentInfo.LastName;
                                    if (rptStudentInfo.GenderId > 0)
                                    {
                                        StudentInfo.Gender = reportCardReUsables.getGender((int)rptStudentInfo.GenderId);
                                    }
                                    StudentInfo.FullName = rptStudentInfo.FirstName + " " + rptStudentInfo.LastName + " " + rptStudentInfo.MiddleName;
                                    StudentInfo.DateOfBirth = rptStudentInfo.DateOfBirth;
                                    StudentInfo.StudentPassport = rptStudentInfo.ProfilePictureUrl;
                                    if (rptStudentInfo.DateOfBirth != null)
                                    {
                                        StudentInfo.Age = reportCardReUsables.getAge(Convert.ToDateTime(rptStudentInfo.DateOfBirth));
                                    }
                                    StudentInfo.Class = reportCardReUsables.getStudentClass(obj.StudentId, obj.SchoolId);
                                    StudentInfo.ClassGrade = reportCardReUsables.getStudentClassGrade(obj.StudentId, obj.SchoolId);


                                    //check if the school configured the report to show and use department
                                    if (rptConfig.ShowDepartment == true)
                                    {
                                        //get the department created by the schools (for schools using subjects by department)
                                        var department = (from s in _context.SubjectDepartment where s.ClassId == obj.ClassId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId select s).ToList();

                                        if (department.Count() > 0)
                                        {
                                            foreach (SubjectDepartment dept in department)
                                            {
                                                Result result = new Result();
                                                IList<SubjectScores> SubjectScoresList = new List<SubjectScores>();

                                                //the department Name
                                                result.DepartmentName = dept.DepartmentName;

                                                //get the all School Subjects
                                                var schoolSubjects = (from s in _context.SchoolSubjects where s.DepartmentId == dept.Id && s.ClassId == obj.ClassId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId select s).ToList();

                                                if (schoolSubjects.Count() > 0)
                                                {
                                                    foreach (SchoolSubjects subj in schoolSubjects)
                                                    {
                                                        SubjectScores subjectScore = new SubjectScores();
                                                        IList<ContinuousAssessments> CA_List = new List<ContinuousAssessments>();

                                                        //the subject Name
                                                        subjectScore.SubjectName = subj.SubjectName;

                                                        //get the score Category Configured by the school to fetch the Continuous Assessments (e.g. CA1, CA2 etc)
                                                        var scoreSubCat = (from s in _context.ScoreSubCategoryConfig
                                                                           where s.ClassId == obj.ClassId && s.SchoolId == obj.SchoolId && s.CategoryId == (int)EnumUtility.ScoreCategory.CA
                                                                             && s.CampusId == obj.CampusId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                                                           select s).OrderBy(x => x.Id).ToList();

                                                        if (scoreSubCat.Count() > 0)
                                                        {
                                                            foreach (ScoreSubCategoryConfig scrCat in scoreSubCat)
                                                            {
                                                                ContinuousAssessments conAss = new ContinuousAssessments();

                                                                //get the scores Obtained by each of the ScoreCategory configured by the school from the continuous Assessment table
                                                                ContinousAssessmentScores CAScores = _context.ContinousAssessmentScores.Where(x => x.DepartmentId == dept.Id && x.SubjectId == subj.Id
                                                                && x.StudentId == obj.StudentId && x.SubCategoryId == scrCat.Id && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId
                                                                && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId).FirstOrDefault();

                                                                //check if the CA Scores is not empty/null
                                                                if (CAScores != null)
                                                                {
                                                                    conAss.CategoryId = scrCat.Id;
                                                                    conAss.CategoryName = scrCat.SubCategoryName;
                                                                    conAss.ScoreObtained = CAScores.MarkObtained;

                                                                    //add the CA Scores gotten to the CA_List
                                                                    CA_List.Add(conAss);
                                                                }
                                                            }
                                                        }

                                                        //the list of continuous Assessments
                                                        subjectScore.ContinuousAssessments = CA_List;

                                                        //the reportcarddata table to fetch the results of the student for each subjects
                                                        ReportCardData rptData = _context.ReportCardData.Where(x => x.StudentId == obj.StudentId && x.SubjectId == subj.Id && x.DepartmentId == dept.Id
                                                        && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId
                                                        && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                                                        if (rptData != null)
                                                        {
                                                            subjectScore.CA_Cumulative = rptData.CumulativeCA_Score;
                                                            subjectScore.CA_Total = rptData.CAScore;
                                                            subjectScore.Exam = rptData.ExamScore;
                                                            subjectScore.FirstTermScore = rptData.FirstTermTotalScore;
                                                            subjectScore.SecondTermScore = rptData.SecondTermTotalScore;
                                                            subjectScore.TotalScore = rptData.TotalScore;
                                                            subjectScore.Grade = rptData.Grade;
                                                            subjectScore.Remark = rptData.Remark;
                                                        }

                                                        //adds the record to the subjectsscore list object
                                                        SubjectScoresList.Add(subjectScore);
                                                    }

                                                }

                                                result.SubjectScores = SubjectScoresList;
                                                ResultList.Add(result);
                                                ReportCardResult.Result = ResultList;
                                            }
                                        }
                                    }
                                    else //if the school doesnt uses department (Schools that doesnt group therir Subjects per departments)
                                    {
                                        Result result = new Result();
                                        IList<SubjectScores> SubjectScoresList = new List<SubjectScores>();

                                        //the department Name is empty and removed from the response
                                        result.DepartmentName = "";

                                        //get the all School Subjects
                                        var schoolSubjects = (from s in _context.SchoolSubjects where s.ClassId == obj.ClassId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId select s).ToList();

                                        if (schoolSubjects.Count() > 0)
                                        {
                                            foreach (SchoolSubjects subj in schoolSubjects)
                                            {
                                                SubjectScores subjectScore = new SubjectScores();
                                                IList<ContinuousAssessments> CA_List = new List<ContinuousAssessments>();

                                                //the subject Name
                                                subjectScore.SubjectName = subj.SubjectName;

                                                //get the score Category Configured by the school to fetch the Continuous Assessments (e.g. CA1, CA2 etc)
                                                var scoreSubCat = (from s in _context.ScoreSubCategoryConfig
                                                                   where s.ClassId == obj.ClassId && s.SchoolId == obj.SchoolId && s.CategoryId == (int)EnumUtility.ScoreCategory.CA
                                                                     && s.CampusId == obj.CampusId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                                                   select s).OrderBy(x => x.Id).ToList();

                                                if (scoreSubCat.Count() > 0)
                                                {
                                                    foreach (ScoreSubCategoryConfig scrCat in scoreSubCat)
                                                    {
                                                        ContinuousAssessments conAss = new ContinuousAssessments();

                                                        //get the scores Obtained by each of the ScoreCategory configured by the school from the continuous Assessment table
                                                        ContinousAssessmentScores CAScores = _context.ContinousAssessmentScores.Where(x => x.SubjectId == subj.Id
                                                        && x.StudentId == obj.StudentId && x.SubCategoryId == scrCat.Id && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId
                                                        && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId).FirstOrDefault();

                                                        //check if the CA Scores is not empty/null
                                                        if (CAScores != null)
                                                        {
                                                            conAss.CategoryId = scrCat.Id;
                                                            conAss.CategoryName = scrCat.SubCategoryName;
                                                            conAss.ScoreObtained = CAScores.MarkObtained;

                                                            //add the CA Scores gotten to the CA_List object
                                                            CA_List.Add(conAss);
                                                        }
                                                    }
                                                }

                                                //the list of continuous Assessments
                                                subjectScore.ContinuousAssessments = CA_List;

                                                //the reportcarddata table to fetch the results of the student for each subjects
                                                ReportCardData rptData = _context.ReportCardData.Where(x => x.StudentId == obj.StudentId && x.SubjectId == subj.Id
                                                && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId
                                                && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                                                if (rptData != null)
                                                {
                                                    subjectScore.CA_Cumulative = rptData.CumulativeCA_Score;
                                                    subjectScore.CA_Total = rptData.CAScore;
                                                    subjectScore.Exam = rptData.ExamScore;
                                                    subjectScore.FirstTermScore = rptData.FirstTermTotalScore;
                                                    subjectScore.SecondTermScore = rptData.SecondTermTotalScore;
                                                    subjectScore.TotalScore = rptData.TotalScore;
                                                    subjectScore.Grade = rptData.Grade;
                                                    subjectScore.Remark = rptData.Remark;
                                                }

                                                //adds the record to the subjectsscore list object
                                                SubjectScoresList.Add(subjectScore);
                                            }

                                        }

                                        result.SubjectScores = SubjectScoresList;
                                        ResultList.Add(result);
                                        ReportCardResult.Result = ResultList;
                                    }


                                    //------------------------------------------CUMULATIVE REPORT CRAD DATA (TotalScore Obtained and Obtainable, Percentage Score and Numbers of Subjects Offered)-------------------------------------------------------

                                    ReportCardPosition rptPos = reportCardReUsables.getStudentCumulativeReportCardData(obj.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, obj.TermId, obj.SessionId);

                                    if (rptPos != null)
                                    {
                                        CumulativeReportCardData.TotalScoreObtainable = rptPos.TotalScoreObtainable;
                                        CumulativeReportCardData.TotalScoreObtained = rptPos.TotalScore;
                                        CumulativeReportCardData.PercentageScore = rptPos.PercentageScore;
                                        CumulativeReportCardData.NoOfSubjectsOffered = rptPos.SubjectComputed;
                                        CumulativeReportCardData.PositionInClass = rptPos.Position + reportCardReUsables.ToOrdinal((int)rptPos.Position); //concatenate the integer position with the ordinal result(e.g 1st, 2nd etc)
                                    }

                                    //------------------------------------------EXTRACURRICULAR AND BEHAVIOURAL DATA-------------------------------------------------------

                                    ExtraCurricularData = reportCardReUsables.getStudentExtracurricularScores(obj.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, (int)EnumUtility.ScoreCategory.ExtraCurricular, obj.TermId, obj.SessionId);
                                    BehaviouralData = reportCardReUsables.getStudentBehaviouralScores(obj.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, (int)EnumUtility.ScoreCategory.Behavioural, obj.TermId, obj.SessionId);


                                    //------------------------------------------LEGEND AND LEGEND LISTS-------------------------------------------------------------------

                                    LegendData = reportCardReUsables.getReportCardLegend(obj.SchoolId, obj.CampusId, obj.TermId);

                                    //------------------------------------------CLASS TEACHER AND PRINCIPAL COMMENTS AND REMARK-------------------------------------------------

                                    //-------CLASS TEACHER'S COMMENT AND REMARK
                                    ReportCardComments rptClassTeacherComments = reportCardReUsables.getClassTeacherAndPrincipalCommentsAndRemark(obj.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, (int)EnumUtility.CommentConfig.ClassTeacher, obj.TermId, obj.SessionId);

                                    if (rptClassTeacherComments != null)
                                    {
                                        RemarksAndCommentData.ClassTeachersComment = rptClassTeacherComments.Comment;
                                        RemarksAndCommentData.ClassTeachersRemark = rptClassTeacherComments.Remark;
                                    }
                                    //-------PRINCIPAL'S COMMENT AND REMARK
                                    ReportCardComments rptPrincipalComments = reportCardReUsables.getClassTeacherAndPrincipalCommentsAndRemark(obj.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, (int)EnumUtility.CommentConfig.Principal, obj.TermId, obj.SessionId);

                                    if (rptPrincipalComments != null)
                                    {
                                        RemarksAndCommentData.PrincipalsComment = rptPrincipalComments.Comment;
                                        RemarksAndCommentData.PrincipalsRemark = rptPrincipalComments.Remark;
                                    }

                                    //------------------------------------------OTHER DATA (e.g Signature, Next Term Begins etc)-------------------------------------------------

                                    OtherData.ClassTeachersFullName = reportCardReUsables.getClassTeacherFullName(obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId);
                                    OtherData.NextTermBeginsDate = reportCardReUsables.getNextTermBeginsDate(obj.SchoolId, obj.CampusId);
                                    OtherData.NoInClass = reportCardReUsables.getNumberOfStudentInClass(obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId);
                                    OtherData.Session = reportCardReUsables.getCurrentSessionName(obj.SchoolId);
                                    OtherData.Term = reportCardReUsables.getTermName(obj.TermId);
                                    OtherData.ClassTeacherSignature = reportCardReUsables.getReportCardSignature(obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, obj.TermId, obj.SessionId);
                                    OtherData.PrincipalSignature = reportCardReUsables.getPrincipalReportCardSignature(obj.SchoolId, obj.CampusId);
                                    OtherData.DateGenerated = DateTime.Now.ToString();

                                    //------------------------------------------------RESPONSE DATA----------------------------------------------------------------------------

                                    studentResult.ReportCardTemplateInfo = ReportCardTemplateInfo;//the Template details
                                    studentResult.SchoolInfo = SchoolInfo;//the School Information
                                    studentResult.StudentInfo = StudentInfo;//the student Information
                                    studentResult.ReportCardHeaderInfo = rptCardHeaderInfo; // The header Information
                                    studentResult.ReportCardResult = ReportCardResult;//the student result 
                                    studentResult.CumulativeReportCardData = CumulativeReportCardData;//the Cumulative student result (Student Performance)
                                    studentResult.ExtraCurricularData = ExtraCurricularData;
                                    studentResult.BehaviouralData = BehaviouralData;
                                    studentResult.LegendData = LegendData;
                                    studentResult.RemarksAndCommentData = RemarksAndCommentData;
                                    studentResult.OtherData = OtherData;

                                    //add student result to response list
                                    //responseList.Add(studentResult);


                                    //update the reportcard pin object
                                    rptPin.ViewedClassId = obj.ClassId;
                                    rptPin.ViewedClassGradeId = obj.ClassGradeId;
                                    rptPin.IsUsed = true;
                                    rptPin.IsUsedById = obj.StudentId.ToString();
                                    rptPin.NoOfTimesUsed = rptPin.NoOfTimesUsed + 1;
                                    rptPin.DateLastUsed = DateTime.Now.ToString();

                                    await _context.SaveChangesAsync();

                                    //response object
                                    response.StatusCode = 200;
                                    response.StatusMessage = "Successful";
                                    response.Data = studentResult;                                    

                                }
                                else
                                {
                                    return new ReportCardDataRespModel { StatusCode = 409, StatusMessage = "Report Card Configurations has not been done" };
                                }
                            }
                            else
                            {
                                return new ReportCardDataRespModel { StatusCode = 409, StatusMessage = $"You can't use this Pin: {rptPin.Pin}, because it has been used to view another Class and ClassGrade Result!" };
                            }
                        }
                        else
                        {
                            return new ReportCardDataRespModel { StatusCode = 409, StatusMessage = $"You can't use this Pin: {rptPin.Pin}, because it has been used {rptPin.NoOfTimesUsed} times and the number of times is valid for is {rptPin.NoOfTimesValid} times!" };
                        }
                    }
                    else
                    {
                        return new ReportCardDataRespModel { StatusCode = 409, StatusMessage = $"This Pin: {obj.Pin.ToUpper()} does not exist, Kinldly retry Pin or Contact the Administrator!" };
                    }
                }
                else
                {
                    return new ReportCardDataRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
                }

                return response;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new ReportCardDataRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
    }
}