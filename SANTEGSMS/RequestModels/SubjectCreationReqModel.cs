using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class SubjectCreationReqModel
    {
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public long MaximumScore { get; set; } //Maximum score of the subject
    }
}
