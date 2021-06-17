using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ScoreUploadSheetRespModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string FileName { get; set; }
        public object ScoreSheetTemplate { get; set; }
        public object Students { get; set; }
        public object Subjects { get; set; }
    }
}
