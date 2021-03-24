using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class AcademicSessionReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
