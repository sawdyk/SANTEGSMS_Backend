using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class UploadScoreReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long CategoryId { get; set; }
        [Required]
        public long SubCategoryId { get; set; }
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        public IList<StudentScoreList> StudentScoreLists { get; set; }
    }

    public class UploadSingleStudentScoreReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long CategoryId { get; set; }
        [Required]
        public long SubCategoryId { get; set; }
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public decimal MarkObtained { get; set; }
    }

    public class StudentScoreList
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public decimal MarkObtained { get; set; }
    }
    public class UploadSubjectScoreReqModel : UploadScoreReqModel
    {
        [Required]
        public long SubjectId { get; set; }
    }
}
