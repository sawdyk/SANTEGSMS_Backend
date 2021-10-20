using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class UpdateSchoolDetailsReqModel
    {
        public string SchoolName { get; set; }
        public string SchoolLogouRL { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string Motto { get; set; }
    }
}
