using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class StudentAttendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public string AdmissionNumber { get; set; }
        public Guid StudentId { get; set; }
        public long TermId { get; set; }
        public long SessionId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long PresentAbsent { get; set; }
        public long AttendancePeriodId { get; set; }
        public long AttendancePeriodIdMorning { get; set; }
        public long AttendancePeriodIdAfternoon { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateDeleted { get; set; }
        public string DeletedBy { get; set; }



        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampus SchoolCampus { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ClassGradeId")]
        public virtual ClassGrades ClassGrades { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("AttendancePeriodId")]
        public virtual AttendancePeriod AttendancePeriod { get; set; }
    }
}
