using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class MoveStudentReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long ClassOrAlumniId { get; set; }
        [Required]
        public IEnumerable<StudentId> StudentIds { get; set; }
    }
}
