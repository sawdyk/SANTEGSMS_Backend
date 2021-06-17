using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class ReportCardPosition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid StudentId { get; set; }
        public string AdmissionNumber { get; set; }
        public decimal TotalScore { get; set; }
        public long TotalScoreObtainable { get; set; }
        public decimal PercentageScore { get; set; }
        public long Position { get; set; }
        public long SubjectComputed { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public DateTime DateComputed { get; set; }


        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampus SchoolCampus { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ClassGradeId")]
        public virtual ClassGrades ClassGrades { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }
    }
}
