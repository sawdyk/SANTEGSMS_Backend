using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace SANTEGSMS.Entities
{
    public class AcademicSessions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public long SchoolId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool IsApproved { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsClosed { get; set; }
        public bool IsOpened { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("UserId")]
        public virtual SchoolUsers SchoolUsers { get; set; }
    }
}
