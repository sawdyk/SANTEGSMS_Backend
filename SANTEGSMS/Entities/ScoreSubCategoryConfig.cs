using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class ScoreSubCategoryConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long ClassId { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public string SubCategoryName { get; set; }
        public long ScoreObtainable { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("CategoryId")]
        public virtual ScoreCategory ScoreCategory { get; set; }

        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampus SchoolCampus { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }
    }
}
