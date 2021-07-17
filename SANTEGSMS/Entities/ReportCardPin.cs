using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class ReportCardPin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Pin { get; set; }
        public Guid CreatedById { get; set; }
        public long ViewedClassId { get; set; }
        public long ViewedClassGradeId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public bool IsUsed { get; set; }
        public string IsUsedById { get; set; }
        public long NoOfTimesValid { get; set; }
        public long NoOfTimesUsed { get; set; }
        public DateTime DateCreated { get; set; }
        public string DateLastUsed { get; set; }


        [ForeignKey("CreatedById")]
        public virtual SchoolUsers SchoolUsers { get; set; }

        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampus SchoolCampus { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

    }
}
