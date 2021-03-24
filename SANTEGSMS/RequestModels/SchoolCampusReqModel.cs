using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class SchoolCampusReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public string CampusName { get; set; }
        public string CampusAddress { get; set; }
    }
}
