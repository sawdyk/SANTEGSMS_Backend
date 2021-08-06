using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class UpdateParentReqModel
    {
        //Parents Data
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string ParentEmail { get; set; }
        public string ParentPhoneNumber { get; set; }
        public long ParentGenderId { get; set; }
        public string ParentNationality { get; set; }
        public string ParentState { get; set; }
        public string ParentCity { get; set; }
        public string ParentHomeAddress { get; set; }
        public string ParentOccupation { get; set; }
        public string ParentStateOfOrigin { get; set; }
        public string ParentLocalGovt { get; set; }
        public string ParentReligion { get; set; }
    }
}
