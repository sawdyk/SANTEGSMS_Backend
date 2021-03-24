using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class StateReqModel
    {
        [Required]
        public IList<string> StateName { get; set; }
    }
}
