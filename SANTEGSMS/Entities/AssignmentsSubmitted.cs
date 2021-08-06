using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    [Table("AssignmentsSubmitted")]
    public class AssignmentsSubmitted
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long AssignmentId { get; set; }
        public Guid StudentId { get; set; }
        public decimal? ObtainableScore { get; set; }
        public decimal? ScoreObtained { get; set; }
        public string FileUrl { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public long ScoreStatusId { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateGraded { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsGraded { get; set; }
        public DateTime DateDeleted { get; set; }
        public string DeletedBy { get; set; }


        [ForeignKey("AssignmentId")]
        public virtual Assignments Assignments { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ClassGradeId")]
        public virtual ClassGrades ClassGrades { get; set; }

        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampus SchoolCampus { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("ScoreStatusId")]
        public virtual ScoreStatus ScoreStatus { get; set; }
    }
}
