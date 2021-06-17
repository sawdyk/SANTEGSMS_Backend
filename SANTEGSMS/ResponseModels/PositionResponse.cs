using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    public class PositionResponse
    {
        public long Score { get; set; }
        public long Position { get; set; }
        public object Data { get; set; }
    }
}
