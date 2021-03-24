using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class DistrictReqModel
    {
        [Required]
        public long StateId { get; set; }
        [Required]
        public long LocalGovtId { get; set; }
        [Required]
        public Guid DistrictAdminId { get; set; }
        [Required]
        public string DistrictName { get; set; }
    }
}
