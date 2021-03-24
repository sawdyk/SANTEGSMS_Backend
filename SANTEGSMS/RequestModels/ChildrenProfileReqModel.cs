using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class ChildrenProfileReqModel
    {
        public IList<Guid> ChildrenId { get; set; }
        public Guid ParentId { get; set; }
    }
}
