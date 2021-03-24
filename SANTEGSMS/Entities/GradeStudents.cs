using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SANTEGSMS.Entities
{
    public class GradeStudents
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid StudentId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long SessionId { get; set; }
        public bool HasGraduated { get; set; }
        public DateTime DateCreated { get; set; }


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
    }
}
