using Microsoft.AspNetCore.Hosting;
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
    public class BroadSheetRepo : IBroadSheetRepo
    {
        private readonly AppDbContext _context;
        private readonly BroadSheetReusables broadSheetReusables;
        private readonly ReportCardReUsables reportCardReUsables;

        public BroadSheetRepo(AppDbContext context, BroadSheetReusables broadSheetReusables, ReportCardReUsables reportCardReUsables)
        {
            _context = context;
            this.broadSheetReusables = broadSheetReusables;
            this.reportCardReUsables = reportCardReUsables;
        }
        public async Task<GenericRespModel> createBroadsheetGradingConfigAsync(BroadsheetGradeReqModel obj)
        {
            try
            {
                var brdSheetGrade = _context.BroadSheetGrading.Where(c => c.LowestRange == obj.LowestRange && c.HighestRange == obj.HighestRange
                && c.Grade == obj.Grade && c.SchoolId == obj.SchoolId && c.CampusId == obj.CampusId && c.SessionId == obj.SessionId).FirstOrDefault();

                if (brdSheetGrade == null)
                {
                    var bdGrd = new BroadSheetGrading
                    {
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        SessionId = obj.SessionId,
                        LowestRange = obj.LowestRange,
                        HighestRange = obj.HighestRange,
                        Grade = obj.Grade,
                        DateCreated = DateTime.Now,
                    };

                    await _context.BroadSheetGrading.AddAsync(bdGrd);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Broadsheet Grade Created Successfully"};
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Broadsheet Grading Already Exist"};
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericRespModel> updateBroadsheetGradingConfigAsync(BroadsheetGradeReqModel obj, long broadSheetConfigId)
        {
            try
            {
                //check if exists
                var grd = _context.BroadSheetGrading.Where(c => c.Id == broadSheetConfigId).FirstOrDefault();

                if (grd != null)
                {
                    //check if already exists
                    var checkResult = _context.BroadSheetGrading.Where(c => c.LowestRange == obj.LowestRange && c.HighestRange == obj.HighestRange
                    && c.Grade == obj.Grade && c.SessionId == obj.SessionId && c.SchoolId == obj.SchoolId && c.CampusId == obj.CampusId).FirstOrDefault();

                    //update if it doesnt exists
                    if (checkResult == null)
                    {
                        grd.SchoolId = obj.SchoolId;
                        grd.CampusId = obj.CampusId;
                        grd.SessionId = obj.SessionId;
                        grd.LowestRange = obj.LowestRange;
                        grd.HighestRange = obj.HighestRange;
                        grd.Grade = obj.Grade;

                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "BroadSheet Grade Updated Successfully", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "BroadSheet Grade Already Exists", };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No BroadSheet Grade with the specified Id", };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericRespModel> getBroadsheetGradingConfigByIdAsync(long broadSheetConfigId)
        {
            try
            {
                //returns all the Score Grading
                var result = from cl in _context.BroadSheetGrading
                             where cl.Id == broadSheetConfigId
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.SessionId,
                                 cl.Sessions.SessionName,
                                 cl.LowestRange,
                                 cl.HighestRange,
                                 cl.Grade,
                                 cl.DateCreated
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault()};
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericRespModel> getBroadsheetGradingConfigAsync()
        {
            try
            {
                //returns all the Score Grading
                var result = from cl in _context.BroadSheetGrading
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.SessionId,
                                 cl.Sessions.SessionName,
                                 cl.LowestRange,
                                 cl.HighestRange,
                                 cl.Grade,
                                 cl.DateCreated
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available"};
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericRespModel> deleteBroadsheetGradingConfigAsync(long broadSheetConfigId)
        {
            try
            {
                //check if the score Grade exists
                var grd = _context.BroadSheetGrading.Where(c => c.Id == broadSheetConfigId).FirstOrDefault();

                if (grd != null)
                {
                    _context.BroadSheetGrading.Remove(grd);
                    await _context.SaveChangesAsync();
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "BroadSheet Grade Deleted Successfully", };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No BroadSheet Grade with the specified Id", };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }


        public async Task<GenericRespModel> createBroadsheetRemarkConfigAsync(BroadSheetRemarkReqModel obj)
        {
            try
            {
                //no of subjects in the class
                var classSubjects = _context.SchoolSubjects.Where(c => c.SchoolId == obj.SchoolId && c.CampusId == obj.CampusId
                    && c.ClassId == obj.ClassId).ToList().Count();

                //no of subjects in the req body
                var subjectsReqCount = obj.SubjectLists.Count();

                //check if the no of subjects in the class is equal to the no of subjects in the request body
                if (classSubjects == subjectsReqCount)
                {
                    foreach (var mand in obj.SubjectLists)
                    {
                        var getBroadSheetRemark = _context.BroadSheetRemark.Where(c => c.SchoolId == obj.SchoolId && c.CampusId == obj.CampusId
                        && c.ClassId == obj.ClassId && c.Mandatory == mand.Mandatory && c.SubjectId == mand.SubjectId).FirstOrDefault();

                        if (getBroadSheetRemark == null)
                        {
                            var bdGrd = new BroadSheetRemark
                            {
                                SchoolId = obj.SchoolId,
                                CampusId = obj.CampusId,
                                ClassId = obj.ClassId,
                                SubjectId = mand.SubjectId,
                                Mandatory = mand.Mandatory,
                                DateCreated = DateTime.Now,
                            };

                            await _context.BroadSheetRemark.AddAsync(bdGrd);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            getBroadSheetRemark.SchoolId = obj.SchoolId;
                            getBroadSheetRemark.CampusId = obj.CampusId;
                            getBroadSheetRemark.ClassId = obj.ClassId;
                            getBroadSheetRemark.SubjectId = mand.SubjectId;
                            getBroadSheetRemark.Mandatory = mand.Mandatory;

                            await _context.SaveChangesAsync();
                        }
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Broadsheet Remark Created/Updated Successfully" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No of Subjects in Class is more/less than selected Sujects in the config" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericRespModel> getBroadsheetRemarkConfigAsync()
        {
            try
            {
                //returns all the Score Grading
                var result = from cl in _context.BroadSheetRemark
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.SubjectId,
                                 cl.Classes.ClassName,
                                 cl.ClassId,
                                 cl.Mandatory,
                                 cl.DateCreated
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<BroadSheetRespModel> generateClassBroadsheetAsync(long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId)
        {
            try
            {
                BroadSheetRespModel response = new BroadSheetRespModel();
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                //the student subject score lists
                IList<StudentSubjectScore> studentSubjectScoreList = new List<StudentSubjectScore>();               
                int countMandatorySubjectsPassed = 0; 
                int countOtherSubjectsPassed = 0;
                int countPassedStudents = 0;
                int countFailedStudents = 0;

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true && checkTerm == true && checkSession == true)
                {
                    //the total numbers of subjects that are mandatory for the broadsheet
                    var totalMandatoryConfig = (from c in _context.BroadSheetRemark where c.SchoolId == schoolId && c.CampusId == campusId && c.Mandatory == true select c).ToList().Count();

                    var studentsInClass = (from cl in _context.GradeStudents where cl.SchoolId == schoolId && cl.CampusId == campusId 
                                           && cl.ClassId == classId && cl.ClassGradeId == classGradeId  select cl).ToList();

                    //school and class info
                    SchoolAndClassInfo schoolAndClassInfo = new SchoolAndClassInfo();
                    Schools schInfo = _context.Schools.Where(rp => rp.Id == schoolId).FirstOrDefault();
                    SchoolCampus campusInfo = _context.SchoolCampus.Where(rp => rp.Id == campusId).FirstOrDefault();

                    schoolAndClassInfo.SchoolName = schInfo.SchoolName;
                    schoolAndClassInfo.CampusName = campusInfo.CampusName;
                    schoolAndClassInfo.CampusAddress = campusInfo.CampusAddress;
                    schoolAndClassInfo.Class = broadSheetReusables.getClass(classId);
                    schoolAndClassInfo.ClassGrade = broadSheetReusables.getClassGrade(classGradeId);

                    schoolAndClassInfo.ClassTeacherName = reportCardReUsables.getClassTeacherFullName(schoolId, campusId, classId, classGradeId);
                    schoolAndClassInfo.Session = reportCardReUsables.getCurrentSessionName(schoolId);
                    schoolAndClassInfo.Term = reportCardReUsables.getTermName(termId);
                    schoolAndClassInfo.DateGenerated = DateTime.Now.ToString();

                    //all subjects created for a class
                    var subjectsInClass = (from cl in _context.SchoolSubjects where cl.SchoolId == schoolId && cl.CampusId == campusId && cl.ClassId == classId select cl).OrderBy(a=>a.SubjectName).ToList();

                    //broadsheet summary code goes here!
                    BroadSheetSummaryInfo BroadSheetSummaryInfo = new BroadSheetSummaryInfo();

                    //broadsheet header code goes here!
                    BroadSheetHeaderInfo BroadSheetHeaderInfo = new BroadSheetHeaderInfo();
                    BroadSheetSubjectsInfo BroadSheetSubjectsInfo = new BroadSheetSubjectsInfo();
                    BroadSheetSubjectsInfo.Subjects = subjectsInClass.Select(c=>c.SubjectName).ToList();

                    BroadSheetHeaderInfo.BroadSheetSubjectsInfo = BroadSheetSubjectsInfo;
                    BroadSheetHeaderInfo.BroadSheetCandidatesInfo = new BroadSheetCandidatesInfo();
                    BroadSheetHeaderInfo.BroadSheetScoresInfo = new BroadSheetScoresInfo();

                    if (studentsInClass.Count() > 0)
                    {
                        foreach (var std in studentsInClass)
                        {                            
                            CumulativeScore cumulativeScore = new CumulativeScore();
                            StudentSubjectScore studentSubjectScore = new StudentSubjectScore();
                            IList<SubjectScore> subjectScoreList = new List<SubjectScore>();

                            //the student details
                            Students brdStudentInfo = _context.Students.Where(rp => rp.Id == std.StudentId && rp.SchoolId == schoolId && rp.CampusId == campusId).FirstOrDefault();

                            //student data code goes here
                            BroadSheetStudentInfo studentInfo = new BroadSheetStudentInfo();
                            if (brdStudentInfo != null)
                            {
                                studentInfo.StudentId = brdStudentInfo.Id;
                                studentInfo.LastName = brdStudentInfo.LastName;
                                studentInfo.FirstName = brdStudentInfo.FirstName;
                                studentInfo.MiddleName = brdStudentInfo.MiddleName;
                                if (brdStudentInfo.GenderId > 0)
                                {
                                    studentInfo.Gender = reportCardReUsables.getGender((int)brdStudentInfo.GenderId);
                                }
                            }
                           
                            //
                            if (subjectsInClass.Count() > 0)
                            {
                                foreach (var subj in subjectsInClass)
                                {
                                    //the subject score and subjectName that are to be added to a list
                                    SubjectScore subjectScore = new SubjectScore();

                                    //the reportcard data after it has been computed for getting the total score of each subjects for a student
                                    var rptData = _context.ReportCardData.Where(c => c.SchoolId == schoolId && c.CampusId == campusId && c.SubjectId == subj.Id && c.ClassId == classId
                                    && c.ClassGradeId == classGradeId && c.SessionId == sessionId && c.TermId == termId && c.StudentId == std.StudentId).FirstOrDefault();

                                    if (rptData != null)
                                    {
                                        //assign the subject total score and the subject name
                                        subjectScore.Score = rptData.TotalScore;
                                        subjectScore.SubjectName = broadSheetReusables.subjectName(rptData.SubjectId);
                                    }
                                   
                                    //add the subject score obj to a list
                                    subjectScoreList.Add(subjectScore);

                                    //the broadsheet remark configuration by the school for each subjects
                                    var brdRmkConfig = _context.BroadSheetRemark.Where(c => c.SchoolId == schoolId && c.CampusId == campusId && c.SubjectId == subj.Id).FirstOrDefault();
                                    if (brdRmkConfig != null)
                                    {
                                        //check if the subject is configured as mandatory and apply the logic
                                        if (brdRmkConfig.Mandatory == true)
                                        {
                                            if (rptData.TotalScore >= 50) //check if the total score for the madatory configured is greater than 50%
                                            {
                                                countMandatorySubjectsPassed++;
                                            }
                                        }
                                        else
                                        {
                                            if (rptData.TotalScore >= 50) //check if the total score for others that are not configured as mandatory is greater than 50%
                                            {
                                                countOtherSubjectsPassed++;
                                            }
                                        }
                                    }
                                }
                            }

                            //student total No of mandatory passed
                            var mandatoryRemrk = broadSheetReusables.mandatoryPassedRemark(countMandatorySubjectsPassed, totalMandatoryConfig);
                            //student total No of others passed
                            var othersRemrk = broadSheetReusables.othersPassedRemark(countOtherSubjectsPassed);
                            //Student final Remark
                            var studentFinalRemark = broadSheetReusables.studentFinalRemark(mandatoryRemrk, othersRemrk);

                            //count No of students that passed or failed
                            if (studentFinalRemark == "Passed")
                                countPassedStudents++;
                            else
                                countFailedStudents++;

                            //the ReportCardPosition (after it has been computed) to get the total score of all the subjects offered by a student
                            var rptPos = _context.ReportCardPosition.Where(c => c.SchoolId == schoolId && c.CampusId == campusId && c.ClassId == classId
                                        && c.ClassGradeId == classGradeId && c.SessionId == sessionId && c.TermId == termId && c.StudentId == std.StudentId).FirstOrDefault();

                            //total score obtained by student
                            decimal totalScore = 0;
                            if (rptPos != null)
                            {
                                totalScore = rptPos.TotalScore;
                            }
                           
                            //total numbers of subjects offered by student
                            int totalSubjectsOffered = subjectScoreList.Count();

                            //percentage score (total score obtained by the student divided by the total numbers of subjects offered by the student)
                            decimal percentageScore = totalScore / totalSubjectsOffered;
                            var grade = broadSheetReusables.getBroadSheetGrade(percentageScore, schoolId, campusId, sessionId);

                            //Cumulative data goes here
                            cumulativeScore.Total = totalScore;
                            cumulativeScore.Percentage = percentageScore;
                            cumulativeScore.Grade = grade; //get the grade for each student
                            cumulativeScore.Remark = studentFinalRemark;

                            //broadsheet summary data goes here
                            BroadSheetSummaryInfo.NoPasseed_Promoted = countPassedStudents.ToString();
                            BroadSheetSummaryInfo.NoFailed_Repeat = countFailedStudents.ToString();
                            BroadSheetSummaryInfo.NoInClass = reportCardReUsables.getNumberOfStudentInClass(schoolId, campusId, classId, classGradeId).ToString();
                            BroadSheetSummaryInfo.NoPresent = "";
                            BroadSheetSummaryInfo.NoAbsent = "";

                            //student subject data goes here
                            studentSubjectScore.StudentInfo = studentInfo;
                            studentSubjectScore.SubjectScore = subjectScoreList;
                            studentSubjectScore.CumulativeScore = cumulativeScore;

                            //list of the student subject data and cumulative data
                            studentSubjectScoreList.Add(studentSubjectScore);

                            //check if the broadsheet data doesnt exists
                            var brdShtData = _context.BroadSheetData.Where(c => c.SchoolId == schoolId && c.CampusId == campusId && c.ClassId == classId
                                       && c.ClassGradeId == classGradeId && c.SessionId == sessionId && c.TermId == termId && c.StudentId == std.StudentId).FirstOrDefault();

                            if (brdShtData == null)
                            {
                                //save or update the broadsheetdata table
                                var broadSheetData = new BroadSheetData()
                                {
                                    StudentId = brdStudentInfo.Id,
                                    GenderId = brdStudentInfo.GenderId,
                                    SchoolId = schoolId,
                                    CampusId = campusId,
                                    ClassId = classId,
                                    ClassGradeId = classGradeId,
                                    TermId = termId,
                                    SessionId = sessionId,
                                    NoOfSubjectsComputed = subjectsInClass.Count(),
                                    TotalScore = totalScore,
                                    PercentageScore = percentageScore,
                                    Grade = grade,
                                    Remark = studentFinalRemark,
                                    DateComputed = DateTime.Today,
                                };

                                await _context.BroadSheetData.AddAsync(broadSheetData);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                //updates the data if it exists
                                brdShtData.StudentId = brdStudentInfo.Id;
                                brdShtData.SchoolId = schoolId;
                                brdShtData.CampusId = campusId;
                                brdShtData.ClassId = classId;
                                brdShtData.ClassGradeId = classGradeId;
                                brdShtData.TermId = termId;
                                brdShtData.SessionId = sessionId;
                                brdShtData.NoOfSubjectsComputed = subjectsInClass.Count();
                                brdShtData.TotalScore = totalScore;
                                brdShtData.PercentageScore = percentageScore;
                                brdShtData.Grade = grade;
                                brdShtData.Remark = studentFinalRemark;
                                brdShtData.DateComputed = DateTime.Today;

                                await _context.SaveChangesAsync();
                            }

                            countMandatorySubjectsPassed = 0;
                        countOtherSubjectsPassed = 0;
                        }
                    }

                    //final success response
                    response.StatusCode = 200;
                    response.StatusMessage = "Sucessful";
                    response.BroadSheetSummaryInfo = BroadSheetSummaryInfo;
                    response.BroadSheetHeaderInfo = BroadSheetHeaderInfo;
                    response.SchoolAndClassInfo = schoolAndClassInfo;
                    response.StudentSubjectScore = studentSubjectScoreList;
                }

                return response;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new BroadSheetRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<PerformanceRespModel> generatePerformanceAnalysisSheetAsync(PerformanceAnalysisReqModel obj)
        {
            try
            {
                PerformanceRespModel response = new PerformanceRespModel();
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkTerm = check.checkTermById(obj.TermId);
                var checkSession = check.checkSessionById(obj.SessionId);

                //the student subject score lists
                IList<PerformanceAnalysisInfo> perfAnalysisList = new List<PerformanceAnalysisInfo>();

                //check if all parameters supplied is Valid
                if (checkSchool && checkCampus && checkTerm && checkSession)
                {
                    PerformanceAnalysisSchoolnfo performanceAnalysisSchoolnfo = new PerformanceAnalysisSchoolnfo();
                    Schools schInfo = _context.Schools.Where(rp => rp.Id == obj.SchoolId).FirstOrDefault();
                    SchoolCampus campusInfo = _context.SchoolCampus.Where(rp => rp.Id == obj.CampusId).FirstOrDefault();

                    performanceAnalysisSchoolnfo.SchoolName = schInfo.SchoolName;
                    performanceAnalysisSchoolnfo.CampusName = campusInfo.CampusName;
                    performanceAnalysisSchoolnfo.CampusAddress = campusInfo.CampusAddress;
                    performanceAnalysisSchoolnfo.Session = reportCardReUsables.getCurrentSessionName(obj.SchoolId);
                    performanceAnalysisSchoolnfo.Term = reportCardReUsables.getTermName(obj.TermId);
                    performanceAnalysisSchoolnfo.DateGenerated = DateTime.Now.ToString();

                    foreach (var classId in obj.ClassIds)
                    {
                        var checkClass = check.checkClassById(classId);

                        if (checkClass)
                        {
                            PerformanceSummaryInfo registered = new PerformanceSummaryInfo();
                            PerformanceSummaryInfo present = new PerformanceSummaryInfo();
                            PerformanceSummaryInfo absent = new PerformanceSummaryInfo();
                            PerformanceSummaryInfo mandatory = new PerformanceSummaryInfo();
                            PerformanceSummaryInfo others = new PerformanceSummaryInfo();

                            //registered
                            //number of students registered (Male, Female, Total) in a classes (all arms put together)
                            long registeredMale = broadSheetReusables.getNumberOfStudentInClassByGender(obj.SchoolId, obj.CampusId, classId, (long)EnumUtility.Gender.Male);
                            long registeredFemale = broadSheetReusables.getNumberOfStudentInClassByGender(obj.SchoolId, obj.CampusId, classId, (long)EnumUtility.Gender.Female);
                            long totalRegistered = registeredMale + registeredFemale;

                            registered.Male = registeredMale;
                            registered.Female = registeredFemale;
                            registered.Total = totalRegistered;

                            //present
                            //number of students (Male, Female, Total) who took the exams in all the classes. (Students must take atleast 1 exam to be recorded as present).
                            present.Male = registeredMale;
                            present.Female = registeredFemale;
                            present.Total = totalRegistered;

                            //absent
                            //number of student who did not take any exams at all (Male, Female, Total) we can get this by subtracting number present from number registered
                            long absentMale = registeredMale - registeredMale;
                            long absentFemale = registeredFemale - registeredFemale;
                            long totalAbsent = absentMale + absentFemale;

                            absent.Male = absentMale;
                            absent.Female = absentFemale;
                            absent.Total = totalAbsent;

                            //mandatory
                            //Number who passed 6 subject including English and mathematics: These are those students who has pass as their remark.
                            long mandatoryMale = broadSheetReusables.getNumberOfStudentsPassedOrFailedByGender(obj.SchoolId, obj.CampusId, obj.SessionId, obj.TermId, classId, (long)EnumUtility.Gender.Male, "Passed");
                            long mandatoryFemale = broadSheetReusables.getNumberOfStudentsPassedOrFailedByGender(obj.SchoolId, obj.CampusId, obj.SessionId, obj.TermId, classId, (long)EnumUtility.Gender.Female, "Passed");
                            long totalMandatory = mandatoryMale + mandatoryFemale;

                            mandatory.Male = mandatoryMale;
                            mandatory.Female = mandatoryFemale;
                            mandatory.Total = totalMandatory;

                            //others
                            //hese are those students who has failed under the remark. These students did not meet the pass criteria.
                            long othersMale = broadSheetReusables.getNumberOfStudentsPassedOrFailedByGender(obj.SchoolId, obj.CampusId, obj.SessionId, obj.TermId, classId, (long)EnumUtility.Gender.Male, "Failed");
                            long othersFemale = broadSheetReusables.getNumberOfStudentsPassedOrFailedByGender(obj.SchoolId, obj.CampusId, obj.SessionId, obj.TermId, classId, (long)EnumUtility.Gender.Female, "Failed");
                            long totalOthers = othersMale + othersFemale;

                            others.Male = othersMale;
                            others.Female = othersFemale;
                            others.Total = totalOthers;

                            //this can be derived by dividing The TOTAL OF NUMBER OF STUDENT WHO PASSED 6 SUBJECTS INCLUDING ENGLISH AND MATHEMATICS by the TOTAL of NUMBER PRESENT then multiply by 100.
                            decimal percentagePass = 0;
                            if (totalRegistered > 0)
                            {
                                percentagePass = Math.Round((decimal)totalMandatory / totalRegistered * 100);
                            }

                            PerformanceAnalysisInfo perfAnalysis = new PerformanceAnalysisInfo();
                            perfAnalysis.Class = broadSheetReusables.getClass(classId); ;
                            perfAnalysis.Registered = registered;
                            perfAnalysis.Present = present;
                            perfAnalysis.Absent = absent;
                            perfAnalysis.Mandatory = mandatory;
                            perfAnalysis.Others = others;
                            perfAnalysis.PercentagePass = percentagePass;

                            perfAnalysisList.Add(perfAnalysis);
                        }
                    }

                    //final success response
                    response.StatusCode = 200;
                    response.StatusMessage = "Sucessful";
                    response.PerformanceAnalysisSchoolnfo = performanceAnalysisSchoolnfo;
                    response.PerformanceAnalysisInfo = perfAnalysisList;
                }

                return response;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new PerformanceRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }
    }
}

