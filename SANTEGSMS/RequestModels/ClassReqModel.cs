using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class ClassReqModel
    {
        [Required]
        public string ClassName { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
    }
}
