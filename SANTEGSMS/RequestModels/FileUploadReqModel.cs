using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class FileUploadReqModel
    {
        [Required]
        public string AppId { get; set; }
        [Required]
        public string FolderTypeId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
