using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class BroadSheetRemarkReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public IList<SubjectLists> SubjectLists { get; set; }

    }

    public class SubjectLists
    {
        [Required]
        public long SubjectId { get; set; }
        [Required]
        public bool Mandatory { get; set; }

    }
}
