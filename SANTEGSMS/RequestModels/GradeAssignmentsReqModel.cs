using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class GradeAssignmentsReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public long AssignmentId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public IEnumerable<AssignmentScore> AssignmentScore { get; set; }

    }

    public class AssignmentScore
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public long ScoreObtained { get; set; }
    }
}
