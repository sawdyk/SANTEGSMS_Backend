using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FileUploadRespModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public FileData FileData { get; set; }       
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class FileData
    {
        public string OriginalFileName { get; set; }
        public string FileExtension { get; set; }
        public string FileType { get; set; }
        public string FileUrl { get; set; }
        public string FileSize { get; set; }
        public string UniqueFileName { get; set; }

    }
}
