using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class ParentsStudentsMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid StudentId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("ParentId")]
        public virtual Parents Parents { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampus SchoolCampus { get; set; }
    }
}
