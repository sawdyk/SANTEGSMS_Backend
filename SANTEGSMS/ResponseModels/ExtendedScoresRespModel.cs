using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    public class ExtendedScoresRespModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public IList<StudentDetails> Data { get; set; }
    }

    public class StudentDetails
    {
        public string StudentFullName { get; set; }
        public string AdmissionNumber { get; set; }
        public IList<object> SubjectDetails { get; set; }

    }

    public class SubjectDetails
    {
        public string SubjectName { get; set; }
        public decimal SubjectScore { get; set; }

    }
}

