using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class StudentDuplicates
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string NewStudentFullName { get; set; }
        public Guid ExistingStudentId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampus SchoolCampus { get; set; }

        [ForeignKey("ExistingStudentId")]
        public virtual Students Students { get; set; }
    }
}
