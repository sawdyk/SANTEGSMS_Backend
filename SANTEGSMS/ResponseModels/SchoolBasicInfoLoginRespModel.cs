using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    public class SchoolBasicInfoLoginRespModel
    {
        public long SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolTypeName { get; set; }
        public string SchoolLogoUrl { get; set; }
        public string SchoolPhoneNumber { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolEmailAddress { get; set; }
        public string SchoolMotto { get; set; }
        public long CampusId { get; set; }
        public string CampusName { get; set; }
        public string CampusAddress { get; set; }
    }
}
