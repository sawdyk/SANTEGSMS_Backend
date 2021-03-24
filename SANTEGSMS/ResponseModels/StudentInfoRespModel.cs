using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    public class StudentInfoRespModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string AdmissionNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime DateCreated { get; set; }
        public StudentClassInfo StudentClassInfo { get; set; }
    }

    public class StudentClassInfo
    {
        public string Message { get; set; }
        public long ClassId { get; set; }
        public string ClassName { get; set; }
        public long ClassGradeId { get; set; }
        public string ClassGradeName { get; set; }
    }
}
