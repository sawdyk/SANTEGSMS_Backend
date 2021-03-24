using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace SANTEGSMS.RequestModels
{
    public class SessionReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public string SessionName { get; set; }
    }
}
