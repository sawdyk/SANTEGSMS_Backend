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
    public class ExtraCurricularBehavioralScoreRepo : IExtraCurricularBehavioralScoresRepo
    {
        private readonly AppDbContext _context;
        public ExtraCurricularBehavioralScoreRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UploadScoreRespModel> uploadExtraCurricularBehavioralScoresAsync(UploadScoreReqModel obj)
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
                    //--------------------------------------------------------------------EXTRACURRICULAR SCORES---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
                    {
                        //Using Category Configuration
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
                                    //Check if the ExtraCurricular scores for student has previously been uploaded
                                    ExtraCurricularScores extraScore = _context.ExtraCurricularScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                    && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                    && s.StudentId == scr.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                    //the Student AdmissionNumber
                                    var admissionNumber = _context.Students.Where(s => s.Id == scr.StudentId).FirstOrDefault().AdmissionNumber;

                                    if (extraScore == null)
                                    {
                                        var extScr = new ExtraCurricularScores
                                        {
                                            SchoolId = obj.SchoolId,
                                            CampusId = obj.CampusId,
                                            ClassId = obj.ClassId,
                                            ClassGradeId = obj.ClassGradeId,
                                            SessionId = obj.SessionId,
                                            TermId = obj.TermId,
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

                                        await _context.ExtraCurricularScores.AddAsync(extScr);
                                        await _context.SaveChangesAsync();

                                        //Return the Scores uploaded
                                        var newScr = (from ex in _context.ExtraCurricularScores
                                                      where ex.Id == extScr.Id
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
                                response.StatusMessage = "Kindly Configure the ExtraCurricular Score SubCategory for this Class or SubCategory with the specified ID does not exists";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the ExtraCurricular Score Category for this Class!";
                        }
                    }

                    //--------------------------------------------------------------------BEHAVIOURAL SCORES---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.Behavioural)
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
                                    //Check if the Behavioral scores for student has previously been uploaded
                                    BehavioralScores behvScore = _context.BehavioralScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                    && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                    && s.StudentId == scr.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                    //the Student AdmissionNumber
                                    var admissionNumber = _context.Students.Where(s => s.Id == scr.StudentId).FirstOrDefault().AdmissionNumber;

                                    if (behvScore == null)
                                    {
                                        var score = new BehavioralScores
                                        {
                                            SchoolId = obj.SchoolId,
                                            CampusId = obj.CampusId,
                                            ClassId = obj.ClassId,
                                            ClassGradeId = obj.ClassGradeId,
                                            SessionId = obj.SessionId,
                                            TermId = obj.TermId,
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

                                        await _context.BehavioralScores.AddAsync(score);
                                        await _context.SaveChangesAsync();

                                        //Return the Scores uploaded
                                        var newScr = (from ex in _context.BehavioralScores
                                                      where ex.Id == score.Id
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
                                response.StatusMessage = "Kindly Configure the Behavioral Score SubCategory for this Class or SubCategory with the specified ID does not exists";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Behavioral Score Category for this Class!";
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

        public async Task<UploadScoreRespModel> uploadSingleStudentExtraCurricularBehavioralScoreAsync(UploadScorePerStudentReqModel obj)
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
                    //--------------------------------------------------------------------EXTRACURRICULAR SCORES---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
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
                                //Check if the ExtraCurricular scores for student has previously been uploaded
                                ExtraCurricularScores extraScore = _context.ExtraCurricularScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                && s.StudentId == obj.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                //the Student AdmissionNumber
                                var admissionNumber = _context.Students.Where(s => s.Id == obj.StudentId).FirstOrDefault().AdmissionNumber;

                                //if Exam scores does not exit, Save the new Exam scores
                                if (extraScore == null)
                                {
                                    var exScore = new ExtraCurricularScores
                                    {
                                        SchoolId = obj.SchoolId,
                                        CampusId = obj.CampusId,
                                        ClassId = obj.ClassId,
                                        ClassGradeId = obj.ClassGradeId,
                                        SessionId = obj.SessionId,
                                        TermId = obj.TermId,
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

                                    await _context.ExtraCurricularScores.AddAsync(exScore);
                                    await _context.SaveChangesAsync();

                                    //Return the Scores uploaded
                                    var newScr = (from ex in _context.ExtraCurricularScores
                                                  where ex.Id == exScore.Id
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
                                    response.StatusMessage = "Student Score Already Exits!";
                                }
                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "Kindly Configure the Extracurricular Score SubCategory for this Class or SubCategory with the specified ID does not exists";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Extracurricular Score Category for this Class!";
                        }
                    }

                    //--------------------------------------------------------------------BEHAVIOURAL SCORES---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.Behavioural)
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
                                //Check if the Behavioral scores for student has previously been uploaded
                                BehavioralScores behvScore = _context.BehavioralScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                && s.StudentId == obj.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                //the Student AdmissionNumber
                                var admissionNumber = _context.Students.Where(s => s.Id == obj.StudentId).FirstOrDefault().AdmissionNumber;

                                //if CA scores does not exit, Save the new CA scores
                                if (behvScore == null)
                                {
                                    var score = new BehavioralScores
                                    {
                                        SchoolId = obj.SchoolId,
                                        CampusId = obj.CampusId,
                                        ClassId = obj.ClassId,
                                        ClassGradeId = obj.ClassGradeId,
                                        SessionId = obj.SessionId,
                                        TermId = obj.TermId,
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

                                    await _context.BehavioralScores.AddAsync(score);
                                    await _context.SaveChangesAsync();

                                    //Return the Scores uploaded
                                    var newScr = (from ex in _context.BehavioralScores
                                                  where ex.Id == score.Id
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
                                response.StatusMessage = "Kindly Configure the Behavioural Score SubCategory for this Class or SubCategory does not exists!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Behavioural Score Category for this Class!";
                        }
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "A Parameter with the specified ID does not Exist!";
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

        public async Task<UploadScoreRespModel> updateExtraCurricularBehavioralScoresAsync(UploadScoreReqModel obj)
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
                    //--------------------------------------------------------------------EXTRACURRICULAR SCORES---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
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
                                    ExtraCurricularScores extScore = _context.ExtraCurricularScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                    && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                    && s.StudentId == scr.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                    //the Student AdmissionNumber
                                    var admissionNumber = _context.Students.Where(s => s.Id == scr.StudentId).FirstOrDefault().AdmissionNumber;

                                    if (extScore != null)
                                    {
                                        extScore.SchoolId = obj.SchoolId;
                                        extScore.CampusId = obj.CampusId;
                                        extScore.ClassId = obj.ClassId;
                                        extScore.ClassGradeId = obj.ClassGradeId;
                                        extScore.SessionId = obj.SessionId;
                                        extScore.TermId = obj.TermId;
                                        extScore.StudentId = scr.StudentId;
                                        extScore.AdmissionNumber = admissionNumber;
                                        extScore.MarkObtainable = subCategoryConfig.ScoreObtainable; //score obtainable from the subCategory Configured by school
                                        extScore.MarkObtained = scr.MarkObtained;
                                        extScore.CategoryId = obj.CategoryId;
                                        extScore.SubCategoryId = obj.SubCategoryId;
                                        extScore.TeacherId = obj.TeacherId;
                                        extScore.DateUpdated = DateTime.Now;

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
                                response.StatusMessage = "Kindly Configure the Extracurricular Score SubCategory for this Class!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Extracurricular Score Category for this Class!";
                        }
                    }

                    //--------------------------------------------------------------------BEHAVIOURAL SCORES---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.Behavioural)
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
                                    BehavioralScores behvScore = _context.BehavioralScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                    && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                    && s.StudentId == scr.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                    //the Student AdmissionNumber
                                    var admissionNumber = _context.Students.Where(s => s.Id == scr.StudentId).FirstOrDefault().AdmissionNumber;

                                    if (behvScore != null)
                                    {
                                        behvScore.SchoolId = obj.SchoolId;
                                        behvScore.CampusId = obj.CampusId;
                                        behvScore.ClassId = obj.ClassId;
                                        behvScore.ClassGradeId = obj.ClassGradeId;
                                        behvScore.SessionId = obj.SessionId;
                                        behvScore.TermId = obj.TermId;
                                        behvScore.StudentId = scr.StudentId;
                                        behvScore.AdmissionNumber = admissionNumber;
                                        behvScore.MarkObtainable = subCategoryConfig.ScoreObtainable; //score obtainable from the subCategory Configured by school
                                        behvScore.MarkObtained = scr.MarkObtained;
                                        behvScore.CategoryId = obj.CategoryId;
                                        behvScore.SubCategoryId = obj.SubCategoryId;
                                        behvScore.TeacherId = obj.TeacherId;
                                        behvScore.DateUpdated = DateTime.Now;

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
                                response.StatusMessage = "Kindly Configure the Behavioural Score SubCategory for this Class or SubCategory does not exists!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Behavioural Score Category for this Class!";
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

        public async Task<GenericRespModel> getExtraCurricularBehavioralScoresAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
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
                    if (categoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
                    {
                        var result = from ex in _context.ExtraCurricularScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                      && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                      && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId
                                     select new
                                     {
                                         ex.Id,
                                         ex.StudentId,
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
                    else if (categoryId == (long)EnumUtility.ScoreCategory.Behavioural)
                    {
                        var result = from ex in _context.BehavioralScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                       && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                       && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId
                                     select new
                                     {
                                         ex.Id,
                                         ex.StudentId,
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

        public async Task<GenericRespModel> getExtraCurricularBehavioralScoresByStudentIdAndCategoryIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
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
                    if (categoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
                    {
                        var result = from ex in _context.ExtraCurricularScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                      && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                      && ex.CategoryId == categoryId && ex.StudentId == studentId
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
                    else if (categoryId == (long)EnumUtility.ScoreCategory.Behavioural)
                    {
                        var result = from ex in _context.BehavioralScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                       && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                       && ex.CategoryId == categoryId && ex.StudentId == studentId
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

        public async Task<GenericRespModel> getExtraCurricularBehavioralScoresByStudentIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
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
                    if (categoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
                    {
                        var result = from ex in _context.ExtraCurricularScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                      && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                      && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId && ex.StudentId == studentId
                                     select new
                                     {
                                         ex.Id,
                                         ex.SchoolId,
                                         ex.CampusId,
                                         ex.ClassId,
                                         ex.ClassGradeId,
                                         ex.CategoryId,
                                         ex.SubCategoryId,
                                         ex.TermId,
                                         ex.SessionId,
                                         ex.Classes.ClassName,
                                         ex.ClassGrades.GradeName,
                                         ex.Sessions.SessionName,
                                         ex.Terms.TermName,
                                         ex.StudentId,
                                         StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                         ex.Students.AdmissionNumber,
                                         ex.MarkObtainable,
                                         ex.MarkObtained,
                                         ex.ScoreCategory.CategoryName,
                                         ex.ScoreSubCategoryConfig.SubCategoryName,
                                         ex.TeacherId,
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
                    else if (categoryId == (long)EnumUtility.ScoreCategory.Behavioural)
                    {
                        var result = from ex in _context.BehavioralScores
                                     where ex.SchoolId == schoolId && ex.CampusId == campusId
                                       && ex.ClassId == classId && ex.ClassGradeId == classGradeId && ex.TermId == termId && ex.SessionId == sessionId
                                       && ex.CategoryId == categoryId && ex.SubCategoryId == subCategoryId && ex.StudentId == studentId
                                     select new
                                     {
                                         ex.Id,
                                         ex.SchoolId,
                                         ex.CampusId,
                                         ex.ClassId,
                                         ex.ClassGradeId,
                                         ex.CategoryId,
                                         ex.SubCategoryId,
                                         ex.TermId,
                                         ex.SessionId,
                                         ex.Classes.ClassName,
                                         ex.ClassGrades.GradeName,
                                         ex.Sessions.SessionName,
                                         ex.Terms.TermName,
                                         ex.StudentId,
                                         StudentName = ex.Students.FirstName + " " + ex.Students.LastName,
                                         ex.Students.AdmissionNumber,
                                         ex.MarkObtainable,
                                         ex.MarkObtained,
                                         ex.ScoreCategory.CategoryName,
                                         ex.ScoreSubCategoryConfig.SubCategoryName,
                                         ex.TeacherId,
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

        public async Task<GenericRespModel> deleteExtraCurricularBehavioralScoresForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            try
            {
                if (categoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
                {
                    //Check if the ExtraCurricularScores scores for student Exists
                    IList<ExtraCurricularScores> extrScore = (_context.ExtraCurricularScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId).ToList());

                    if (extrScore.Count > 0)
                    {
                        _context.ExtraCurricularScores.RemoveRange(extrScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };

                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }
                else if (categoryId == (long)EnumUtility.ScoreCategory.Behavioural)
                {
                    //Check if the BehavioralScores scores for student Exists
                    IList<BehavioralScores> behvScore = (_context.BehavioralScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId).ToList());

                    if (behvScore.Count() > 0)
                    {
                        _context.BehavioralScores.RemoveRange(behvScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category(Extracurricular/Behavioural)", };
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

        public async Task<GenericRespModel> deleteExtraCurricularBehavioralScoresForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            try
            {
                if (categoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
                {
                    //Check if the ExtraCurricularScores scores for student Exists
                    IList<ExtraCurricularScores> extrScore = (_context.ExtraCurricularScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId && s.StudentId == studentId).ToList());

                    if (extrScore.Count() > 0)
                    {
                        _context.ExtraCurricularScores.RemoveRange(extrScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };

                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }
                else if (categoryId == (long)EnumUtility.ScoreCategory.Behavioural)
                {
                    //Check if the BehavioralScores scores for student Exists
                    IList<BehavioralScores> behvScore = (_context.BehavioralScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.CategoryId == categoryId && s.SubCategoryId == subCategoryId && s.StudentId == studentId).ToList());

                    if (behvScore.Count > 0)
                    {
                        _context.BehavioralScores.RemoveRange(behvScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category(Extracurricular/Behavioural)", };
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

        public async Task<GenericRespModel> deleteExtraCurricularBehavioralScoresPerCategoryForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            try
            {
                if (categoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
                {
                    //Check if the ExtraCurricularScores scores for student Exists
                    IList<ExtraCurricularScores> extrScore = (_context.ExtraCurricularScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.CategoryId == categoryId).ToList());

                    if (extrScore.Count() > 0)
                    {
                        _context.ExtraCurricularScores.RemoveRange(extrScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }
                else if (categoryId == (long)EnumUtility.ScoreCategory.Behavioural)
                {
                    //Check if the BehavioralScores scores for student Exists
                    IList<BehavioralScores> behvScore = (_context.BehavioralScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.CategoryId == categoryId).ToList());

                    if (behvScore.Count > 0)
                    {
                        _context.BehavioralScores.RemoveRange(behvScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };

                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category(Extracurricular/Behavioural)", };
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

        public async Task<GenericRespModel> deleteExtraCurricularBehavioralScoresPerCategoryForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            try
            {
                if (categoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
                {
                    //Check if the ExtraCurricularScores scores for student Exists
                    IList<ExtraCurricularScores> extrScore = (_context.ExtraCurricularScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.CategoryId == categoryId && s.StudentId == studentId).ToList());

                    if (extrScore.Count() > 0)
                    {
                        _context.ExtraCurricularScores.RemoveRange(extrScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }
                else if (categoryId == (long)EnumUtility.ScoreCategory.Behavioural)
                {
                    //Check if the BehavioralScores scores for student Exists
                    IList<BehavioralScores> behvScore = (_context.BehavioralScores.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId
                    && s.CategoryId == categoryId && s.StudentId == studentId).ToList());

                    if (behvScore.Count() > 0)
                    {
                        _context.BehavioralScores.RemoveRange(behvScore);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Scores Deleted Successfully", };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Kindly Select the Category(Extracurricular/Behavioural)", };
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

        public async Task<UploadScoreRespModel> updateSingleStudentExtraCurricularBehavioralScoresAsync(UploadSingleStudentScoreReqModel obj)
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
                    //--------------------------------------------------------------------EXTRACURRICULAR SCORES---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.ExtraCurricular)
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
                                ExtraCurricularScores extScore = _context.ExtraCurricularScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                && s.StudentId == obj.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                //the Student AdmissionNumber
                                var admissionNumber = _context.Students.Where(s => s.Id == obj.StudentId).FirstOrDefault().AdmissionNumber;

                                if (extScore != null)
                                {
                                    extScore.SchoolId = obj.SchoolId;
                                    extScore.CampusId = obj.CampusId;
                                    extScore.ClassId = obj.ClassId;
                                    extScore.ClassGradeId = obj.ClassGradeId;
                                    extScore.SessionId = obj.SessionId;
                                    extScore.TermId = obj.TermId;
                                    extScore.StudentId = obj.StudentId;
                                    extScore.AdmissionNumber = admissionNumber;
                                    extScore.MarkObtainable = subCategoryConfig.ScoreObtainable; //score obtainable from the subCategory Configured by school
                                    extScore.MarkObtained = obj.MarkObtained;
                                    extScore.CategoryId = obj.CategoryId;
                                    extScore.SubCategoryId = obj.SubCategoryId;
                                    extScore.TeacherId = obj.TeacherId;
                                    extScore.DateUpdated = DateTime.Now;

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
                                response.StatusMessage = "Kindly Configure the Extracurricular Score SubCategory for this Class!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Extracurricular Score Category for this Class!";
                        }
                    }

                    //--------------------------------------------------------------------BEHAVIOURAL SCORES---------------------------------------------------------------------------------------------

                    if (obj.CategoryId == (long)EnumUtility.ScoreCategory.Behavioural)
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
                                BehavioralScores behvScore = _context.BehavioralScores.Where(s => s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId
                                && s.ClassId == obj.ClassId && s.ClassGradeId == obj.ClassGradeId && s.TermId == obj.TermId && s.SessionId == obj.SessionId
                                && s.StudentId == obj.StudentId && s.CategoryId == obj.CategoryId && s.SubCategoryId == obj.SubCategoryId).FirstOrDefault();

                                //the Student AdmissionNumber
                                var admissionNumber = _context.Students.Where(s => s.Id == obj.StudentId).FirstOrDefault().AdmissionNumber;

                                if (behvScore != null)
                                {
                                    behvScore.SchoolId = obj.SchoolId;
                                    behvScore.CampusId = obj.CampusId;
                                    behvScore.ClassId = obj.ClassId;
                                    behvScore.ClassGradeId = obj.ClassGradeId;
                                    behvScore.SessionId = obj.SessionId;
                                    behvScore.TermId = obj.TermId;
                                    behvScore.StudentId = obj.StudentId;
                                    behvScore.AdmissionNumber = admissionNumber;
                                    behvScore.MarkObtainable = subCategoryConfig.ScoreObtainable; //score obtainable from the subCategory Configured by school
                                    behvScore.MarkObtained = obj.MarkObtained;
                                    behvScore.CategoryId = obj.CategoryId;
                                    behvScore.SubCategoryId = obj.SubCategoryId;
                                    behvScore.TeacherId = obj.TeacherId;
                                    behvScore.DateUpdated = DateTime.Now;

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
                                response.StatusMessage = "Kindly Configure the Behavioural Score SubCategory for this Class or SubCategory does not exists!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Kindly Configure the Behavioural Score Category for this Class!";
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
    }
}
