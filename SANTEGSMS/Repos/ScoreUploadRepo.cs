using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using SANTEGSMS.IRepos;
using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using SANTEGSMS.Reusables;
using SANTEGSMS.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SANTEGSMS.Repos
{
    public class ScoreUploadRepo : IScoreUploadRepo
    {
        private readonly AppDbContext _context;
        private readonly ReportCardReUsables reportCardReUsables;
        private readonly IWebHostEnvironment env;
        //private readonly ServerPath _serverPath;

        public ScoreUploadRepo(AppDbContext context, ReportCardReUsables reportCardReUsables, IWebHostEnvironment env)
        {
            _context = context;
            this.reportCardReUsables = reportCardReUsables;
            this.env = env;
            //_serverPath = serverPath;
        }

        public async Task<UploadScoreRespModel> uploadScoresAsync(UploadSubjectScoreReqModel obj)
        {
            try
            {
                IList<object> existingScores = new List<object>();
                IList<object> newScores = new List<object>();
                IList<object> dataResponse = new List<object>();

                UploadScoreRespModel response = new UploadScoreRespModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    //--------------------------------------------------------------------EXAMINATION SCORES---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.Exam)
                    {
                        //Using one table for Category and SubCategory Configuration
                        var categoryConfig = _context.ScoreCategoryConfig.Where(s => s.CategoryId == obj.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                        && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                        if (categoryConfig != null)
                        {
                            var subCategoryConfig = _context.ScoreSubCategoryConfig.Where(s => s.Id == obj.SubCategoryId && s.CategoryId == categoryConfig.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                            && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                            if (subCategoryConfig != null)
                            {
                                foreach (StudentScoreList scr in obj.StudentScoreLists)
                                {
                                    //Check if the examination scores for student has previously been uploaded
                                    ExaminationScores examScore = _context.ExaminationScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                    && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                    && s.SubjectId == obj.SubjectId && s.StudentId == scr.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                    //the subject departmentId
                                    var departmentId = _context.SchoolSubjects.Where(s => s.Id == obj.SubjectId).FirstOrDefault().DepartmentId;
                                    //the Student AdmissionNumber
                                    var admissionNumber = _context.Students.Where(s => s.Id == scr.StudentId).FirstOrDefault().AdmissionNumber;

                                    if (examScore == null)
                                    {
                                        var examScr = new ExaminationScores
                                        {
                                            SchoolId = obj.SchoolId,
                                            CampusId = obj.CampusId,
                                            ClassId = obj.ClassId,
                                            ClassGradeId = obj.ClassGradeId,
                                            SessionId = obj.SessionId,
                                            TermId = obj.TermId,
                                            SubjectId = obj.SubjectId,
                                            DepartmentId = departmentId,
                                            StudentId = scr.StudentId,
                                            AdmissionNumber = admissionNumber,
                                            MarkObtainable = subCategoryConfig.ScoreObtainable, //score obtainable from the subCategory Configured by school
                                            MarkObtained = scr.MarkObtained,
                                            CategoryId = obj.CategoryId,
                                            SubCategoryId = obj.SubCategoryId,
                                            TeacherId = obj.TeacherId,
                                            DateUploaded = DateTime.Now,
                                            DateUpdated = DateTime.Now,
                                        };

                                        await _context.ExaminationScores.AddAsync(examScr);
                                        await _context.SaveChangesAsync();

                                        //Return the Scores uploaded
                                        var newScr = (from ex in _context.ExaminationScores
                                                      where ex.Id == examScr.Id
                                                      select new
                                                      {
                                                          ex.Id,
                                                          ex.SchoolId,
                                                          ex.CampusId,
                                                          ex.Classes.ClassName,
                                                          ex.ClassGrades.GradeName,
                                                          ex.Sessions.SessionName,
                                                          ex.Terms.TermName,
                                                          ex.SchoolSubjects.SubjectName,
                                                          ex.SubjectDepartment.DepartmentName,
                                                          StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                                          ex.Students.AdmissionNumber,
                                                          ex.MarkObtainable,
                                                          ex.MarkObtained,
                                                          ex.ScoreCategory.CategoryName,
                                                          ex.ScoreSubCategoryConfig.SubCategoryName,
                                                          TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                                          ex.DateUploaded,
                                                          ex.DateUpdated
                                                      }).FirstOrDefault();
                                        //list of scores uploaded
                                        newScores.Add(newScr);

                                        response.StatusCode = 200;
                                        response.StatusMessage = "Scores Uploaded Successfully!";
                                        response.ScoresUploaded = newScores.ToList();
                                    }
                                    else
                                    {
                                        response.StatusCode = 409;
                                        response.StatusMessage = "One or more Student Score Already Exits and Skipped!";
                                    }
                                }
                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "Kindly Configure the Exam Score SubCategory for this Class or SubCategory With the specified ID does not exist!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Exam Score Category for this Class!";
                        }
                    }

                    //--------------------------------------------------------------------CONTINOUS ASSESSMENT SCORES (CA)---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.CA)
                    {
                        //Using one table for Category and SubCategory Configuration
                        var categoryConfig = _context.ScoreCategoryConfig.Where(s => s.CategoryId == obj.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                        && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                        if (categoryConfig != null)
                        {
                            var subCategoryConfig = _context.ScoreSubCategoryConfig.Where(s => s.Id == obj.SubCategoryId && s.CategoryId == categoryConfig.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                            && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                            if (subCategoryConfig != null)
                            {
                                foreach (StudentScoreList scr in obj.StudentScoreLists)
                                {
                                    //Check if the examination scores for student has previously been uploaded
                                    ContinousAssessmentScores CAScore = _context.ContinousAssessmentScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                    && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                    && s.SubjectId == obj.SubjectId && s.StudentId == scr.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                    //the subject departmentId
                                    var departmentId = _context.SchoolSubjects.Where(s => s.Id == obj.SubjectId).FirstOrDefault().DepartmentId;
                                    //the Student AdmissionNumber
                                    var admissionNumber = _context.Students.Where(s => s.Id == scr.StudentId).FirstOrDefault().AdmissionNumber;

                                    if (CAScore == null)
                                    {
                                        var caScr = new ContinousAssessmentScores
                                        {
                                            SchoolId = obj.SchoolId,
                                            CampusId = obj.CampusId,
                                            ClassId = obj.ClassId,
                                            ClassGradeId = obj.ClassGradeId,
                                            SessionId = obj.SessionId,
                                            TermId = obj.TermId,
                                            SubjectId = obj.SubjectId,
                                            DepartmentId = departmentId,
                                            StudentId = scr.StudentId,
                                            AdmissionNumber = admissionNumber,
                                            MarkObtainable = subCategoryConfig.ScoreObtainable, //score obtainable from the subCategory Configured by school
                                            MarkObtained = scr.MarkObtained,
                                            CategoryId = obj.CategoryId,
                                            SubCategoryId = obj.SubCategoryId,
                                            TeacherId = obj.TeacherId,
                                            DateUploaded = DateTime.Now,
                                            DateUpdated = DateTime.Now,
                                        };

                                        await _context.ContinousAssessmentScores.AddAsync(caScr);
                                        await _context.SaveChangesAsync();

                                        //Return the Scores uploaded
                                        var newScr = (from ex in _context.ContinousAssessmentScores
                                                      where ex.Id == caScr.Id
                                                      select new
                                                      {
                                                          ex.Id,
                                                          ex.SchoolId,
                                                          ex.CampusId,
                                                          ex.Classes.ClassName,
                                                          ex.ClassGrades.GradeName,
                                                          ex.Sessions.SessionName,
                                                          ex.Terms.TermName,
                                                          ex.SchoolSubjects.SubjectName,
                                                          StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                                          ex.Students.AdmissionNumber,
                                                          ex.MarkObtainable,
                                                          ex.MarkObtained,
                                                          ex.ScoreCategory.CategoryName,
                                                          ex.ScoreSubCategoryConfig.SubCategoryName,
                                                          TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                                          ex.DateUploaded,
                                                          ex.DateUpdated
                                                      }).FirstOrDefault();
                                        //list of scores uploaded
                                        newScores.Add(newScr);

                                        response.StatusCode = 200;
                                        response.StatusMessage = "Scores Uploaded Successfully!";
                                        response.ScoresUploaded = newScores.ToList();
                                    }
                                    else
                                    {
                                        response.StatusCode = 409;
                                        response.StatusMessage = "One or more Student Score Already Exits and Skipped";
                                    }
                                }
                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "Kindly Configure the Continuous Assessment Score SubCategory for this Class or SubCategory With the specified ID does not exist!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Continuous Assessment Score SubCategory for this Class!";
                        }
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "A Parameter with the specified ID does not exist!";
                }

                return response;

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new UploadScoreRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getScoresBySubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Campus with the specified ID" };
                }
                else
                {
                    if (categoryId == (long)EnumUtility.ScoreCategory.Exam)
                    {
                        var result = from ex in _context.ExaminationScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                      && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                      && ex.SubjectId == subjectId && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId
                                     select new
                                     {
                                         ex.Id,
                                         ex.SchoolId,
                                         ex.CampusId,
                                         ex.Classes.ClassName,
                                         ex.ClassGrades.GradeName,
                                         ex.Sessions.SessionName,
                                         ex.Terms.TermName,
                                         ex.SchoolSubjects.SubjectName,
                                         StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                         ex.Students.AdmissionNumber,
                                         ex.MarkObtainable,
                                         ex.MarkObtained,
                                         ex.ScoreCategory.CategoryName,
                                         ex.ScoreSubCategoryConfig.SubCategoryName,
                                         TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                         ex.DateUploaded,
                                         ex.DateUpdated
                                     };

                        if (result.Count() > 0)
                        {
                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                        }

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                    }
                    else if (categoryId == (long)EnumUtility.ScoreCategory.CA)
                    {
                        var result = from ex in _context.ContinousAssessmentScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                       && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                       && ex.SubjectId == subjectId && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId
                                     select new
                                     {
                                         ex.Id,
                                         ex.SchoolId,
                                         ex.CampusId,
                                         ex.Classes.ClassName,
                                         ex.ClassGrades.GradeName,
                                         ex.Sessions.SessionName,
                                         ex.Terms.TermName,
                                         ex.SchoolSubjects.SubjectName,
                                         StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                         ex.Students.AdmissionNumber,
                                         ex.MarkObtainable,
                                         ex.MarkObtained,
                                         ex.ScoreCategory.CategoryName,
                                         ex.ScoreSubCategoryConfig.SubCategoryName,
                                         TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                         ex.DateUploaded,
                                         ex.DateUpdated
                                     };

                        if (result.Count() > 0)
                        {
                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                        }

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category", };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getScoresByStudentIdAndSubjectIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Campus with the specified ID" };
                }
                else
                {
                    if (categoryId == (long)EnumUtility.ScoreCategory.Exam)
                    {
                        var result = from ex in _context.ExaminationScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                      && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                      && ex.SubjectId == subjectId && ex.StudentId == studentId && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId
                                     select new
                                     {
                                         ex.Id,
                                         ex.SchoolId,
                                         ex.CampusId,
                                         ex.Classes.ClassName,
                                         ex.ClassGrades.GradeName,
                                         ex.Sessions.SessionName,
                                         ex.Terms.TermName,
                                         ex.SchoolSubjects.SubjectName,
                                         StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                         ex.Students.AdmissionNumber,
                                         ex.MarkObtainable,
                                         ex.MarkObtained,
                                         ex.ScoreCategory.CategoryName,
                                         ex.ScoreSubCategoryConfig.SubCategoryName,
                                         TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                         ex.DateUploaded,
                                         ex.DateUpdated
                                     };

                        if (result.Count() > 0)
                        {
                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                        }

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                    }
                    else if (categoryId == (long)EnumUtility.ScoreCategory.CA)
                    {
                        var result = from ex in _context.ContinousAssessmentScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                      && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                      && ex.SubjectId == subjectId && ex.StudentId == studentId && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId
                                     select new
                                     {
                                         ex.Id,
                                         ex.SchoolId,
                                         ex.CampusId,
                                         ex.Classes.ClassName,
                                         ex.ClassGrades.GradeName,
                                         ex.Sessions.SessionName,
                                         ex.Terms.TermName,
                                         ex.SchoolSubjects.SubjectName,
                                         StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                         ex.Students.AdmissionNumber,
                                         ex.MarkObtainable,
                                         ex.MarkObtained,
                                         ex.ScoreCategory.CategoryName,
                                         ex.ScoreSubCategoryConfig.SubCategoryName,
                                         TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                         ex.DateUploaded,
                                         ex.DateUpdated
                                     };

                        if (result.Count() > 0)
                        {
                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                        }

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category", };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        public async Task<GenericRespModel> getAllScoresByStudentIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Campus with the specified ID" };
                }
                else
                {
                    if (categoryId == (long)EnumUtility.ScoreCategory.Exam)
                    {
                        var result = from ex in _context.ExaminationScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                      && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                     && ex.StudentId == studentId && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId
                                     select new
                                     {
                                         ex.Id,
                                         ex.SchoolId,
                                         ex.CampusId,
                                         ex.Classes.ClassName,
                                         ex.ClassGrades.GradeName,
                                         ex.Sessions.SessionName,
                                         ex.Terms.TermName,
                                         ex.SchoolSubjects.SubjectName,
                                         StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                         ex.Students.AdmissionNumber,
                                         ex.MarkObtainable,
                                         ex.MarkObtained,
                                         ex.ScoreCategory.CategoryName,
                                         ex.ScoreSubCategoryConfig.SubCategoryName,
                                         TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                         ex.DateUploaded,
                                         ex.DateUpdated
                                     };

                        if (result.Count() > 0)
                        {
                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                        }

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                    }
                    else if (categoryId == (long)EnumUtility.ScoreCategory.CA)
                    {
                        var result = from ex in _context.ContinousAssessmentScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                       && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                       && ex.StudentId == studentId && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId
                                     select new
                                     {
                                         ex.Id,
                                         ex.SchoolId,
                                         ex.CampusId,
                                         ex.Classes.ClassName,
                                         ex.ClassGrades.GradeName,
                                         ex.Sessions.SessionName,
                                         ex.Terms.TermName,
                                         ex.SchoolSubjects.SubjectName,
                                         StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                         ex.Students.AdmissionNumber,
                                         ex.MarkObtainable,
                                         ex.MarkObtained,
                                         ex.ScoreCategory.CategoryName,
                                         ex.ScoreSubCategoryConfig.SubCategoryName,
                                         TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                         ex.DateUploaded,
                                         ex.DateUpdated
                                     };

                        if (result.Count() > 0)
                        {
                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                        }

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category", };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<UploadScoreRespModel> updateScoresAsync(UploadSubjectScoreReqModel obj)
        {
            try
            {
                IList<object> existingScores = new List<object>();
                IList<object> newScores = new List<object>();
                IList<object> dataResponse = new List<object>();

                UploadScoreRespModel response = new UploadScoreRespModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.Exam)
                    {
                        //Using one table for Category and SubCategory Configuration
                        var categoryConfig = _context.ScoreCategoryConfig.Where(s => s.CategoryId == obj.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                        && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                        if (categoryConfig != null)
                        {
                            var subCategoryConfig = _context.ScoreSubCategoryConfig.Where(s => s.Id == obj.SubCategoryId && s.CategoryId == categoryConfig.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                            && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                            if (subCategoryConfig != null)
                            {
                                foreach (StudentScoreList scr in obj.StudentScoreLists)
                                {
                                    //Check if the examination scores for student has previously been uploaded
                                    ExaminationScores examScore = _context.ExaminationScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                    && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                    && s.SubjectId == obj.SubjectId && s.StudentId == scr.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                    //the subject departmentId
                                    var departmentId = _context.SchoolSubjects.Where(s => s.Id == obj.SubjectId).FirstOrDefault().DepartmentId;
                                    //the Student AdmissionNumber
                                    var admissionNumber = _context.Students.Where(s => s.Id == scr.StudentId).FirstOrDefault().AdmissionNumber;

                                    if (examScore != null)
                                    {
                                        examScore.SchoolId = obj.SchoolId;
                                        examScore.CampusId = obj.CampusId;
                                        examScore.ClassId = obj.ClassId;
                                        examScore.ClassGradeId = obj.ClassGradeId;
                                        examScore.SessionId = obj.SessionId;
                                        examScore.TermId = obj.TermId;
                                        examScore.SubjectId = obj.SubjectId;
                                        examScore.DepartmentId = departmentId;
                                        examScore.StudentId = scr.StudentId;
                                        examScore.AdmissionNumber = admissionNumber;
                                        examScore.MarkObtainable = subCategoryConfig.ScoreObtainable; //score obtainable from the subCategory Configured by school
                                        examScore.MarkObtained = scr.MarkObtained;
                                        examScore.CategoryId = obj.CategoryId;
                                        examScore.SubCategoryId = obj.SubCategoryId;
                                        examScore.TeacherId = obj.TeacherId;
                                        examScore.DateUpdated = DateTime.Now;

                                        await _context.SaveChangesAsync();

                                        response.StatusCode = 200;
                                        response.StatusMessage = "Scores Updated Successfully!";
                                    }
                                    else
                                    {
                                        response.StatusCode = 409;
                                        response.StatusMessage = "Scores does not Exists!";
                                    }
                                }
                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "Kindly Configure the Exam Score SubCategory for this Class or SubCategory with the specified ID does not exist!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Exam Score Category for this Class!";
                        }
                    }

                    //--------------------------------------------------------------------CONTINOUS ASSESSMENT SCORES (CA)---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.CA)
                    {
                        //Using one table for Category and SubCategory Configuration
                        var categoryConfig = _context.ScoreCategoryConfig.Where(s => s.CategoryId == obj.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                        && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                        if (categoryConfig != null)
                        {
                            var subCategoryConfig = _context.ScoreSubCategoryConfig.Where(s => s.Id == obj.SubCategoryId && s.CategoryId == categoryConfig.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                            && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                            if (subCategoryConfig != null)
                            {
                                foreach (StudentScoreList scr in obj.StudentScoreLists)
                                {
                                    //Check if the examination scores for student has previously been uploaded
                                    ContinousAssessmentScores CAScore = _context.ContinousAssessmentScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                    && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                    && s.SubjectId == obj.SubjectId && s.StudentId == scr.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                    //the subject departmentId
                                    var departmentId = _context.SchoolSubjects.Where(s => s.Id == obj.SubjectId).FirstOrDefault().DepartmentId;
                                    //the Student AdmissionNumber
                                    var admissionNumber = _context.Students.Where(s => s.Id == scr.StudentId).FirstOrDefault().AdmissionNumber;

                                    if (CAScore != null)
                                    {
                                        CAScore.SchoolId = obj.SchoolId;
                                        CAScore.CampusId = obj.CampusId;
                                        CAScore.ClassId = obj.ClassId;
                                        CAScore.ClassGradeId = obj.ClassGradeId;
                                        CAScore.SessionId = obj.SessionId;
                                        CAScore.TermId = obj.TermId;
                                        CAScore.SubjectId = obj.SubjectId;
                                        CAScore.DepartmentId = departmentId;
                                        CAScore.StudentId = scr.StudentId;
                                        CAScore.AdmissionNumber = admissionNumber;
                                        CAScore.MarkObtainable = subCategoryConfig.ScoreObtainable; //score obtainable from the subCategory Configured by school
                                        CAScore.MarkObtained = scr.MarkObtained;
                                        CAScore.CategoryId = obj.CategoryId;
                                        CAScore.SubCategoryId = obj.SubCategoryId;
                                        CAScore.TeacherId = obj.TeacherId;
                                        CAScore.DateUpdated = DateTime.Now;

                                        await _context.SaveChangesAsync();

                                        response.StatusCode = 200;
                                        response.StatusMessage = "Scores Updated Successfully!";
                                    }
                                    else
                                    {
                                        response.StatusCode = 409;
                                        response.StatusMessage = "Scores does not Exists!";
                                    }
                                }
                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "Kindly Configure the Continuous Assessment Score SubCategory for this Class or SubCategory with the specified ID does not exist!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Continuous Assessment Score SubCategory for this Class!";
                        }
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "A Parameter with the Specified ID does not Exist!";
                }

                return response;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new UploadScoreRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        // <summary>
        // Uploading scores for a single student for a subject
        // </summary>
        // <param name = "obj" ></ param >
        // < returns ></ returns >
        public async Task<UploadScoreRespModel> uploadSingleStudentScoreAsync(UploadScorePerSubjectAndStudentReqModel obj)
        {
            try
            {
                IList<object> existingScores = new List<object>();
                IList<object> newScores = new List<object>();
                IList<object> dataResponse = new List<object>();

                UploadScoreRespModel response = new UploadScoreRespModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    //--------------------------------------------------------------------EXAMINATION SCORES---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.Exam)
                    {
                        //Using one table for Category and SubCategory Configuration
                        var categoryConfig = _context.ScoreCategoryConfig.Where(s => s.CategoryId == obj.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                        && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                        if (categoryConfig != null)
                        {
                            var subCategoryConfig = _context.ScoreSubCategoryConfig.Where(s => s.Id == obj.SubCategoryId && s.CategoryId == categoryConfig.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                            && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                            if (subCategoryConfig != null)
                            {
                                //Check if the examination scores for student has previously been uploaded
                                ExaminationScores examScore = _context.ExaminationScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                && s.SubjectId == obj.SubjectId && s.StudentId == obj.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                //the subject departmentId
                                var departmentId = _context.SchoolSubjects.Where(s => s.Id == obj.SubjectId).FirstOrDefault().DepartmentId;
                                //the Student AdmissionNumber
                                var admissionNumber = _context.Students.Where(s => s.Id == obj.StudentId).FirstOrDefault().AdmissionNumber;

                                //if Exam scores does not exit, Save the new Exam scores
                                if (examScore == null)
                                {
                                    var examScr = new ExaminationScores
                                    {
                                        SchoolId = obj.SchoolId,
                                        CampusId = obj.CampusId,
                                        ClassId = obj.ClassId,
                                        ClassGradeId = obj.ClassGradeId,
                                        SessionId = obj.SessionId,
                                        TermId = obj.TermId,
                                        SubjectId = obj.SubjectId,
                                        DepartmentId = departmentId,
                                        StudentId = obj.StudentId,
                                        AdmissionNumber = admissionNumber,
                                        MarkObtainable = subCategoryConfig.ScoreObtainable, //score obtainable from the subCategory Configured by school
                                        MarkObtained = obj.MarkObtained,
                                        CategoryId = obj.CategoryId,
                                        SubCategoryId = obj.SubCategoryId,
                                        TeacherId = obj.TeacherId,
                                        DateUploaded = DateTime.Now,
                                        DateUpdated = DateTime.Now,
                                    };

                                    await _context.ExaminationScores.AddAsync(examScr);
                                    await _context.SaveChangesAsync();

                                    //Return the Scores uploaded
                                    var newScr = (from ex in _context.ExaminationScores
                                                  where ex.Id == examScr.Id
                                                  select new
                                                  {
                                                      ex.Id,
                                                      ex.SchoolId,
                                                      ex.CampusId,
                                                      ex.Classes.ClassName,
                                                      ex.ClassGrades.GradeName,
                                                      ex.Sessions.SessionName,
                                                      ex.Terms.TermName,
                                                      ex.SchoolSubjects.SubjectName,
                                                      StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                                      ex.Students.AdmissionNumber,
                                                      ex.MarkObtainable,
                                                      ex.MarkObtained,
                                                      ex.ScoreCategory.CategoryName,
                                                      ex.ScoreSubCategoryConfig.SubCategoryName,
                                                      TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                                      ex.DateUploaded,
                                                      ex.DateUpdated
                                                  }).FirstOrDefault();
                                    //list of scores uploaded
                                    newScores.Add(newScr);

                                    response.StatusCode = 200;
                                    response.StatusMessage = "Scores Uploaded Successfully!";
                                    response.ScoresUploaded = newScores.ToList();
                                }
                                else
                                {
                                    response.StatusCode = 409;
                                    response.StatusMessage = "One or more Student Score Already Exits and Skipped!";
                                }
                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "Kindly Configure the Exam Score SubCategory for this Class or SubCategory with the specified ID does not exist!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Exam Score Category for this Class!";
                        }
                    }

                    //--------------------------------------------------------------------CONTINOUS ASSESSMENT SCORES (CA)---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.CA)
                    {
                        //Using one table for Category and SubCategory Configuration
                        var categoryConfig = _context.ScoreCategoryConfig.Where(s => s.CategoryId == obj.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                        && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                        if (categoryConfig != null)
                        {
                            var subCategoryConfig = _context.ScoreSubCategoryConfig.Where(s => s.Id == obj.SubCategoryId && s.CategoryId == categoryConfig.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                            && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                            if (subCategoryConfig != null)
                            {
                                //Check if the examination scores for student has previously been uploaded
                                ContinousAssessmentScores CAScore = _context.ContinousAssessmentScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                && s.SubjectId == obj.SubjectId && s.StudentId == obj.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                //the subject departmentId
                                var departmentId = _context.SchoolSubjects.Where(s => s.Id == obj.SubjectId).FirstOrDefault().DepartmentId;
                                //the Student AdmissionNumber
                                var admissionNumber = _context.Students.Where(s => s.Id == obj.StudentId).FirstOrDefault().AdmissionNumber;

                                //if CA scores does not exit, Save the new CA scores
                                if (CAScore == null)
                                {
                                    var caScr = new ContinousAssessmentScores
                                    {
                                        SchoolId = obj.SchoolId,
                                        CampusId = obj.CampusId,
                                        ClassId = obj.ClassId,
                                        ClassGradeId = obj.ClassGradeId,
                                        SessionId = obj.SessionId,
                                        TermId = obj.TermId,
                                        SubjectId = obj.SubjectId,
                                        DepartmentId = departmentId,
                                        StudentId = obj.StudentId,
                                        AdmissionNumber = admissionNumber,
                                        MarkObtainable = subCategoryConfig.ScoreObtainable, //score obtainable from the subCategory Configured by school
                                        MarkObtained = obj.MarkObtained,
                                        CategoryId = obj.CategoryId,
                                        SubCategoryId = obj.SubCategoryId,
                                        TeacherId = obj.TeacherId,
                                        DateUploaded = DateTime.Now,
                                        DateUpdated = DateTime.Now,
                                    };

                                    await _context.ContinousAssessmentScores.AddAsync(caScr);
                                    await _context.SaveChangesAsync();

                                    //Return the Scores uploaded
                                    var newScr = (from ex in _context.ContinousAssessmentScores
                                                  where ex.Id == caScr.Id
                                                  select new
                                                  {
                                                      ex.Id,
                                                      ex.SchoolId,
                                                      ex.CampusId,
                                                      ex.Classes.ClassName,
                                                      ex.ClassGrades.GradeName,
                                                      ex.Sessions.SessionName,
                                                      ex.Terms.TermName,
                                                      ex.SchoolSubjects.SubjectName,
                                                      StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                                      ex.Students.AdmissionNumber,
                                                      ex.MarkObtainable,
                                                      ex.MarkObtained,
                                                      ex.ScoreCategory.CategoryName,
                                                      ex.ScoreSubCategoryConfig.SubCategoryName,
                                                      TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                                      ex.DateUploaded,
                                                      ex.DateUpdated
                                                  }).FirstOrDefault();
                                    //list of scores uploaded
                                    newScores.Add(newScr);

                                    response.StatusCode = 200;
                                    response.StatusMessage = "Scores Uploaded Successfully!";
                                    response.ScoresUploaded = newScores.ToList();
                                }
                                else
                                {
                                    response.StatusCode = 409;
                                    response.StatusMessage = "One or more Student Score Already Exits!";
                                }
                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "Kindly Configure the Continuous Assessment Score SubCategory for this Class or SubCategory with the specified ID does not exist!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Continuous Assessment Score Category for this Class!";
                        }
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "A Paremeter With Specified ID does not exist!";
                }

                return response;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new UploadScoreRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        public async Task<UploadScoreRespModel> updateSingleStudentScoresAsync(UploadScorePerSubjectAndStudentReqModel obj)
        {
            try
            {
                var response = new UploadScoreRespModel();
                IList<object> existingScores = new List<object>();
                IList<object> newScores = new List<object>();
                IList<object> dataResponse = new List<object>();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.Exam)
                    {
                        //Using one table for Category and SubCategory Configuration
                        var categoryConfig = _context.ScoreCategoryConfig.Where(s => s.CategoryId == obj.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                        && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                        if (categoryConfig != null)
                        {
                            var subCategoryConfig = _context.ScoreSubCategoryConfig.Where(s => s.Id == obj.SubCategoryId && s.CategoryId == categoryConfig.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                            && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                            if (subCategoryConfig != null)
                            {
                                //Check if the examination scores for student has previously been uploaded
                                ExaminationScores examScore = _context.ExaminationScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                && s.SubjectId == obj.SubjectId && s.StudentId == obj.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                //the subject departmentId
                                var departmentId = _context.SchoolSubjects.Where(s => s.Id == obj.SubjectId).FirstOrDefault().DepartmentId;
                                //the Student AdmissionNumber
                                var admissionNumber = _context.Students.Where(s => s.Id == obj.StudentId).FirstOrDefault().AdmissionNumber;

                                if (examScore != null)
                                {
                                    examScore.SchoolId = obj.SchoolId;
                                    examScore.CampusId = obj.CampusId;
                                    examScore.ClassId = obj.ClassId;
                                    examScore.ClassGradeId = obj.ClassGradeId;
                                    examScore.SessionId = obj.SessionId;
                                    examScore.TermId = obj.TermId;
                                    examScore.SubjectId = obj.SubjectId;
                                    examScore.DepartmentId = departmentId;
                                    examScore.StudentId = obj.StudentId;
                                    examScore.AdmissionNumber = admissionNumber;
                                    examScore.MarkObtainable = subCategoryConfig.ScoreObtainable; //score obtainable from the subCategory Configured by school
                                    examScore.MarkObtained = obj.MarkObtained;
                                    examScore.CategoryId = obj.CategoryId;
                                    examScore.SubCategoryId = obj.SubCategoryId;
                                    examScore.TeacherId = obj.TeacherId;
                                    examScore.DateUpdated = DateTime.Now;

                                    await _context.SaveChangesAsync();

                                    response.StatusCode = 200;
                                    response.StatusMessage = "Scores Updated Successfully!";
                                }
                                else
                                {
                                    response.StatusCode = 409;
                                    response.StatusMessage = "Scores does not Exists!";
                                }
                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "Kindly Configure the Exam Score SubCategory for this Class or SubCategory with specified ID does not exists!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Exam Score Category for this Class!";
                        }
                    }

                    //--------------------------------------------------------------------CONTINOUS ASSESSMENT SCORES (CA)---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.CA)
                    {
                        //Using one table for Category and SubCategory Configuration
                        var categoryConfig = _context.ScoreCategoryConfig.Where(s => s.CategoryId == obj.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                        && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                        if (categoryConfig != null)
                        {
                            var subCategoryConfig = _context.ScoreSubCategoryConfig.Where(s => s.Id == obj.SubCategoryId && s.CategoryId == categoryConfig.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                            && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                            if (subCategoryConfig != null)
                            {
                                //Check if the examination scores for student has previously been uploaded
                                ContinousAssessmentScores CAScore = _context.ContinousAssessmentScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                && s.SubjectId == obj.SubjectId && s.StudentId == obj.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                //the subject departmentId
                                var departmentId = _context.SchoolSubjects.Where(s => s.Id == obj.SubjectId).FirstOrDefault().DepartmentId;
                                //the Student AdmissionNumber
                                var admissionNumber = _context.Students.Where(s => s.Id == obj.StudentId).FirstOrDefault().AdmissionNumber;

                                if (CAScore != null)
                                {
                                    CAScore.SchoolId = obj.SchoolId;
                                    CAScore.CampusId = obj.CampusId;
                                    CAScore.ClassId = obj.ClassId;
                                    CAScore.ClassGradeId = obj.ClassGradeId;
                                    CAScore.SessionId = obj.SessionId;
                                    CAScore.TermId = obj.TermId;
                                    CAScore.SubjectId = obj.SubjectId;
                                    CAScore.DepartmentId = departmentId;
                                    CAScore.StudentId = obj.StudentId;
                                    CAScore.AdmissionNumber = admissionNumber;
                                    CAScore.MarkObtainable = subCategoryConfig.ScoreObtainable; //score obtainable from the subCategory Configured by school
                                    CAScore.MarkObtained = obj.MarkObtained;
                                    CAScore.CategoryId = obj.CategoryId;
                                    CAScore.SubCategoryId = obj.SubCategoryId;
                                    CAScore.TeacherId = obj.TeacherId;
                                    CAScore.DateUpdated = DateTime.Now;

                                    await _context.SaveChangesAsync();

                                    response.StatusCode = 200;
                                    response.StatusMessage = "Scores Updated Successfully!";
                                }
                                else
                                {
                                    response.StatusCode = 409;
                                    response.StatusMessage = "Scores does not Exists!";
                                }
                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "Kindly Configure the Continuous Assessment Score SubCategory for this Class or SubCategory With Specified ID does not Exists";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Continuous Assessment Score Category for this Class!";
                        }
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "A Parameter with the Specified ID does not Exist!";
                }

                return response;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new UploadScoreRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> deleteScoresPerSubjectForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            try
            {
                if (categoryId == (long)EnumUtility.ScoreCategory.Exam)
                {
                    //Check if the ExaminationScores scores for student Exists
                    ExaminationScores examScore = _context.ExaminationScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.SubjectId == subjectId && s.StudentId == studentId && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId).FirstOrDefault();

                    if (examScore != null)
                    {
                        _context.ExaminationScores.Remove(examScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Student Score Deleted Successfully", };

                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }
                else if (categoryId == (long)EnumUtility.ScoreCategory.CA)
                {
                    //Check if the ContinousAssessmentScores scores for student Exists
                    ContinousAssessmentScores CAScore = _context.ContinousAssessmentScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.SubjectId == subjectId && s.StudentId == studentId && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId).FirstOrDefault();

                    if (CAScore != null)
                    {
                        _context.ContinousAssessmentScores.Remove(CAScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Student Score Deleted Successfully", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category(Exam/CA)", };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> deleteScoresPerSubjectForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            try
            {
                if (categoryId == (long)EnumUtility.ScoreCategory.Exam)
                {
                    //Check if the ExaminationScores scores for student Exists
                    IList<ExaminationScores> examScore = (_context.ExaminationScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.SubjectId == subjectId && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId)).ToList();

                    if (examScore.Count() > 0)
                    {
                        _context.ExaminationScores.RemoveRange(examScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }
                else if (categoryId == (long)EnumUtility.ScoreCategory.CA)
                {
                    //Check if the ContinousAssessmentScores scores for student Exists
                    IList<ContinousAssessmentScores> CAScore = (_context.ContinousAssessmentScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.SubjectId == subjectId && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId)).ToList();

                    if (CAScore.Count() > 0)
                    {
                        _context.ContinousAssessmentScores.RemoveRange(CAScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };

                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category(Exam/CA)", };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> deleteScoresPerCategoryForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            try
            {
                if (categoryId == (long)EnumUtility.ScoreCategory.Exam)
                {
                    //Check if the ExaminationScores scores for student Exists
                    IList<ExaminationScores> examScore = (_context.ExaminationScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.StudentId == studentId && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId)).ToList();

                    if (examScore.Count() > 0)
                    {
                        _context.ExaminationScores.RemoveRange(examScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };

                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }
                else if (categoryId == (long)EnumUtility.ScoreCategory.CA)
                {
                    //Check if the ContinousAssessmentScores scores for student Exists
                    IList<ContinousAssessmentScores> CAScore = (_context.ContinousAssessmentScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.StudentId == studentId && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId)).ToList();

                    if (CAScore.Count() > 0)
                    {
                        _context.ContinousAssessmentScores.RemoveRange(CAScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };

                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category(Exam/CA)", };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> deleteScoresPerCategoryForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            try
            {
                if (categoryId == (long)EnumUtility.ScoreCategory.Exam)
                {
                    //Check if the ExaminationScores scores for student Exists
                    IList<ExaminationScores> examScore = (_context.ExaminationScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId)).ToList();

                    if (examScore.Count() > 0)
                    {
                        _context.ExaminationScores.RemoveRange(examScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }
                else if (categoryId == (long)EnumUtility.ScoreCategory.CA)
                {
                    //Check if the ContinousAssessmentScores scores for student Exists
                    IList<ContinousAssessmentScores> CAScore = (_context.ContinousAssessmentScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId)).ToList();

                    if (CAScore.Count() > 0)
                    {
                        _context.ContinousAssessmentScores.RemoveRange(CAScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };

                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category(Exam/CA)", };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<UploadScoreRespModel> bulkScoresUploadAsync(BulkScoresUploadReqModel obj)
        {
            try
            {
                //Return object
                IList<object> data = new List<object>();
                IList<object> newScores = new List<object>();

                //Response Model object
                UploadScoreRespModel response = new UploadScoreRespModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);

                //if the selected Category to be uploaded is Exam
                if (obj.CategoryId == (long)EnumUtility.ScoreCategory.Exam)
                {
                    //check if all parameters supplied is Valid
                    if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                    {
                        // get the ScoressheetTemplate
                        var scoreSheetTemplate = _context.ScoreUploadSheetTemplates.Where(x => x.Id == obj.ScoreSheetTemplateId).FirstOrDefault();

                        if (scoreSheetTemplate != null && scoreSheetTemplate.IsUsed == false)
                        {
                            //the subjectId's generated from the template
                            string[] subjectId = scoreSheetTemplate.SubjectId.Split(',');

                            //string subFolderName = "Others";

                            //the file path
                            //get the defined filepath (e.g. @"C:\inetpub\wwwroot\SoftlearnMedia")
                            //string serverPath = _serverPath.ServerFolderPath((int)EnumUtility.AppName.SchoolApp, subFolderName);

                            //the file path
                            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", obj.File.FileName);
                            //copy the file to the stream and read from the file

                            //the file path to save the file
                            //var FilePath = Path.Combine(serverPath, obj.File.FileName);

                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                await obj.File.CopyToAsync(stream);
                            }

                            FileInfo existingFile = new FileInfo(FilePath);
                            using (ExcelPackage package = new ExcelPackage(existingFile))
                            {
                                //get the first worksheet in the workbook
                                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                                int colCount = worksheet.Dimension.Columns;  //get Column Count
                                int rowCount = worksheet.Dimension.Rows;     //get row count


                                for (int rowCounts = 2; rowCounts <= rowCount; rowCounts++) // starts from the second row (Jumping the table headings)
                                {
                                    //initialize the count starting from 1 for each row count in the excel sheet
                                    int count = 1;

                                    foreach (var subId in subjectId)
                                    {
                                        //Using one table for Category and SubCategory Configuration
                                        var categoryConfig = _context.ScoreCategoryConfig.Where(s => s.CategoryId == obj.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                        && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                                        if (categoryConfig != null)
                                        {
                                            //get the subcategory from the scoreSubCategoryConfig 
                                            var subCategoryConfig = _context.ScoreSubCategoryConfig.Where(s => s.Id == obj.SubCategoryId && s.CategoryId == categoryConfig.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                            && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                                            if (subCategoryConfig != null)
                                            {
                                                //get the student by the admissionNumber
                                                var getStudent =  _context.Students.Where(s => s.AdmissionNumber == worksheet.Cells[rowCounts, 3].Value.ToString().Trim()).FirstOrDefault();

                                                //Check if the examination scores for student has previously been uploaded
                                                ExaminationScores examScore = _context.ExaminationScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                                && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                                && s.SubjectId == Convert.ToInt64(subId) && s.StudentId == getStudent.Id && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                                //the subject departmentId
                                                var departmentId = _context.SchoolSubjects.Where(s => s.Id == Convert.ToInt64(subId)).FirstOrDefault().DepartmentId;
                                                //the mark obtained
                                                decimal markObtained = Convert.ToDecimal(worksheet.Cells[rowCounts, count + 3].Value.ToString());

                                                if (examScore == null)
                                                {
                                                    var examScr = new ExaminationScores
                                                    {
                                                        SchoolId = obj.SchoolId,
                                                        CampusId = obj.CampusId,
                                                        ClassId = obj.ClassId,
                                                        ClassGradeId = obj.ClassGradeId,
                                                        SessionId = obj.SessionId,
                                                        TermId = obj.TermId,
                                                        SubjectId = Convert.ToInt64(subId),
                                                        DepartmentId = departmentId,
                                                        StudentId = getStudent.Id,
                                                        AdmissionNumber = worksheet.Cells[rowCounts, 3].Value.ToString().Trim(), //AdmissionNumber Row
                                                        MarkObtainable = subCategoryConfig.ScoreObtainable, //score obtainable from the subCategory Configured by school
                                                        MarkObtained = Convert.ToDecimal(worksheet.Cells[rowCounts, count + 3].Value.ToString()), //MarkObtained Row
                                                        CategoryId = obj.CategoryId,
                                                        SubCategoryId = obj.SubCategoryId,
                                                        TeacherId = obj.TeacherId,
                                                        DateUploaded = DateTime.Now,
                                                        DateUpdated = DateTime.Now,
                                                    };

                                                    await _context.ExaminationScores.AddAsync(examScr);
                                                    await _context.SaveChangesAsync();

                                                    //Return the Scores uploaded
                                                    var newScr = (from ex in _context.ExaminationScores
                                                                  where ex.Id == examScr.Id
                                                                  select new
                                                                  {
                                                                      ex.Id,
                                                                      ex.SchoolId,
                                                                      ex.CampusId,
                                                                      ex.Classes.ClassName,
                                                                      ex.ClassGrades.GradeName,
                                                                      ex.Sessions.SessionName,
                                                                      ex.Terms.TermName,
                                                                      ex.SchoolSubjects.SubjectName,
                                                                      ex.SubjectDepartment.DepartmentName,
                                                                      StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                                                      ex.Students.AdmissionNumber,
                                                                      ex.MarkObtainable,
                                                                      ex.MarkObtained,
                                                                      ex.ScoreCategory.CategoryName,
                                                                      ex.ScoreSubCategoryConfig.SubCategoryName,
                                                                      TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                                                      ex.DateUploaded,
                                                                      ex.DateUpdated
                                                                  }).FirstOrDefault();
                                                    //list of scores uploaded
                                                    newScores.Add(newScr);

                                                    response.StatusCode = 200;
                                                    response.StatusMessage = "Scores Uploaded Successfully!";
                                                    response.ScoresUploaded = newScores.ToList();

                                                    //Update the ScoreSheetTemplate IsUsed to True
                                                    scoreSheetTemplate.IsUsed = true;
                                                    await _context.SaveChangesAsync();

                                                }
                                                else
                                                {
                                                    response.StatusCode = 409;
                                                    response.StatusMessage = "One or more Student Score Already Exits and Skipped";
                                                }
                                            }
                                            else
                                            {
                                                response.StatusCode = 409;
                                                response.StatusMessage = "Kindly Configure the Exam Score SubCategory for this Class or SubCategory with the specified ID does not Exists";
                                            }
                                        }
                                        else
                                        {
                                            response.StatusCode = 409;
                                            response.StatusMessage = "Kindly Configure the Exam Score Category for this Class!";
                                        }

                                        count++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            response.StatusCode = 200;
                            response.StatusMessage = "ScoreSheet Template with the specified ID does not exist or ScoreSheet Template has been Used!";
                        }
                    }
                    else
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "A Paremeter With Specified ID does not exist!";
                    }
                }

                //-------------------------------------------------CONTINOUS ASSESSMENT(CA)-------------------------------------------------------------------------

                //if the selected Category to be uploaded is Exam
                if (obj.CategoryId == (long)EnumUtility.ScoreCategory.CA)
                {
                    //check if all parameters supplied is Valid
                    if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                    {
                        // get the ScoressheetTemplate
                        var scoreSheetTemplate = _context.ScoreUploadSheetTemplates.Where(x => x.Id == obj.ScoreSheetTemplateId).FirstOrDefault();

                        if (scoreSheetTemplate != null && scoreSheetTemplate.IsUsed == false)
                        {
                            //the subjectId's generated from the template
                            string[] subjectId = scoreSheetTemplate.SubjectId.Split(',');

                            //string subFolderName = "Others";

                            //the file path
                            //get the defined filepath (e.g. @"C:\inetpub\wwwroot\SoftlearnMedia")
                            //string serverPath = _serverPath.ServerFolderPath((int)EnumUtility.AppName.SchoolApp, subFolderName);

                            //the file path
                            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", obj.File.FileName);
                            //copy the file to the stream and read from the file

                            //the file path to save the file
                            //var FilePath = Path.Combine(serverPath, obj.File.FileName);

                            using (var stream = new FileStream(FilePath, FileMode.Create))
                            {
                                await obj.File.CopyToAsync(stream);
                            }

                            FileInfo existingFile = new FileInfo(FilePath);
                            using (ExcelPackage package = new ExcelPackage(existingFile))
                            {
                                //get the first worksheet in the workbook
                                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                                int colCount = worksheet.Dimension.Columns;  //get Column Count
                                int rowCount = worksheet.Dimension.Rows;     //get row count


                                for (int rowCounts = 2; rowCounts <= rowCount; rowCounts++) // starts from the second row (Jumping the table headings)
                                {
                                    //initialize the count starting from 1 for each row count in the excel sheet
                                    int count = 1;

                                    foreach (var subId in subjectId)
                                    {
                                        //Using one table for Category and SubCategory Configuration
                                        var categoryConfig = _context.ScoreCategoryConfig.Where(s => s.CategoryId == obj.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                        && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                                        if (categoryConfig != null)
                                        {
                                            //get the subcategory from the scoreSubCategoryConfig 
                                            var subCategoryConfig = _context.ScoreSubCategoryConfig.Where(s => s.Id == obj.SubCategoryId && s.CategoryId == categoryConfig.CategoryId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                            && s.ClassId == obj.ClassId && s.TermId == obj.TermId && s.SessionId == obj.SessionId).FirstOrDefault();

                                            if (subCategoryConfig != null)
                                            {
                                                //get the student by the admissionNumber
                                                var getStudent = _context.Students.Where(s => s.AdmissionNumber == worksheet.Cells[rowCounts, 3].Value.ToString().Trim()).FirstOrDefault();

                                                //Check if the ContinousAssessmentScores scores for student has previously been uploaded
                                                ContinousAssessmentScores CAScore = _context.ContinousAssessmentScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                                && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                                && s.SubjectId == Convert.ToInt64(subId) && s.StudentId == getStudent.Id && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                                //the subject departmentId
                                                var departmentId = _context.SchoolSubjects.Where(s => s.Id == Convert.ToInt64(subId)).FirstOrDefault().DepartmentId;
                                                //the mark obtained
                                                decimal markObtained = Convert.ToDecimal(worksheet.Cells[rowCounts, count + 3].Value.ToString());

                                                if (CAScore == null)
                                                {
                                                    var caScore = new ContinousAssessmentScores
                                                    {
                                                        SchoolId = obj.SchoolId,
                                                        CampusId = obj.CampusId,
                                                        ClassId = obj.ClassId,
                                                        ClassGradeId = obj.ClassGradeId,
                                                        SessionId = obj.SessionId,
                                                        TermId = obj.TermId,
                                                        SubjectId = Convert.ToInt64(subId),
                                                        DepartmentId = departmentId,
                                                        StudentId = getStudent.Id,
                                                        AdmissionNumber = worksheet.Cells[rowCounts, 3].Value.ToString().Trim(),
                                                        MarkObtainable = subCategoryConfig.ScoreObtainable, //score obtainable from the subCategory Configured by school
                                                        MarkObtained = Convert.ToDecimal(worksheet.Cells[rowCounts, count + 3].Value.ToString()),
                                                        CategoryId = obj.CategoryId,
                                                        SubCategoryId = obj.SubCategoryId,
                                                        TeacherId = obj.TeacherId,
                                                        DateUploaded = DateTime.Now,
                                                        DateUpdated = DateTime.Now,
                                                    };

                                                    await _context.ContinousAssessmentScores.AddAsync(caScore);
                                                    await _context.SaveChangesAsync();

                                                    //Return the Scores uploaded
                                                    var newScr = (from ex in _context.ContinousAssessmentScores
                                                                  where ex.Id == caScore.Id
                                                                  select new
                                                                  {
                                                                      ex.Id,
                                                                      ex.SchoolId,
                                                                      ex.CampusId,
                                                                      ex.Classes.ClassName,
                                                                      ex.ClassGrades.GradeName,
                                                                      ex.Sessions.SessionName,
                                                                      ex.Terms.TermName,
                                                                      ex.SchoolSubjects.SubjectName,
                                                                      ex.SubjectDepartment.DepartmentName,
                                                                      StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                                                      ex.Students.AdmissionNumber,
                                                                      ex.MarkObtainable,
                                                                      ex.MarkObtained,
                                                                      ex.ScoreCategory.CategoryName,
                                                                      ex.ScoreSubCategoryConfig.SubCategoryName,
                                                                      TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                                                      ex.DateUploaded,
                                                                      ex.DateUpdated
                                                                  }).FirstOrDefault();
                                                    //list of scores uploaded
                                                    newScores.Add(newScr);

                                                    response.StatusCode = 200;
                                                    response.StatusMessage = "Scores Uploaded Successfully!";
                                                    response.ScoresUploaded = newScores.ToList();

                                                    //Update the ScoreSheetTemplate IsUsed to True
                                                    scoreSheetTemplate.IsUsed = true;
                                                    await _context.SaveChangesAsync();
                                                }
                                                else
                                                {
                                                    response.StatusCode = 409;
                                                    response.StatusMessage = "One or more Student Score Already Exits and Skipped";
                                                }
                                            }
                                            else
                                            {
                                                response.StatusCode = 409;
                                                response.StatusMessage = "Kindly Configure the Exam Score SubCategory for this Class or SubCategory With the specified ID does not Exists";
                                            }
                                        }
                                        else
                                        {
                                            response.StatusCode = 409;
                                            response.StatusMessage = "Kindly Configure the Exam Score Category for this Class!";
                                        }

                                        count++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "ScoreSheet Template with the specified ID does not exist or ScoreSheet Template has been Used!";
                        }
                    }
                    else
                    {
                        response.StatusCode = 409;
                        response.StatusMessage = "A Paremeter With Specified ID does not exist!";
                    }
                }

                return response;
            }

            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new UploadScoreRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<ExtendedScoresRespModel> getAllStudentAndSubjectScoresExtendedAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId, IList<SubjectId> subjectId)
        {
            try
            {
                IList<StudentDetails> studentData = new List<StudentDetails>();
                IList<object> SudDetails2 = new List<object>();
                long countStudent = 0;
                IList<object> data = new List<object>();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGrade = check.checkClassGradeById(classGradeId);

                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true)
                {
                    //get all students in the class and ClassGrade
                    IList<GradeStudents> allStudents = (from ex in _context.GradeStudents
                                                        where ex.ClassId == classId && ex.ClassGradeId == classGradeId
                      && ex.SchoolId == schoolId && ex.CampusId == campusId && ex.SessionId == sessionId
                                                        select ex).ToList();

                    if (allStudents.Count() > 0)
                    {
                        foreach (GradeStudents students in allStudents)
                        {
                            //creates an instance of a new list of object
                            IList<object> scoresList = new List<object>();

                            //Iterate through the subjects selected
                            foreach (SubjectId subId in subjectId)
                            {
                                if (categoryId == (long)EnumUtility.ScoreCategory.Exam)
                                {
                                    var examScore = (from ex in _context.ExaminationScores
                                                     where ex.StudentId == students.StudentId && ex.SubjectId == subId.Id && ex.SchoolId == schoolId && ex.CampusId == campusId
                                                     && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId
                                                     && ex.TermId == termId && ex.SessionId == sessionId
                                                     select new
                                                     {
                                                         ex.Id,
                                                         ex.StudentId,
                                                         StudentFullName = ex.Students.FirstName + " " + ex.Students.LastName,
                                                         ex.SubjectId,
                                                         ex.SchoolSubjects.SubjectName,
                                                         ex.MarkObtainable,
                                                         ex.MarkObtained,
                                                     }).FirstOrDefault();

                                    scoresList.Add(examScore);
                                }
                                if (categoryId == (long)EnumUtility.ScoreCategory.CA)
                                {
                                    var CAScore = (from ex in _context.ContinousAssessmentScores
                                                   where ex.StudentId == students.StudentId && ex.SubjectId == subId.Id && ex.SchoolId == schoolId && ex.CampusId == campusId
                                                   && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId
                                                   && ex.TermId == termId && ex.SessionId == sessionId
                                                   select new
                                                   {
                                                       ex.Id,
                                                       ex.StudentId,
                                                       StudentFullName = ex.Students.FirstName + " " + ex.Students.LastName,
                                                       ex.SubjectId,
                                                       ex.SchoolSubjects.SubjectName,
                                                       ex.MarkObtainable,
                                                       ex.MarkObtained,
                                                   }).FirstOrDefault();

                                    scoresList.Add(CAScore);
                                }
                            }

                            //get the student 
                            var getStudent = _context.Students.Where(x => x.Id == students.StudentId).FirstOrDefault();

                            //The student object to be returned 
                            StudentDetails std = new StudentDetails();
                            std.StudentFullName = getStudent.FirstName + " " + getStudent.LastName;
                            std.AdmissionNumber = getStudent.AdmissionNumber;
                            std.SubjectDetails = scoresList.ToList();

                            //Student Details List
                            studentData.Add(std);

                            //Count the numbers of students
                            countStudent++;
                        }

                        return new ExtendedScoresRespModel { StatusCode = 200, StatusMessage = "Success", Data = studentData };

                    }

                    return new ExtendedScoresRespModel { StatusCode = 409, StatusMessage = "No Student in this Class" };
                }

                return new ExtendedScoresRespModel { StatusCode = 409, StatusMessage = "One of the Supplied Parameters is Invalid" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new ExtendedScoresRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<ScoreUploadSheetRespModel> createScoreUploadSheetTemplateAsync(ScoreUploadSheetTemplateReqModel obj)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    StringBuilder sb = new StringBuilder();
                    long countSubjects = 0;

                    //the list of selected subjects
                    IList<SchoolSubjects> subjectList = new List<SchoolSubjects>();

                    foreach (SubjectId subjId in obj.SubjectId)
                    {
                        //validate if the subject exists
                        var subject = _context.SchoolSubjects.Where(s => s.Id == subjId.Id && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).AsNoTracking().FirstOrDefault();
                        if (subject != null)
                        {
                            sb.Append(subjId.Id.ToString() + ",");
                            subjectList.Add(subject);

                            countSubjects++;
                        }
                    }

                    //all the subjectIds, removing the last comma seperating them ','
                    string allSubjectId = string.Empty;
                    allSubjectId = sb.ToString().Remove(sb.ToString().Length - 1);

                    //get the Class and ClassGrade
                    string gradeName = _context.ClassGrades.Where(s => s.Id == obj.ClassGradeId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault().GradeName;
                    string className = _context.Classes.Where(s => s.Id == obj.ClassId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault().ClassName;

                    //remove space from description
                    string description = obj.Description.Replace(" ", "_");

                    //Generate a FileName
                    string FileName1 = className + gradeName + allSubjectId + description + ".xls";
                    string FileName = FileName1.Replace(",", "_");

                    //checks if a Template has been created with the description
                    var checkResult = _context.ScoreUploadSheetTemplates.Where(x => x.Description == obj.Description && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();
                    if (checkResult == null)
                    {

                        var scoreTemp = new ScoreUploadSheetTemplates
                        {
                            SchoolId = obj.SchoolId,
                            CampusId = obj.CampusId,
                            ClassId = obj.ClassId,
                            ClassGradeId = obj.ClassGradeId,
                            TeacherId = obj.TeacherId,
                            Description = obj.Description,
                            SubjectId = allSubjectId,
                            TotalNumberOfSubjects = countSubjects,
                            IsUsed = false,
                            DateGenerated = DateTime.Now,
                        };

                        await _context.ScoreUploadSheetTemplates.AddAsync(scoreTemp);
                        await _context.SaveChangesAsync();

                        //Return the Scores Sheet Template Created
                        var temp = from ex in _context.ScoreUploadSheetTemplates
                                   where ex.SchoolId == obj.SchoolId && ex.CampusId == obj.CampusId
                                   && ex.ClassId == obj.ClassId && ex.ClassGradeId == obj.ClassGradeId
                                   && ex.TeacherId == obj.TeacherId && ex.Description == obj.Description
                                   select new
                                   {
                                       ex.Id,
                                       ex.SchoolId,
                                       ex.CampusId,
                                       ex.Classes.ClassName,
                                       ex.ClassGrades.GradeName,
                                       ex.Description,
                                       ex.SubjectId,
                                       ex.TotalNumberOfSubjects,
                                       ex.TeacherId,
                                       TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                       ex.IsUsed,
                                       ex.DateGenerated
                                   };


                        //get all the students in the class and classGrade
                        var studentsInClass = from ex in _context.GradeStudents
                                              where ex.SchoolId == obj.SchoolId && ex.CampusId == obj.CampusId
                                              && ex.ClassId == obj.ClassId && ex.ClassGradeId == obj.ClassGradeId
                                              select new
                                              {
                                                  ex.Id,
                                                  ex.SchoolId,
                                                  ex.CampusId,
                                                  ex.Students.FirstName,
                                                  ex.Students.LastName,
                                                  ex.Students.UserName,
                                                  ex.Students.AdmissionNumber,
                                                  ex.Students.hasParent,
                                                  ex.Students.IsActive,
                                                  ex.ClassId,
                                                  ex.Classes.ClassName,
                                                  ex.ClassGradeId,
                                                  ex.ClassGrades.GradeName,
                                                  ex.DateCreated,
                                              };

                        return new ScoreUploadSheetRespModel { StatusCode = 200, StatusMessage = "ScoreUpload Sheet Generated Successfully", FileName = FileName, ScoreSheetTemplate = temp.FirstOrDefault(), Students = studentsInClass, Subjects = subjectList };

                    }
                    else
                    {
                        return new ScoreUploadSheetRespModel { StatusCode = 409, StatusMessage = "A Template with this Description Already Exists" };
                    }
                }
                else
                {
                    return new ScoreUploadSheetRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new ScoreUploadSheetRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getScoreSheetTemplateByIdAsync(long scoreSheetTemplateId)
        {
            try
            {
                var result = from ex in _context.ScoreUploadSheetTemplates
                             where ex.Id == scoreSheetTemplateId
                             select new
                             {
                                 ex.Id,
                                 ex.SchoolId,
                                 ex.CampusId,
                                 ex.Classes.ClassName,
                                 ex.ClassGrades.GradeName,
                                 ex.Description,
                                 ex.SubjectId,
                                 ex.TotalNumberOfSubjects,
                                 TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                 ex.IsUsed,
                                 ex.DateGenerated
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAllUsedScoreSheetTemplateAsync(long schoolId, long campusId, long classId, long classGradeId, Guid teacherId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    var result = from ex in _context.ScoreUploadSheetTemplates
                                 where ex.SchoolId == schoolId && ex.CampusId == campusId && ex.ClassId == classId
                                 && ex.ClassGradeId == classGradeId && ex.TeacherId == teacherId && ex.IsUsed == true
                                 select new
                                 {
                                     ex.Id,
                                     ex.SchoolId,
                                     ex.CampusId,
                                     ex.Classes.ClassName,
                                     ex.ClassGrades.GradeName,
                                     ex.Description,
                                     ex.SubjectId,
                                     ex.TotalNumberOfSubjects,
                                     TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                     ex.IsUsed,
                                     ex.DateGenerated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAllUnUsedScoreSheetTemplateAsync(long schoolId, long campusId, long classId, long classGradeId, Guid teacherId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    var result = from ex in _context.ScoreUploadSheetTemplates
                                 where ex.SchoolId == schoolId && ex.CampusId == campusId && ex.ClassId == classId
                                 && ex.ClassGradeId == classGradeId && ex.TeacherId == teacherId && ex.IsUsed == false
                                 select new
                                 {
                                     ex.Id,
                                     ex.SchoolId,
                                     ex.CampusId,
                                     ex.Classes.ClassName,
                                     ex.ClassGrades.GradeName,
                                     ex.Description,
                                     ex.SubjectId,
                                     ex.TotalNumberOfSubjects,
                                     TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                     ex.IsUsed,
                                     ex.DateGenerated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        public async Task<GenericRespModel> studentGradeBookScoresPerSubjectAndCategoryAsync(Guid studentId, long schoolId, long campusId, long categoryId, long subCategoryId)
        {
            try
            {
                //Session and Term Validations
                SessionAndTerm currentSessionTerm = new SessionAndTerm(_context);
                long currentSessionId = currentSessionTerm.getCurrentSessionId(schoolId);
                long currentTermId = currentSessionTerm.getCurrentTermId(schoolId);

                //Check if the current Term and Session has been set
                if (currentSessionId > 0 && currentTermId > 0)
                {
                    //check if the student is assigned to a class for the current session
                    var studentGrade = _context.GradeStudents.Where(x => x.StudentId == studentId && x.SessionId == currentSessionId).FirstOrDefault();
                    if (studentGrade != null)
                    {
                        long classId = studentGrade.ClassId; //the classId
                        long classGradeId = studentGrade.ClassGradeId; //the classGradeId

                        //if the category is Examination
                        if (categoryId == (long)EnumUtility.ScoreCategory.Exam)
                        {
                            var result = from ex in _context.ExaminationScores
                                         where ex.StudentId == studentId && ex.CategoryId == (long)EnumUtility.ScoreCategory.Exam && ex.SubCategoryId == subCategoryId && ex.ClassId == classId
                                         && ex.ClassGradeId == classGradeId && ex.TermId == currentTermId && ex.SessionId == currentSessionId
                                         select new
                                         {
                                             ex.Id,
                                             ex.SchoolId,
                                             ex.CampusId,
                                             ex.Classes.ClassName,
                                             ex.ClassGrades.GradeName,
                                             ex.Sessions.SessionName,
                                             ex.Terms.TermName,
                                             ex.SchoolSubjects.SubjectName,
                                             ex.SubjectDepartment.DepartmentName,
                                             StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                             ex.Students.AdmissionNumber,
                                             ex.MarkObtainable,
                                             ex.MarkObtained,
                                             ex.ScoreCategory.CategoryName,
                                             ex.ScoreSubCategoryConfig.SubCategoryName,
                                             TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                             ex.DateUploaded,
                                             ex.DateUpdated
                                         };

                            if (result.Count() > 0)
                            {
                                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                            }

                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                        }
                        //if the category is ContinousAssessment
                        else if (categoryId == (long)EnumUtility.ScoreCategory.CA)
                        {
                            var result = from ex in _context.ContinousAssessmentScores
                                         where ex.StudentId == studentId && ex.CategoryId == (long)EnumUtility.ScoreCategory.CA && ex.SubCategoryId == subCategoryId && ex.ClassId == classId
                                         && ex.ClassGradeId == classGradeId && ex.TermId == currentTermId && ex.SessionId == currentSessionId
                                         select new
                                         {
                                             ex.Id,
                                             ex.SchoolId,
                                             ex.CampusId,
                                             ex.Classes.ClassName,
                                             ex.ClassGrades.GradeName,
                                             ex.Sessions.SessionName,
                                             ex.Terms.TermName,
                                             ex.SchoolSubjects.SubjectName,
                                             ex.SubjectDepartment.DepartmentName,
                                             StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                             ex.Students.AdmissionNumber,
                                             ex.MarkObtainable,
                                             ex.MarkObtained,
                                             ex.ScoreCategory.CategoryName,
                                             ex.ScoreSubCategoryConfig.SubCategoryName,
                                             TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                             ex.DateUploaded,
                                             ex.DateUpdated
                                         };

                            if (result.Count() > 0)
                            {
                                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                            }

                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                        }
                        //if the category is Behavioural
                        else if (categoryId == (long)EnumUtility.ScoreCategory.Behavioural)
                        {
                            var result = from ex in _context.BehavioralScores
                                         where ex.StudentId == studentId && ex.CategoryId == (long)EnumUtility.ScoreCategory.Behavioural && ex.SubCategoryId == subCategoryId && ex.ClassId == classId
                                         && ex.ClassGradeId == classGradeId && ex.TermId == currentTermId && ex.SessionId == currentSessionId
                                         select new
                                         {
                                             ex.Id,
                                             ex.SchoolId,
                                             ex.CampusId,
                                             ex.Classes.ClassName,
                                             ex.ClassGrades.GradeName,
                                             ex.Sessions.SessionName,
                                             ex.Terms.TermName,
                                             StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                             ex.Students.AdmissionNumber,
                                             ex.MarkObtainable,
                                             ex.MarkObtained,
                                             ex.ScoreCategory.CategoryName,
                                             ex.ScoreSubCategoryConfig.SubCategoryName,
                                             TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                             ex.DateUploaded,
                                             ex.DateUpdated
                                         };

                            if (result.Count() > 0)
                            {
                                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                            }

                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                        }
                        //if the category is ExtraCurricular
                        else if (categoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
                        {
                            var result = from ex in _context.ExtraCurricularScores
                                         where ex.StudentId == studentId && ex.CategoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular && ex.SubCategoryId == subCategoryId && ex.ClassId == classId
                                         && ex.ClassGradeId == classGradeId && ex.TermId == currentTermId && ex.SessionId == currentSessionId
                                         select new
                                         {
                                             ex.Id,
                                             ex.SchoolId,
                                             ex.CampusId,
                                             ex.Classes.ClassName,
                                             ex.ClassGrades.GradeName,
                                             ex.Sessions.SessionName,
                                             ex.Terms.TermName,
                                             StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                             ex.Students.AdmissionNumber,
                                             ex.MarkObtainable,
                                             ex.MarkObtained,
                                             ex.ScoreCategory.CategoryName,
                                             ex.ScoreSubCategoryConfig.SubCategoryName,
                                             TeachersName = ex.SchoolUsers.FirstName + " " + ex.SchoolUsers.LastName,
                                             ex.DateUploaded,
                                             ex.DateUpdated
                                         };

                            if (result.Count() > 0)
                            {
                                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                            }

                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                        }
                        else
                        {
                            return new GenericRespModel { StatusCode = 409, StatusMessage = "No Category/SubCategory With the Specified ID", };
                        }

                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Student does not belong to a Class!" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Current Academic Session and Term has not been Set!" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
    }
}