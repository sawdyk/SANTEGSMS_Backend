using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class BroadsheetGradeReqModel
    {

        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long LowestRange { get; set; }
        [Required]
        public long HighestRange { get; set; }
        [Required]
        public string Grade { get; set; }
    }

    public class PerformanceAnalysisReqModel
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
        public IList<long> ClassIds { get; set; }
       
    }
}
