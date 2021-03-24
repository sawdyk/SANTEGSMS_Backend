using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    public class SchoolBasicInfoRespModel
    {
        public Guid SchoolSuperAdministratorId { get; set; }
        public long SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public long SchoolTypeId { get; set; }
        public string SchoolTypeName { get; set; }
    }
}
