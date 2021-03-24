using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class OrderOfSubjectsReqModel
    {
        [Required]
        public long SubjectId { get; set; }
        [Required]
        public long OrderNumber { get; set; }
    }
}
