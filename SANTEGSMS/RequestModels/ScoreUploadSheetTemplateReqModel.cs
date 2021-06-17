using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class ScoreUploadSheetTemplateReqModel
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        public IList<SubjectId> SubjectId { get; set; }
    }
}
