using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class ReportCardConfigReqModel
    {
        public bool ShowSubject { get; set; }
        public bool ShowDepartment { get; set; }
        public bool ShowCAScore { get; set; }
        public bool ShowExamScore { get; set; }
        public bool ComputeCA_Cumulative { get; set; }
        public bool ShowCA_Cumulative { get; set; }
        public bool MultipleLegend { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public Guid CreatedOrUpdatedBy { get; set; }
        public bool RefFirstTermScoreCompute { get; set; }  //third term
        public bool RefFirstTermScoreShow { get; set; }  //third term
        public bool RefSecondTermScoreCompute { get; set; }  //third term
        public bool RefSecondTermScoreShow { get; set; }  //third term
        public bool ComputeOverallTotalAverage { get; set; } //third term
        public bool ShowComputeOverallTotalAverage { get; set; }  //third term
    }
}
