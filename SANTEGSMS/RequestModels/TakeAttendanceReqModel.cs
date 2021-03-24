using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class TakeAttendanceReqModel
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
        public long AttendancePeriodId { get; set; }
        [Required]
        public DateTime AttendanceDate { get; set; }
        [Required]
        public IEnumerable<StudentID> StudentIds { get; set; }
    }

    public class StudentID
    {
        public Guid StudentId { get; set; }
        public bool isChecked { get; set; }
    }
}
