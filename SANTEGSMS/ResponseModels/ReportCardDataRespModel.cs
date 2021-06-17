using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    public class ReportCardDataRespModel
    {

        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object Data { get; set; }
    }

    public class StudentResult
    {
        public ReportCardTemplateInfo ReportCardTemplateInfo { get; set; }
        public SchoolInfo SchoolInfo { get; set; }
        public StudentInfo StudentInfo { get; set; }
        public ReportCardHeaderInfo ReportCardHeaderInfo { get; set; }
        public ReportCardResult ReportCardResult { get; set; }
        public CumulativeReportCardData CumulativeReportCardData { get; set; }
        public object ExtraCurricularData { get; set; }
        public object BehaviouralData { get; set; }
        public object LegendData { get; set; }
        public RemarksAndCommentData RemarksAndCommentData { get; set; }
        public OtherData OtherData { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ReportCardHeaderInfo
    {
        public string Department { get; set; }
        public string Subject => "SUBJECT";
        public object CA_SubCategory { get; set; }
        public string CA_Cumulative { get; set; }
        public string CA_Total => "CA";
        public string Exam => "EXAM";
        public string FirstTermScore { get; set; }
        public string SecondTermScore { get; set; }
        public string TotalScore => "TOTAL";
        public string Grade => "GRADE";
        public string Remark => "REMARK";

    }

    public class ReportCardTemplateInfo
    {
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long ClassId { get; set; }
        public long SchoolSubTypeId { get; set; }
        public string Description { get; set; }
    }

    public class SchoolInfo
    {
        public string SchoolName { get; set; }
        public string SchoolType { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string LogoUrl { get; set; }
        public string Motto { get; set; }
    }
    public class StudentInfo
    {
        public Guid StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FullName { get; set; }
        public string Class { get; set; }
        public string ClassGrade { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string StudentPassport { get; set; }
    }


    //--------------------------------------------------------

    public class ReportCardResult
    {
        public IList<Result> Result { get; set; }
    }

    public class Result
    {
        public string DepartmentName { get; set; }
        public IList<SubjectScores> SubjectScores { get; set; }

    }


    public class SubjectScores
    {
        public string SubjectName { get; set; }
        public IList<ContinuousAssessments> ContinuousAssessments { get; set; }
        public decimal CA_Cumulative { get; set; }
        public decimal CA_Total { get; set; }
        public decimal Exam { get; set; }
        public decimal FirstTermScore { get; set; }
        public decimal SecondTermScore { get; set; }
        public decimal TotalScore { get; set; }
        public string Grade { get; set; }
        public string Remark { get; set; }
    }

    public class ContinuousAssessments
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal ScoreObtained { get; set; }

    }


    //-------------------------------------------------------


    public class CumulativeReportCardData
    {
        public decimal TotalScoreObtainable { get; set; }
        public decimal TotalScoreObtained { get; set; }
        public decimal PercentageScore { get; set; }
        public long NoOfSubjectsOffered { get; set; }
        public string PositionInClass { get; set; }

    }

    //comments on reportcrad
    public class RemarksAndCommentData
    {
        public string ClassTeachersComment { get; set; }
        public string ClassTeachersRemark { get; set; }
        public string PrincipalsComment { get; set; }
        public string PrincipalsRemark { get; set; }
    }

    //Next term begins, Signature, etc
    public class OtherData
    {
        public string Session { get; set; }
        public string Term { get; set; }
        public long NoInClass { get; set; }
        public string NextTermBeginsDate { get; set; }
        public string ClassTeachersFullName { get; set; }
        public string Signature { get; set; }
        public string DateGenerated { get; set; }
    }

}
