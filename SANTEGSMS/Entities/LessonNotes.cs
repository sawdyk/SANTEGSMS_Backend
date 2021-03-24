using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class LessonNotes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public long SubjectId { get; set; }
        public Guid TeacherId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public long StatusId { get; set; }
        public DateTime DateUploaded { get; set; }
        public DateTime LastDateUpdated { get; set; }


        [ForeignKey("SubjectId")]
        public virtual SchoolSubjects SchoolSubjects { get; set; }

        [ForeignKey("TeacherId")]
        public virtual SchoolUsers SchoolUsers { get; set; }

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

        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
    }
}
