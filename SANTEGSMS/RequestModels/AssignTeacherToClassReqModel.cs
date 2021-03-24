using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class AssignTeacherToClassReqModel
    {
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
    }
}
