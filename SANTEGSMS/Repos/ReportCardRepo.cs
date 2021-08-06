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
using System.Net;
using System.Threading.Tasks;

namespace SANTEGSMS.Repos
{
    public class ReportCardRepo : IReportCardRepo
    {
        private readonly AppDbContext _context;
        private readonly ReportCardReUsables reportCardReUsables;
        private readonly IWebHostEnvironment env;

        public ReportCardRepo(AppDbContext context, ReportCardReUsables reportCardReUsables)
        {
            _context = context;
            this.reportCardReUsables = reportCardReUsables;
        }

        //---------------SAMPLE ALGORITHM TO CALCULATE STUDENTS POSITION BASED ON FINAL SCORE ACCUMULATED --------------------------------------

        public async Task<GenericRespModel> getScorePositionAsync()
        {
            IList<PositionResponse> posList = new List<PositionResponse>();

            //array of integer values
            long[] scores = { 10, 20, 30, 45, 60, 20, 15, 20, 15, 10, 8, 10, 2, 10 };

            //count the number of items in the array
            long scoreCount = scores.Count();

            //sort the values in the array in descending order and convert to array of integer values
            long[] descending = scores.OrderByDescending(s => s).ToArray();

            //A dictionary object for keeping the keys and their values
            Dictionary<long, long> dic = new Dictionary<long, long>();

            //Variable declarations and intantiations
            long count = 1;
            long scorePosition = 1;
            long val = 0;

            //Iteration performed on the scores array
            for (int i = 0; i < scoreCount; i++)
            {
                //An Instance of PositionResponse Class
                PositionResponse pos = new PositionResponse();
                pos.Score = descending[i]; //Assign each score values to score property of PositionResponse Class
                pos.Position = scorePosition; //Assign each scoreposition values to position property of PositionResponse Class

                //Check if the dictionary does not contain an existing score
                if (dic.ContainsValue(descending[i]))
                {
                    //if dictionary contains an existing score, deduct the count from current score position
                    val = scorePosition - count;
                    pos.Position = val; //Assign the value to position property of PositionResponse Class
                    count++;
                }
                else //if the dictionary does not contain an existing score, reInitialize count to 1
                {
                    count = 1;
                }

                //Add the Key and Value to dictionary (Index of the score array as the Key and scores ordered by descending as the value)
                dic.Add(i, descending[i]);

                //Add the PositionResponse Class to a List of PositionResponse Class
                posList.Add(pos);

                scorePosition++;
            }

            return new GenericRespModel { StatusCode = 200, StatusMessage = "Computed Successfully!", Data = posList };
        }

        //-------------------------------------------COMPUTE STUDENT SCORE (EXAM AND CA) -----------------------------------------------------------------------------------
        public async Task<ComputeResultRespModel> computeResultAndSubjectPositionAsync(ComputeResultPositionReqModel obj)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);
                var checkTerm = check.checkTermById(obj.TermId);
                var checkSession = check.checkSessionById(obj.SessionId);

