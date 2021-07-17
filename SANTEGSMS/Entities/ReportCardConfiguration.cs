using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class ReportCardConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public bool ShowSubject { get; set; }  //all terms
        public bool ShowDepartment { get; set; }  //all terms
        public bool ShowCAScore { get; set; }  //all terms
        public bool ShowExamScore { get; set; }  //all terms
        public bool ComputeCA_Cumulative { get; set; } //all terms
        public bool ShowCA_Cumulative { get; set; } //all terms
        public bool MultipleLegend { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long TermId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool RefFirstTermScoreCompute { get; set; }  //third term
        public bool RefFirstTermScoreShow { get; set; }  //third term
        public bool RefSecondTermScoreCompute { get; set; }  //third term
        public bool RefSecondTermScoreShow { get; set; }  //third term
        public bool ComputeOverallTotalAverage { get; set; } //third term
        public bool ShowComputeOverallTotalAverage { get; set; }  //third term
        public bool IsDeleted { get; set; }
        public DateTime DateDeleted { get; set; }
        public string DeletedBy { get; set; }



        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampus SchoolCampus { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual SchoolUsers SchoolUsersCreatedBy { get; set; }

        [ForeignKey("LastUpdatedBy")]
        public virtual SchoolUsers SchoolUsersLastUpdatedBy { get; set; }
    }
}
