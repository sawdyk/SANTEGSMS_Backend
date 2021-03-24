using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class AssignSubjectToDepartmentReqModel
    {
        [Required]
        public long DepartmentId { get; set; }
        [Required]
        public IEnumerable<SubjectIds> SubjectId { get; set; }
    }

    public class SubjectIds
    {
        public long Id { get; set; }
    }
}