                ReportCardData report = new ReportCardData();
                decimal examFinalScore = 0;
                decimal CAFinalScore = 0;
                decimal totalScore = 0;

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true && checkTerm == true && checkSession == true)
                {
                    //get the report Card Configuration 
                    ReportCardConfiguration rptConfig = _context.ReportCardConfiguration.Where(rp => rp.SchoolId == obj.SchoolId && rp.CampusId == obj.CampusId && rp.TermId == obj.TermId).FirstOrDefault();   //selects all the courses in the cart

                    //all students in the class and classGrade
                    IList<GradeStudents> gradeStudent = (from ex in _context.GradeStudents
                                                         where ex.SchoolId == obj.SchoolId && ex.CampusId == obj.CampusId && ex.ClassId == obj.ClassId
                                                         && ex.ClassGradeId == obj.ClassGradeId && ex.SessionId == obj.SessionId
                                                         select ex).ToList();
                    if (rptConfig != null)
                    {
                        //categoryConfig
                        var categoryConfig = from sc in _context.ScoreCategoryConfig
                                             where sc.SchoolId == obj.SchoolId && sc.CampusId == obj.CampusId
                                            && sc.ClassId == obj.ClassId && sc.SessionId == obj.SessionId && sc.TermId == obj.TermId
                                             select sc;

                        if (categoryConfig.Count() > 0)
                        {
                            //subCategoryConfig
                            var subCategoryConfig = from sc in _context.ScoreSubCategoryConfig
                                                    where sc.SchoolId == obj.SchoolId && sc.CampusId == obj.CampusId
                                                    && sc.ClassId == obj.ClassId && sc.SessionId == obj.SessionId && sc.TermId == obj.TermId
                                                    select sc;

                            if (subCategoryConfig.Count() > 0)
                            {

                                foreach (SubjectId subject in obj.SubjectIds)
                                {
                                    //check the subject
                                    SchoolSubjects schSubj = _context.SchoolSubjects.Where(s => s.Id == subject.Id && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();

                                    if (schSubj != null)
                                    {
                                        //check if there are students in the class
                                        if (gradeStudent.Count() > 0)
                                        {
                                            foreach (GradeStudents std in gradeStudent)
                                            {
                                                //first, second and third term
                                                decimal totalCAScoreObtained = 0;
                                                decimal CAScoreObtained = 0;
                                                decimal totalExamScoreObtained = 0;
                                                decimal examScoreObtained = 0;
                                                decimal CA_CumulativeScore = 0;

                                                string gradeLetter = string.Empty;
                                                string gradeRemark = string.Empty;

                                                //third term
                                                decimal firstTermTotalScore = 0;
                                                decimal secondTermTotalScore = 0;
                                                decimal averageTotalScore = 0;


                                                //----get the student AdmissionNumber
                                                string admissionNumber = _context.Students.Where(s => s.Id == std.StudentId).FirstOrDefault().AdmissionNumber;



                                                ///////////////////////////////////////////////////////////______________FIRST_TERM______________________///////////////////////////////////////////////////////////////////////////////////


                                                if (obj.TermId == (int)EnumUtility.Terms.FirstTerm)
                                                {
                                                    //****************************************EXAMINATION******************************************************************

                                                    //get all the examination scores and add up the markObtained by each student for each subject
                                                    IList<ExaminationScores> exScore = reportCardReUsables.getExaminationScores(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, admissionNumber, obj.TermId, obj.SessionId);

                                                    if (exScore.Count > 0)
                                                    {
                                                        foreach (ExaminationScores scrs in exScore)
                                                        {
                                                            examScoreObtained = Convert.ToDecimal(scrs.MarkObtained);
                                                            totalExamScoreObtained += examScoreObtained;
                                                        }

                                                        //use this to get the configured percentage(e.g. 60% for Exams)
                                                        ScoreCategoryConfig configExamPercentage = categoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.Exam).FirstOrDefault();

                                                        //use this to get and summ all the scoreObtainable for first term for Examination
                                                        long examScoreObtainable = subCategoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.Exam).Sum(sc => sc.ScoreObtainable);

                                                        //computes the exam final score 
                                                        examFinalScore = ScoreComputation.computeScore(totalExamScoreObtained, examScoreObtainable, configExamPercentage.Percentage);
                                                    }
                                                    else
                                                    {
                                                        examFinalScore = 0;
                                                        totalExamScoreObtained = 0;
                                                    }


                                                    //****************************************CONTINUOUS ASSESSMENT******************************************************************

                                                    //get all the ContinousAssessment scores and add up the markObtainable
                                                    IList<ContinousAssessmentScores> caScore = reportCardReUsables.getContinuousAssessmentScores(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, admissionNumber, obj.TermId, obj.SessionId);

                                                    if (caScore.Count > 0)
                                                    {
                                                        foreach (ContinousAssessmentScores scrs in caScore)
                                                        {
                                                            CAScoreObtained = Convert.ToDecimal(scrs.MarkObtained);
                                                            totalCAScoreObtained += CAScoreObtained;
                                                        }

                                                        //use this to get the configured percentage(e.g. 60% for Exams)
                                                        ScoreCategoryConfig configCAPercentage = categoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.CA).FirstOrDefault();

                                                        //use this to get and summ all the scoreObtainable for first term for Examination
                                                        long CAscoreObtainable = subCategoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.CA).Sum(sc => sc.ScoreObtainable);

                                                        //computes the exam final score 
                                                        CAFinalScore = ScoreComputation.computeScore(totalCAScoreObtained, CAscoreObtainable, configCAPercentage.Percentage);
                                                    }
                                                    else
                                                    {
                                                        CAFinalScore = 0;
                                                        totalCAScoreObtained = 0;
                                                    }

                                                    //total score of each subjects (Examinations Scores + Cumulative Continuous Assessment Scores)
                                                    totalScore = CAFinalScore + examFinalScore;
                                                    gradeLetter = reportCardReUsables.getExamGradeLetters(totalScore, obj.SchoolId, obj.CampusId, obj.ClassId); //score grade Letter (e.g. AA, AB)
                                                    gradeRemark = reportCardReUsables.getExamGradeRemarks(totalScore, obj.SchoolId, obj.CampusId, obj.ClassId); //score grade remark (e.g. Excellent, very good )

                                                    //--------------------------Compute Continuous Assessment-------------------------------------------

                                                    if (rptConfig.ComputeCA_Cumulative == true)
                                                    {
                                                        CA_CumulativeScore = totalCAScoreObtained;
                                                    }

                                                }


                                                ///////////////////////////////////////////////////////////______________SECOND_TERM______________________///////////////////////////////////////////////////////////////////////////////////


                                                if (obj.TermId == (int)EnumUtility.Terms.SecondTerm)
                                                {
                                                    //****************************************EXAMINATION******************************************************************

                                                    //get all the examination scores and add up the markObtained by each student for each subject
                                                    IList<ExaminationScores> exScore = reportCardReUsables.getExaminationScores(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, admissionNumber, obj.TermId, obj.SessionId);

                                                    if (exScore.Count > 0)
                                                    {
                                                        foreach (ExaminationScores scrs in exScore)
                                                        {
                                                            examScoreObtained = Convert.ToDecimal(scrs.MarkObtained);
                                                            totalExamScoreObtained += examScoreObtained;
                                                        }

                                                        //use this to get the configured percentage(e.g. 60% for Exams)
                                                        ScoreCategoryConfig configExamPercentage = categoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.Exam).FirstOrDefault();

                                                        //use this to get and summ all the scoreObtainable for first term for Examination
                                                        long examScoreObtainable = subCategoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.Exam).Sum(sc => sc.ScoreObtainable);

                                                        //computes the exam final score 
                                                        examFinalScore = ScoreComputation.computeScore(totalExamScoreObtained, examScoreObtainable, configExamPercentage.Percentage);
                                                    }
                                                    else
                                                    {
                                                        examFinalScore = 0;
                                                        totalExamScoreObtained = 0;
                                                    }


                                                    //****************************************CONTINUOUS ASSESSMENT******************************************************************

                                                    //get all the ContinousAssessment scores and add up the markObtainable
                                                    IList<ContinousAssessmentScores> caScore = reportCardReUsables.getContinuousAssessmentScores(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, admissionNumber, obj.TermId, obj.SessionId);

                                                    if (caScore.Count > 0)
                                                    {
                                                        foreach (ContinousAssessmentScores scrs in caScore)
                                                        {
                                                            CAScoreObtained = Convert.ToDecimal(scrs.MarkObtained);
                                                            totalCAScoreObtained += CAScoreObtained;
                                                        }

                                                        //use this to get the configured percentage(e.g. 60% for Exams)
                                                        ScoreCategoryConfig configCAPercentage = categoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.CA).FirstOrDefault();

                                                        //use this to get and summ all the scoreObtainable for first term for Examination
                                                        long CAscoreObtainable = subCategoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.CA).Sum(sc => sc.ScoreObtainable);

                                                        //computes the exam final score 
                                                        CAFinalScore = ScoreComputation.computeScore(totalCAScoreObtained, CAscoreObtainable, configCAPercentage.Percentage);
                                                    }
                                                    else
                                                    {
                                                        CAFinalScore = 0;
                                                        totalCAScoreObtained = 0;
                                                    }

                                                    //total score of each subjects (Examinations Scores + Cumulative Continuous Assessment Scores)
                                                    totalScore = CAFinalScore + examFinalScore;
                                                    gradeLetter = reportCardReUsables.getExamGradeLetters(totalScore, obj.SchoolId, obj.CampusId, obj.ClassId); //score grade Letter (e.g. AA, AB)
                                                    gradeRemark = reportCardReUsables.getExamGradeRemarks(totalScore, obj.SchoolId, obj.CampusId, obj.ClassId); //score grade remark (e.g. Excellent, very good )

                                                    //--------------------------Compute Continuous Assessment-------------------------------------------

                                                    if (rptConfig.ComputeCA_Cumulative == true)
                                                    {
                                                        CA_CumulativeScore = totalCAScoreObtained;
                                                    }

                                                }



                                                ///////////////////////////////////////////////////////////______________THIRD_TERM______________________///////////////////////////////////////////////////////////////////////////////////


                                                if (obj.TermId == (int)EnumUtility.Terms.ThirdTerm)
                                                {
                                                    //****************************************EXAMINATION******************************************************************

                                                    //get all the examination scores and add up the markObtained by each student for each subject
                                                    IList<ExaminationScores> exScore = reportCardReUsables.getExaminationScores(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, admissionNumber, obj.TermId, obj.SessionId);

                                                    if (exScore.Count > 0)
                                                    {
                                                        foreach (ExaminationScores scrs in exScore)
                                                        {
                                                            examScoreObtained = Convert.ToDecimal(scrs.MarkObtained);
                                                            totalExamScoreObtained += examScoreObtained;
                                                        }

                                                        //use this to get the configured percentage(e.g. 60% for Exams)
                                                        ScoreCategoryConfig configExamPercentage = categoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.Exam).FirstOrDefault();

                                                        //use this to get and summ all the scoreObtainable for first term for Examination
                                                        long examScoreObtainable = subCategoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.Exam).Sum(sc => sc.ScoreObtainable);

                                                        //computes the exam final score 
                                                        examFinalScore = ScoreComputation.computeScore(totalExamScoreObtained, examScoreObtainable, configExamPercentage.Percentage);
                                                    }
                                                    else
                                                    {
                                                        examFinalScore = 0;
                                                        totalExamScoreObtained = 0;
                                                    }


                                                    //****************************************CONTINUOUS ASSESSMENT******************************************************************

                                                    //get all the ContinousAssessment scores and add up the markObtainable
                                                    IList<ContinousAssessmentScores> caScore = reportCardReUsables.getContinuousAssessmentScores(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, admissionNumber, obj.TermId, obj.SessionId);

                                                    if (caScore.Count > 0)
                                                    {
                                                        foreach (ContinousAssessmentScores scrs in caScore)
                                                        {
                                                            CAScoreObtained = Convert.ToDecimal(scrs.MarkObtained);
                                                            totalCAScoreObtained += CAScoreObtained;
                                                        }

                                                        //use this to get the configured percentage(e.g. 60% for Exams)
                                                        ScoreCategoryConfig configCAPercentage = categoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.CA).FirstOrDefault();

                                                        //use this to get and summ all the scoreObtainable for first term for Examination
                                                        long CAscoreObtainable = subCategoryConfig.Where(x => x.CategoryId == (int)EnumUtility.ScoreCategory.CA).Sum(sc => sc.ScoreObtainable);

                                                        //computes the exam final score 
                                                        CAFinalScore = ScoreComputation.computeScore(totalCAScoreObtained, CAscoreObtainable, configCAPercentage.Percentage);
                                                    }
                                                    else
                                                    {
                                                        CAFinalScore = 0;
                                                        totalCAScoreObtained = 0;
                                                    }


                                                    //total score of each subjects (Examinations Scores + Cumulative Continuous Assessment Scores)
                                                    totalScore = CAFinalScore + examFinalScore;
                                                    gradeLetter = reportCardReUsables.getExamGradeLetters(totalScore, obj.SchoolId, obj.CampusId, obj.ClassId); //score grade Letter (e.g. AA, AB)
                                                    gradeRemark = reportCardReUsables.getExamGradeRemarks(totalScore, obj.SchoolId, obj.CampusId, obj.ClassId); //score grade remark (e.g. Excellent, very good )

                                                    //--------------------------Compute Continuous Assessment-------------------------------------------

                                                    if (rptConfig.ComputeCA_Cumulative == true)
                                                    {
                                                        CA_CumulativeScore = totalCAScoreObtained;
                                                    }

                                                    //--------------------------Compute and Show First and Second Term-------------------------------------------

                                                    if (rptConfig.RefFirstTermScoreCompute == true && rptConfig.RefSecondTermScoreCompute == false)
                                                    {
                                                        //firstTerm totalScore
                                                        ReportCardData rptDataFirst = reportCardReUsables.getStudentSubjectTotalScorePerTerm(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, (int)EnumUtility.Terms.FirstTerm, obj.SessionId);
                                                        if (rptDataFirst != null)
                                                        {
                                                            firstTermTotalScore = rptDataFirst.TotalScore;
                                                        }
                                                    }

                                                    if (rptConfig.RefFirstTermScoreCompute == false && rptConfig.RefSecondTermScoreCompute == true)
                                                    {
                                                        //secondTerm totalScore
                                                        ReportCardData rptDataSecond = reportCardReUsables.getStudentSubjectTotalScorePerTerm(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, (int)EnumUtility.Terms.SecondTerm, obj.SessionId);
                                                        if (rptDataSecond != null)
                                                        {
                                                            secondTermTotalScore = rptDataSecond.TotalScore;
                                                        }
                                                    }

                                                    if (rptConfig.RefFirstTermScoreCompute == true && rptConfig.RefSecondTermScoreCompute == true)
                                                    {
                                                        //firstTerm totalScore
                                                        ReportCardData rptDataFirst = reportCardReUsables.getStudentSubjectTotalScorePerTerm(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, (int)EnumUtility.Terms.FirstTerm, obj.SessionId);
                                                        //secondTerm totalScore
                                                        ReportCardData rptDataSecond = reportCardReUsables.getStudentSubjectTotalScorePerTerm(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, (int)EnumUtility.Terms.SecondTerm, obj.SessionId);

                                                        if (rptDataFirst != null && rptDataSecond != null)
                                                        {
                                                            firstTermTotalScore = rptDataFirst.TotalScore;
                                                            secondTermTotalScore = rptDataSecond.TotalScore;
                                                        }
                                                    }

                                                    //--------------------------Compute Average Total Score (i.e. Addition of First,Second and Third term Score divided by three)-------------------------------------------

                                                    if (rptConfig.ComputeOverallTotalAverage == true)
                                                    {
                                                        //firstTerm totalScore
                                                        ReportCardData firstTermScore = reportCardReUsables.getStudentSubjectTotalScorePerTerm(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, (int)EnumUtility.Terms.FirstTerm, obj.SessionId);
                                                        //secondTerm totalScore
                                                        ReportCardData secondTermScore = reportCardReUsables.getStudentSubjectTotalScorePerTerm(std.StudentId, obj.SchoolId, obj.CampusId, obj.ClassId, obj.ClassGradeId, subject.Id, (int)EnumUtility.Terms.SecondTerm, obj.SessionId);

                                                        if (firstTermScore != null && secondTermScore != null)
                                                        {
                                                            //Overall Total (Average Total Score)
                                                            averageTotalScore = (firstTermScore.TotalScore + secondTermScore.TotalScore + totalScore) / 3;
                                                        }
                                                    }

                                                }


                                                //check the report card data (Report card data table is used to save each student Exam and CA and their total scores)
                                                ReportCardData getScoreIfExist = _context.ReportCardData.Where(s =>
                                                                      s.CAScore == Convert.ToDecimal(Math.Round(CAFinalScore, 2)) &&
                                                                      s.ExamScore == Convert.ToDecimal(Math.Round(examFinalScore, 2)) &&
                                                                      s.TotalScore == Convert.ToDecimal(Math.Round(totalScore, 2)) &&
                                                                      s.AdmissionNumber == admissionNumber &&
                                                                      s.StudentId == std.StudentId &&
                                                                      s.ClassId == obj.ClassId &&
                                                                      s.ClassGradeId == obj.ClassGradeId &&
                                                                      s.SubjectId == subject.Id &&
                                                                      s.TermId == obj.TermId &&
                                                                      s.SessionId == obj.SessionId &&
                                                                      s.DepartmentId == schSubj.DepartmentId &&
                                                                      s.Grade == gradeLetter &&
                                                                      s.Remark == gradeRemark &&
                                                                      s.SchoolId == obj.SchoolId &&
                                                                      s.CampusId == obj.CampusId).FirstOrDefault();

                                                if (getScoreIfExist == null) //update the record if it exists
                                                {
                                                    report = new ReportCardData();
                                                    report.ExamScore = Convert.ToDecimal(Math.Round(examFinalScore, 2));
                                                    report.CAScore = Convert.ToDecimal(Math.Round(CAFinalScore, 2));
                                                    report.TotalScore = Convert.ToDecimal(Math.Round(totalScore, 2));
                                                    report.AdmissionNumber = admissionNumber;
                                                    report.StudentId = std.StudentId;
                                                    report.SubjectId = subject.Id;
                                                    report.SchoolId = obj.SchoolId;
                                                    report.CampusId = obj.CampusId;
                                                    report.ClassId = obj.ClassId;
                                                    report.ClassGradeId = obj.ClassGradeId;
                                                    report.TermId = obj.TermId;
                                                    report.SessionId = obj.SessionId;
                                                    report.DepartmentId = schSubj.DepartmentId;
                                                    report.Grade = gradeLetter;
                                                    report.Remark = gradeRemark;
                                                    report.CumulativeCA_Score = CA_CumulativeScore;
                                                    report.FirstTermTotalScore = firstTermTotalScore;
                                                    report.SecondTermTotalScore = secondTermTotalScore;
                                                    report.AverageTotalScore = averageTotalScore;
                                                    report.DateComputed = DateTime.Today;

                                                    await _context.ReportCardData.AddAsync(report);

                                                }
                                                else //Save a new record if it doesnt exist
                                                {
                                                    getScoreIfExist.ExamScore = Convert.ToDecimal(Math.Round(examFinalScore, 2));
                                                    getScoreIfExist.CAScore = Convert.ToDecimal(Math.Round(CAFinalScore, 2));
                                                    getScoreIfExist.TotalScore = Convert.ToDecimal(Math.Round(totalScore, 2));
                                                    getScoreIfExist.AdmissionNumber = admissionNumber;
                                                    getScoreIfExist.StudentId = std.StudentId;
                                                    getScoreIfExist.SubjectId = subject.Id;
                                                    getScoreIfExist.SchoolId = obj.SchoolId;
                                                    getScoreIfExist.CampusId = obj.CampusId;
                                                    getScoreIfExist.ClassId = obj.ClassId;
                                                    getScoreIfExist.ClassGradeId = obj.ClassGradeId;
                                                    getScoreIfExist.TermId = obj.TermId;
                                                    getScoreIfExist.SessionId = obj.SessionId;
                                                    getScoreIfExist.DepartmentId = schSubj.DepartmentId;
                                                    getScoreIfExist.Grade = gradeLetter;
                                                    getScoreIfExist.Remark = gradeRemark;
                                                    getScoreIfExist.CumulativeCA_Score = CA_CumulativeScore;
                                                    getScoreIfExist.FirstTermTotalScore = firstTermTotalScore;
                                                    getScoreIfExist.SecondTermTotalScore = secondTermTotalScore;
                                                    getScoreIfExist.AverageTotalScore = averageTotalScore;
                                                    getScoreIfExist.DateComputed = DateTime.Today;

                                                }
                                            }

                                            await _context.SaveChangesAsync();
                                        }

                                        //-------------------------------COMPUTE THE TOTAL SCORES POSITION FOR EXAM AND CONTINUOS ASSESSMENT FOR EACH SUBJECT------------------------------------------------------------------

                                        //computes the subject position for each subject after record has been saved in the ReportCardData table
                                        IList<ReportCardData> tScores = (from t in _context.ReportCardData
                                                                         where t.SubjectId == subject.Id &&
                                                                             t.TermId == obj.TermId &&
                                                                             t.SessionId == obj.SessionId &&
                                                                             t.SchoolId == obj.SchoolId &&
                                                                             t.CampusId == obj.CampusId &&
                                                                             t.ClassId == obj.ClassId &&
                                                                             t.ClassGradeId == obj.ClassGradeId
                                                                         select t).ToList();

                                        //Sort the scores in descending order starting from the highest TotalScore
                                        IList<ReportCardData> sortedTScore = tScores.OrderByDescending(c => c.TotalScore).ToList();

                                        int pos = 1;
                                        decimal? ScorePrev = 0; int countscore = 0; int countRec = 0;

                                        foreach (ReportCardData score in sortedTScore)
                                        {
                                            countRec++;

                                            ReportCardData posDataa = _context.ReportCardData.FirstOrDefault(x => x.TotalScore == score.TotalScore &&
                                            x.Id == score.Id &&
                                            x.SubjectId == subject.Id &&
                                            x.TermId == obj.TermId &&
                                            x.SessionId == obj.SessionId &&
                                            x.SchoolId == obj.SchoolId &&
                                            x.CampusId == obj.CampusId &&
                                            x.ClassId == obj.ClassId &&
                                            x.ClassGradeId == obj.ClassGradeId

                                            );
                                            if (countRec > 1)
                                            {
                                                var data = sortedTScore.Where(d => d.Position > 0).OrderByDescending(d => d.TotalScore);
                                                foreach (var s in data)
                                                {
                                                    ScorePrev = s.TotalScore;
                                                }
                                                if (ScorePrev == score.TotalScore)
                                                {
                                                    countscore++;
                                                    posDataa.Position = pos;
                                                }
                                                else
                                                {
                                                    if (countscore > 0)
                                                    {
                                                        pos = pos + countscore;
                                                        countscore = 0;
                                                    }
                                                    pos++;
                                                    posDataa.Position = pos;
                                                }
                                            }
                                            else
                                            {
                                                posDataa.Position = pos;
                                            }
                                        }

                                        await _context.SaveChangesAsync();



                                        //-----------------------COMPUTE THE REPORT CARD POSITION FOR EACH STUDENTS---------------------------------------------------------------------------------------------

                                        //computes the student position in class after computing the total scores for Exam and CA for each subject
                                        if (gradeStudent.Count() > 0)
                                        {
                                            foreach (GradeStudents gstd in gradeStudent)
                                            {
                                                //----
                                                string admissionNumber = _context.Students.Where(s => s.Id == gstd.StudentId).FirstOrDefault().AdmissionNumber;

                                                //get the reportCardData totalScore for each students
                                                var stdTotalScore = from s in _context.ReportCardData
                                                                    where
                                                                    s.AdmissionNumber == admissionNumber &&
                                                                    s.StudentId == gstd.StudentId &&
                                                                    s.TermId == obj.TermId &&
                                                                    s.SessionId == obj.SessionId &&
                                                                    s.ClassId == obj.ClassId &&
                                                                    s.ClassGradeId == obj.ClassGradeId
                                                                    select s.TotalScore;

                                                //the list of the total Scores from reportcardData Table
                                                List<decimal> totalScoreStd = stdTotalScore.ToList();

                                                if (totalScoreStd.Count() > 0)
                                                {
                                                    decimal aggScore = 0;
                                                    long subjectCount = 0;
                                                    long totalScoreObtainable = 0;
                                                    decimal percentageScore = 0;

                                                    foreach (var oScore in totalScoreStd)
                                                    {
                                                        aggScore += oScore; //addup all the totalscore in the reportCardData for each students
                                                        subjectCount++; // count the number subjects
                                                        totalScoreObtainable += 100; //adds 100 for each scores to get the total score obtainable
                                                    }

                                                    //computes the percentage score 
                                                    percentageScore = (aggScore / totalScoreObtainable) * 100;

                                                    //check the reportcard position
                                                    ReportCardPosition rpPoss = _context.ReportCardPosition.Where(s =>
                                                        s.AdmissionNumber == admissionNumber &&
                                                        s.StudentId == gstd.StudentId &&
                                                        s.ClassId == obj.ClassId &&
                                                        s.ClassGradeId == obj.ClassGradeId &&
                                                        s.TermId == obj.TermId &&
                                                        s.SessionId == obj.SessionId).FirstOrDefault();

                                                    if (rpPoss == null) //if record does not exist, add new record
                                                    {
                                                        ReportCardPosition rpPos = new ReportCardPosition();
                                                        rpPos.AdmissionNumber = admissionNumber;
                                                        rpPos.StudentId = gstd.StudentId;
                                                        rpPos.TotalScore = aggScore;
                                                        rpPos.SchoolId = obj.SchoolId;
                                                        rpPos.CampusId = obj.CampusId;
                                                        rpPos.ClassId = obj.ClassId;
                                                        rpPos.ClassGradeId = obj.ClassGradeId;
                                                        rpPos.SessionId = obj.SessionId;
                                                        rpPos.TermId = obj.TermId;
                                                        rpPos.DateComputed = DateTime.Today;
                                                        rpPos.SubjectComputed = subjectCount;
                                                        rpPos.TotalScoreObtainable = totalScoreObtainable;
                                                        rpPos.PercentageScore = percentageScore;

                                                        await _context.ReportCardPosition.AddAsync(rpPos);
                                                        await _context.SaveChangesAsync();
                                                    }
                                                    else //update data if the record already exists
                                                    {
                                                        rpPoss.AdmissionNumber = admissionNumber;
                                                        rpPoss.StudentId = gstd.StudentId;
                                                        rpPoss.TotalScore = aggScore;
                                                        rpPoss.SchoolId = obj.SchoolId;
                                                        rpPoss.CampusId = obj.CampusId;
                                                        rpPoss.ClassId = obj.ClassId;
                                                        rpPoss.ClassGradeId = obj.ClassGradeId;
                                                        rpPoss.SessionId = obj.SessionId;
                                                        rpPoss.TermId = obj.TermId;
                                                        rpPoss.DateComputed = DateTime.Today;
                                                        rpPoss.SubjectComputed = subjectCount;
                                                        rpPoss.TotalScoreObtainable = totalScoreObtainable;
                                                        rpPoss.PercentageScore = percentageScore;

                                                        await _context.SaveChangesAsync();
                                                    }
                                                }
                                            }
                                        }

                                        //compute students class position from the reportCardPosition table sorting the totalScores in Descending order
                                        IList<ReportCardPosition> classPos = (from s in _context.ReportCardPosition
                                                                              where
                                                                              s.ClassGradeId == obj.ClassGradeId &&
                                                                              s.ClassId == obj.ClassId &&
                                                                              s.SchoolId == obj.SchoolId &&
                                                                              s.CampusId == obj.CampusId &&
                                                                              s.SessionId == obj.SessionId &&
                                                                              s.TermId == obj.TermId
                                                                              select s).ToList();
                                        if (classPos.Count() > 0)
                                        {
                                            IList<ReportCardPosition> sortedClassPos = classPos.OrderByDescending(s => s.TotalScore).ToList(); //sort the scores in descending order

                                            int classPosition = 0; int countClassScore = 0;
                                            IDictionary<decimal?, long> map = new Dictionary<decimal?, long>();

                                            foreach (ReportCardPosition classPoss in sortedClassPos) //itereate through the scores sorted in descending order
                                            {
                                                ReportCardPosition posObj = _context.ReportCardPosition.FirstOrDefault(s => s.Id == classPoss.Id &&
                                                s.TotalScore == classPoss.TotalScore && s.SchoolId == classPoss.SchoolId && s.CampusId == classPoss.CampusId &&
                                                s.ClassGradeId == classPoss.ClassGradeId && s.ClassId == classPoss.ClassId &&
                                                s.TermId == classPoss.TermId && s.SessionId == classPoss.SessionId);

                                                // if the dictionary contains same totalScore increment countClassScore and assign the same position to them
                                                if (map.ContainsKey(classPoss.TotalScore) == true)
                                                {
                                                    countClassScore++;      //increment countClassScore
                                                    posObj.Position = classPosition; //assign the classPosition to position 
                                                }
                                                else
                                                {
                                                    if (countClassScore > 0)
                                                    {
                                                        classPosition = classPosition + countClassScore;
                                                        countClassScore = 0;
                                                    }
                                                    classPosition++;
                                                    posObj.Position = classPosition;

                                                    //add the totalScore and the classPosition to the map
                                                    map.Add(classPoss.TotalScore, classPosition);
                                                }
                                            }

                                            await _context.SaveChangesAsync();
                                            map.Clear();

                                        }
                                    }
                                }

                                //return the subject scores position computed
                                var cumulativeComputedResult = (from s in _context.ReportCardPosition
                                                                where s.ClassGradeId == obj.ClassGradeId && s.ClassId == obj.ClassId
                                                                && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId &&
                                                                s.SessionId == obj.SessionId && s.TermId == obj.TermId
                                                                select new
                                                                {
                                                                    s.Id,
                                                                    s.SchoolId,
                                                                    s.CampusId,
                                                                    s.StudentId,
                                                                    s.Students.FirstName,
                                                                    s.Students.LastName,
                                                                    s.AdmissionNumber,
                                                                    s.TotalScore,
                                                                    s.TotalScoreObtainable,
                                                                    s.PercentageScore,
                                                                    s.Position,
                                                                    s.SubjectComputed,
                                                                    s.ClassId,
                                                                    s.Classes.ClassName,
                                                                    s.ClassGradeId,
                                                                    s.ClassGrades.GradeName,
                                                                    s.TermId,
                                                                    s.Terms.TermName,
                                                                    s.SessionId,
                                                                    s.Sessions.SessionName,
                                                                    s.DateComputed
                                                                }).OrderByDescending(x => x.TotalScore);

                                //return the student score position computed
                                var computedResultPerSubjects = (from s in _context.ReportCardData
                                                                 where s.ClassGradeId == obj.ClassGradeId && s.ClassId == obj.ClassId
                                                                 && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId &&
                                                                 s.SessionId == obj.SessionId && s.TermId == obj.TermId
                                                                 select new
                                                                 {
                                                                     s.Id,
                                                                     s.SchoolId,
                                                                     s.CampusId,
                                                                     s.StudentId,
                                                                     s.Students.FirstName,
                                                                     s.Students.LastName,
                                                                     s.AdmissionNumber,
                                                                     s.CAScore,
                                                                     s.ExamScore,
                                                                     s.TotalScore,
                                                                     s.Position,
                                                                     s.SubjectId,
                                                                     s.SchoolSubjects.SubjectName,
                                                                     s.ClassId,
                                                                     s.Classes.ClassName,
                                                                     s.ClassGradeId,
                                                                     s.ClassGrades.GradeName,
                                                                     s.TermId,
                                                                     s.Terms.TermName,
                                                                     s.SessionId,
                                                                     s.Sessions.SessionName,
                                                                     s.AveragePosition,
                                                                     s.AverageScore,
                                                                     s.Grade,
                                                                     s.Remark,
                                                                     s.DepartmentId,
                                                                     s.SubjectDepartment.DepartmentName,
                                                                     s.AverageTotalScore,
                                                                     s.CumulativeCA_Score,
                                                                     s.FirstTermTotalScore,
                                                                     s.SecondTermTotalScore,
                                                                     s.DateComputed
                                                                 }).OrderByDescending(x => x.TotalScore);

                                return new ComputeResultRespModel { StatusCode = 200, StatusMessage = "Result and Subject Position Computed Successfully", ComputedResultPerSubjects = computedResultPerSubjects.ToList(), CumulativeComputedResult = cumulativeComputedResult.ToList() };

                            }

                            return new ComputeResultRespModel { StatusCode = 409, StatusMessage = "SubCategory has not been Configured" };
                        }

                        return new ComputeResultRespModel { StatusCode = 409, StatusMessage = "Category has not been Configured" };
                    }

                    return new ComputeResultRespModel { StatusCode = 409, StatusMessage = "Report Card Configuration has not been done" };
                }

                return new ComputeResultRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new ComputeResultRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<ComputeResultRespModel> getAllComputedResultAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true && checkTerm == true && checkSession == true)
                {
                    //return the student score position computed
                    var computedResultPerSubjects = (from s in _context.ReportCardData
                                                     where s.ClassGradeId == classGradeId && s.ClassId == classId
                                                     && s.SchoolId == schoolId && s.CampusId == campusId &&
                                                     s.SessionId == sessionId && s.TermId == termId
                                                     select new
                                                     {
                                                         s.Id,
                                                         s.SchoolId,
                                                         s.CampusId,
                                                         s.StudentId,
                                                         s.AdmissionNumber,
                                                         s.CAScore,
                                                         s.ExamScore,
                                                         s.TotalScore,
                                                         s.Position,
                                                         s.SubjectId,
                                                         s.SchoolSubjects.SubjectName,
                                                         s.ClassId,
                                                         s.Classes.ClassName,
                                                         s.ClassGradeId,
                                                         s.ClassGrades.GradeName,
                                                         s.TermId,
                                                         s.Terms.TermName,
                                                         s.SessionId,
                                                         s.Sessions.SessionName,
                                                         s.AveragePosition,
                                                         s.AverageScore,
                                                         s.Grade,
                                                         s.Remark,
                                                         s.DepartmentId,
                                                         s.SubjectDepartment.DepartmentName,
                                                         s.AverageTotalScore,
                                                         s.CumulativeCA_Score,
                                                         s.FirstTermTotalScore,
                                                         s.SecondTermTotalScore,
                                                         s.DateComputed
                                                     }).OrderByDescending(x => x.TotalScore).ToList();

                    //return the subject scores position computed
                    var cumulativeComputedResult = (from s in _context.ReportCardPosition
                                                    where s.ClassGradeId == classGradeId && s.ClassId == classId
                                                    && s.SchoolId == schoolId && s.CampusId == campusId &&
                                                    s.SessionId == sessionId && s.TermId == termId
                                                    select new
                                                    {
                                                        s.Id,
                                                        s.SchoolId,
                                                        s.CampusId,
                                                        s.StudentId,
                                                        s.AdmissionNumber,
                                                        s.TotalScore,
                                                        s.TotalScoreObtainable,
                                                        s.PercentageScore,
                                                        s.Position,
                                                        s.SubjectComputed,
                                                        s.ClassId,
                                                        s.Classes.ClassName,
                                                        s.ClassGradeId,
                                                        s.ClassGrades.GradeName,
                                                        s.TermId,
                                                        s.Terms.TermName,
                                                        s.SessionId,
                                                        s.Sessions.SessionName,
                                                        s.DateComputed
                                                    }).OrderByDescending(x => x.TotalScore).ToList();


                    return new ComputeResultRespModel { StatusCode = 200, StatusMessage = "Successful", ComputedResultPerSubjects = computedResultPerSubjects, CumulativeComputedResult = cumulativeComputedResult };
                }

                return new ComputeResultRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new ComputeResultRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<ComputeResultRespModel> getComputedResultByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true && checkTerm == true && checkSession == true)
                {
                    //check if the student exists in the school
                    Students student = _context.Students.Where(s => s.Id == studentId && s.SchoolId == schoolId && s.CampusId == campusId).FirstOrDefault();

                    if (student != null)
                    {
                        //return the student score position computed
                        var computedResultPerSubjects = (from s in _context.ReportCardData
                                                         where s.StudentId == studentId && s.ClassGradeId == classGradeId && s.ClassId == classId
                                                         && s.SchoolId == schoolId && s.CampusId == campusId &&
                                                         s.SessionId == sessionId && s.TermId == termId
                                                         select new
                                                         {
                                                             s.Id,
                                                             s.SchoolId,
                                                             s.CampusId,
                                                             s.StudentId,
                                                             s.AdmissionNumber,
                                                             s.CAScore,
                                                             s.ExamScore,
                                                             s.TotalScore,
                                                             s.Position,
                                                             s.SubjectId,
                                                             s.SchoolSubjects.SubjectName,
                                                             s.ClassId,
                                                             s.Classes.ClassName,
                                                             s.ClassGradeId,
                                                             s.ClassGrades.GradeName,
                                                             s.TermId,
                                                             s.Terms.TermName,
                                                             s.SessionId,
                                                             s.Sessions.SessionName,
                                                             s.AveragePosition,
                                                             s.AverageScore,
                                                             s.Grade,
                                                             s.Remark,
                                                             s.DepartmentId,
                                                             s.SubjectDepartment.DepartmentName,
                                                             s.AverageTotalScore,
                                                             s.CumulativeCA_Score,
                                                             s.FirstTermTotalScore,
                                                             s.SecondTermTotalScore,
                                                             s.DateComputed
                                                         }).OrderByDescending(x => x.TotalScore).ToList();

                        //return the subject scores position computed
                        var cumulativeComputedResult = (from s in _context.ReportCardPosition
                                                        where s.StudentId == studentId && s.ClassGradeId == classGradeId && s.ClassId == classId
                                                        && s.SchoolId == schoolId && s.CampusId == campusId &&
                                                        s.SessionId == sessionId && s.TermId == termId
                                                        select new
                                                        {
                                                            s.Id,
                                                            s.SchoolId,
                                                            s.CampusId,
                                                            s.StudentId,
                                                            s.AdmissionNumber,
                                                            s.TotalScore,
                                                            s.TotalScoreObtainable,
                                                            s.PercentageScore,
                                                            s.Position,
                                                            s.SubjectComputed,
                                                            s.ClassId,
                                                            s.Classes.ClassName,
                                                            s.ClassGradeId,
                                                            s.ClassGrades.GradeName,
                                                            s.TermId,
                                                            s.Terms.TermName,
                                                            s.SessionId,
                                                            s.Sessions.SessionName,
                                                            s.DateComputed
                                                        }).OrderByDescending(x => x.TotalScore).FirstOrDefault();


                        return new ComputeResultRespModel { StatusCode = 200, StatusMessage = "Successful", ComputedResultPerSubjects = computedResultPerSubjects, CumulativeComputedResult = cumulativeComputedResult };
                    }

                    return new ComputeResultRespModel { StatusCode = 409, StatusMessage = "No Student With the Specified ID" };
                }

                return new ComputeResultRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new ComputeResultRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> deleteComputedResultByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true && checkTerm == true && checkSession == true)
                {
                    //check if the student exists in the school
                    Students student = _context.Students.Where(s => s.Id == studentId && s.SchoolId == schoolId && s.CampusId == campusId).FirstOrDefault();
                    if (student != null)
                    {
                        //reportcard data
                        IList<ReportCardData> rptcardData = (_context.ReportCardData.Where(s => s.StudentId == studentId && s.SchoolId == schoolId && s.CampusId == campusId
                        && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId)).ToList();
                        if (rptcardData.Count() > 0)
                        {
                            //deletes all the reportcard data record
                            _context.ReportCardData.RemoveRange(rptcardData);

                            //reportcard position
                            ReportCardPosition rptcardPos = _context.ReportCardPosition.Where(s => s.StudentId == studentId && s.SchoolId == schoolId && s.CampusId == campusId
                            && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId).FirstOrDefault();

                            if (rptcardPos != null)
                            {
                                //deletes the report card position record
                                _context.ReportCardPosition.Remove(rptcardPos);
                            }

                            await _context.SaveChangesAsync();

                            return new GenericRespModel { StatusCode = 200, StatusMessage = "Deleted Successfully" };
                        }
                    }

                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Student With the Specified ID" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
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


        public async Task<GenericRespModel> deleteAllComputedResultAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);
                var checkTerm = check.checkTermById(termId);
                var checkSession = check.checkSessionById(sessionId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true && checkTerm == true && checkSession == true)
                {
                    //reportcard data
                    IList<ReportCardData> rptcardData = (_context.ReportCardData.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                    && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId)).ToList();
                    if (rptcardData.Count() > 0)
                    {
                        //deletes all the reportcard data record
                        _context.ReportCardData.RemoveRange(rptcardData);

                        //reportcard position
                        IList<ReportCardPosition> rptcardPos = (_context.ReportCardPosition.Where(s => s.SchoolId == schoolId && s.CampusId == campusId
                        && s.ClassId == classId && s.ClassGradeId == classGradeId && s.TermId == termId && s.SessionId == sessionId)).ToList();

                        if (rptcardPos.Count() > 0)
                        {
                            //deletes the report card position record
                            _context.ReportCardPosition.RemoveRange(rptcardPos);
                        }

                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Deleted Successfully" };
                    }
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
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


        //----------------------------REPORT CARD PIN GENERATION----------------------------------------------------

        public async Task<GenericRespModel> generatePinsAsync(PinCreateReqModel obj)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkTerm = check.checkTermById(obj.TermId);
                var checkSession = check.checkSessionById(obj.SessionId);

                IList<object> pinList = new List<object>();

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkTerm == true && checkSession == true)
                {
                    int count = (int)obj.NoOfPinsToGenerate;
                    string schoolCode = new AdmissionNumberGenerator(_context).RetrieveSchoolCode(obj.SchoolId).SchoolCode;

                    for (int i = 1; i <= count; i++)
                    {
                        string random = RandomNumberGenerator.RandomString().ToUpper();
                        string pinGenerated = schoolCode + "-" + random;

                        var reportCardPin = new ReportCardPin()
                        {
                            Pin = pinGenerated,
                            CreatedById = obj.CreatedById,
                            SchoolId = obj.SchoolId,
                            CampusId = obj.CampusId,
                            TermId = obj.TermId,
                            SessionId = obj.SessionId,
                            IsUsed = false,
                            NoOfTimesValid = 5,
                            NoOfTimesUsed = 0,
                            DateCreated = DateTime.Now
                        };

                        await _context.ReportCardPin.AddAsync(reportCardPin);
                    }

                    await _context.SaveChangesAsync();

                    //return the ReportCarPin Created
                    var queryResult = from st in _context.ReportCardPin.Take(count) orderby st.Id descending
                                      select new
                                      {
                                          st.Id,
                                          st.Pin,
                                          st.CreatedById,
                                          st.SchoolUsers.FirstName,
                                          st.SchoolUsers.LastName,
                                          st.SchoolId,
                                          st.Schools.SchoolName,
                                          st.CampusId,
                                          st.SchoolCampus.CampusName,
                                          st.SessionId,
                                          st.Sessions.SessionName,
                                          st.TermId,
                                          st.Terms.TermName,
                                          st.ViewedClassId,
                                          st.ViewedClassGradeId,
                                          st.NoOfTimesValid,
                                          st.NoOfTimesUsed,
                                          st.IsUsed,
                                          st.IsUsedById,
                                          st.DateCreated,
                                          st.DateLastUsed,
                                      };

                    if (queryResult.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = (long)HttpStatusCode.OK, StatusMessage = $"Successfully Generated {count} Pins", Data = queryResult };
                    }
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "A Paremeter With Specified ID does not exist" };
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

        public async Task<GenericRespModel> getPinByIdAsync(long pinId)
        {
            try
            {
                //return the ReportCarPin Created
                var result = from st in _context.ReportCardPin
                             where st.Id == pinId
                             select new
                             {
                                 st.Id,
                                 st.Pin,
                                 st.CreatedById,
                                 st.SchoolUsers.FirstName,
                                 st.SchoolUsers.LastName,
                                 st.SchoolId,
                                 st.Schools.SchoolName,
                                 st.CampusId,
                                 st.SchoolCampus.CampusName,
                                 st.SessionId,
                                 st.Sessions.SessionName,
                                 st.TermId,
                                 st.Terms.TermName,
                                 st.ViewedClassId,  
                                 st.ViewedClassGradeId,
                                 st.NoOfTimesValid,
                                 st.NoOfTimesUsed,
                                 st.IsUsed,
                                 st.IsUsedById,
                                 st.DateCreated,
                                 st.DateLastUsed,
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
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

        public async Task<GenericRespModel> getAllPinsAsync(long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                //return the ReportCarPin Created
                var result = from st in _context.ReportCardPin
                             where st.SchoolId == schoolId && st.CampusId == campusId 
                             && st.TermId == termId && st.SessionId == sessionId
                             select new
                             {
                                 st.Id,
                                 st.Pin,
                                 st.CreatedById,
                                 st.SchoolUsers.FirstName,
                                 st.SchoolUsers.LastName,
                                 st.SchoolId,
                                 st.Schools.SchoolName,
                                 st.CampusId,
                                 st.SchoolCampus.CampusName,
                                 st.SessionId,
                                 st.Sessions.SessionName,
                                 st.TermId,
                                 st.Terms.TermName,
                                 st.ViewedClassId,
                                 st.ViewedClassGradeId,
                                 st.NoOfTimesValid,
                                 st.NoOfTimesUsed,
                                 st.IsUsed,
                                 st.IsUsedById,
                                 st.DateCreated,
                                 st.DateLastUsed,
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
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

        public async Task<GenericRespModel> getPinsByStatusAsync(long schoolId, long campusId, long termId, long sessionId, bool isUsed)
        {
            try
            {
                //return the ReportCarPin Created
                var result = from st in _context.ReportCardPin
                             where st.SchoolId == schoolId && st.CampusId == campusId
                             && st.TermId == termId && st.SessionId == sessionId && st.IsUsed == isUsed
                             select new
                             {
                                 st.Id,
                                 st.Pin,
                                 st.CreatedById,
                                 st.SchoolUsers.FirstName,
                                 st.SchoolUsers.LastName,
                                 st.SchoolId,
                                 st.Schools.SchoolName,
                                 st.CampusId,
                                 st.SchoolCampus.CampusName,
                                 st.SessionId,
                                 st.Sessions.SessionName,
                                 st.TermId,
                                 st.Terms.TermName,
                                 st.ViewedClassId,
                                 st.ViewedClassGradeId,
                                 st.NoOfTimesValid,
                                 st.NoOfTimesUsed,
                                 st.IsUsed,
                                 st.IsUsedById,
                                 st.DateCreated,
                                 st.DateLastUsed,
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
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

        public async Task<GenericRespModel> deletePinsAsync(long pinId)
        {
            try
            {
                var getPin = _context.ReportCardPin.Where(s => s.Id == pinId).FirstOrDefault();

                if (getPin != null)
                {
                    if (getPin.IsUsed == true && getPin.NoOfTimesUsed < getPin.NoOfTimesValid)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = $"This Pin has been used {getPin.NoOfTimesUsed} time(s) and Cannot be deleted" };
                    }
                    else
                    {
                        //delete the Pin
                        _context.ReportCardPin.Remove(getPin);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Pin Deleted Successfully" };
                    }
                }
                
                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Pin with the Specified ID" };
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