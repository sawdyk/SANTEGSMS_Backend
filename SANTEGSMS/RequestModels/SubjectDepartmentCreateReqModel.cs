using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class SubjectDepartmentCreateReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public string DepartmentName { get; set; }
    }
}
