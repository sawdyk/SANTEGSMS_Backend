using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SANTEGSMS.Helpers;

namespace SANTEGSMS.RequestModels
{
    public class BulkStudentReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile File { get; set; }
    }
}
