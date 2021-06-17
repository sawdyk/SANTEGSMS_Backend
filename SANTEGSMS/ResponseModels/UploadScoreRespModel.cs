using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class UploadScoreRespModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object ScoresUploaded { get; set; }
        public object ExistingScores { get; set; }
    }
}
