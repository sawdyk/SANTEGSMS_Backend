using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    public class BroadSheetRespModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public BroadSheetSummaryInfo BroadSheetSummaryInfo { get; set; }
        public BroadSheetHeaderInfo BroadSheetHeaderInfo { get; set; }
        public SchoolAndClassInfo SchoolAndClassInfo { get; set; }
        public IList<StudentSubjectScore> StudentSubjectScore { get; set; }

    }
    public class SchoolAndClassInfo
    {
        public string SchoolName { get; set; }
        public string CampusName { get; set; }
        public string CampusAddress { get; set; }
        public string Session { get; set; }
        public string Term { get; set; }
        public string Class { get; set; }
        public string ClassGrade { get; set; }
        public string ClassTeacherName { get; set; }
        public string DateGenerated { get; set; }
    }

    public class SubjectScore
    {
        public string SubjectName { get; set; }
        public decimal Score { get; set; }
    }

    public class StudentSubjectScore
    {
        public BroadSheetStudentInfo StudentInfo { get; set; }
        public IList<SubjectScore> SubjectScore { get; set; }
        public CumulativeScore CumulativeScore { get; set; }
    }

    public class CumulativeScore
    {
        public decimal Total { get; set; }
        public decimal Percentage { get; set; }
        public string Grade { get; set; }
        public string Remark { get; set; }

    }

    public class BroadSheetStudentInfo
    {
        public Guid StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
    }

    public class BroadSheetHeaderInfo
    {
        public BroadSheetCandidatesInfo BroadSheetCandidatesInfo { get; set; }
        public BroadSheetSubjectsInfo BroadSheetSubjectsInfo { get; set; }
        public BroadSheetScoresInfo BroadSheetScoresInfo { get; set; }
    }

    public class BroadSheetCandidatesInfo
    {
        public string Surname => "SURNAME";
        public string FirstName => "FIRSTNAME";
        public string MiddleName => "MIDDLENAME";
        public string Gender => "GENDER";
    }

    public class BroadSheetSubjectsInfo
    {
        public IList<string> Subjects { get; set; }
    }

    public class BroadSheetScoresInfo
    {
        public string Total => "TOTAL";
        public string Percentage => "PERCENTAGE (%)";
        public string Grade => "GRADE";
        public string Remark => "REMARK";
    }


    public class BroadSheetSummaryInfo
    {
        public string NoPasseed_Promoted { get; set; }
        public string NoFailed_Repeat { get; set; }
        public string NoInClass { get; set; }
        public string NoPresent { get; set; }
        public string NoAbsent { get; set; }
    }

}
