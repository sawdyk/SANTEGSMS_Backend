using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class ReportCardConfigurationLegendList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long LegendId { get; set; }
        public string ReferenceRange { get; set; }
        public string ReferenceValue { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateDeleted { get; set; }
        public string DeletedBy { get; set; }


        [ForeignKey("LegendId")]
        public virtual ReportCardConfigurationLegend ReportCardConfigurationLegend { get; set; }
    }
}
