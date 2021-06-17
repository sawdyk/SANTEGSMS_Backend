using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class ReportCardConfigurationLegendReqModel
    {
        [Required]
        public string LegendName { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long StatusId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public Guid CreatedOrUpdatedBy { get; set; }
        public IList<LegendList> LegendList { get; set; }
    }

    public class LegendList
    {
        public string ReferenceRange { get; set; }
        public string ReferenceValue { get; set; }
    }

    public class UpdateLegendReqModel
    {
        [Required]
        public string LegendName { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long StatusId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public Guid CreatedOrUpdatedBy { get; set; }
    }
}
