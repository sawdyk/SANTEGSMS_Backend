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
        public long CampusId { get; set; }
        public string CampusName { get; set; }
        public string CampusAddress { get; set; }
    }
}
