using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class ExtraCurricularScores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public Guid StudentId { get; set; }
        public string AdmissionNumber { get; set; }
        public long MarkObtainable { get; set; }
        [Column(TypeName = "decimal(60,4)")]
        public decimal MarkObtained { get; set; }
        public long CategoryId { get; set; }
        public long SubCategoryId { get; set; }
        public Guid TeacherId { get; set; }
        public DateTime DateUploaded { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateDeleted { get; set; }
        public string DeletedBy { get; set; }


        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampus SchoolCampus { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ClassGradeId")]
        public virtual ClassGrades ClassGrades { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ScoreCategory ScoreCategory { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual ScoreSubCategoryConfig ScoreSubCategoryConfig { get; set; }

        [ForeignKey("TeacherId")]
        public virtual SchoolUsers SchoolUsers { get; set; }
    }
}
