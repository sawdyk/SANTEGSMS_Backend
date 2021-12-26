using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class StudentBulkCreationRespModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public long NumberOfStudentsCreated { get; set; }
        public long NumberOfExistingParents { get; set; }
        public object StudentsData { get; set; }
        public object ExistingParentsInfoInSchool { get; set; }
        public object ExistingStudentsInfo { get; set; }
        public object ExistingParentsEmail{ get; set; }
    }
}
