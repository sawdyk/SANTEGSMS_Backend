using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class AssignSubjectToTeacherReqModel
    {
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public IEnumerable<SubjectId> SubjectIds { get; set; }
    }
    public class SubjectId
    {
        public long Id { get; set; }
    }
}
