using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class ActivateSchoolAccountReqModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
