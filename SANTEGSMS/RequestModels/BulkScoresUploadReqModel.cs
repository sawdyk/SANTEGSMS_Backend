using Microsoft.AspNetCore.Http;
using SANTEGSMS.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class BulkScoresUploadReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long CategoryId { get; set; }
        [Required]
        public long SubCategoryId { get; set; }
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        public long ScoreSheetTemplateId { get; set; }
        [Required]
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile File { get; set; }
    }
}
