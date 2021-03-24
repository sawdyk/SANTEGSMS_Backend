using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class DeleteStudentAssignedReqModel
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
    }
}
