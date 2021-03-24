using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class LocalGovtReqModel
    {
        [Required]
        public long StateId { get; set; }
        [Required]
        public string LocalGovtName { get; set; }

    }
}
