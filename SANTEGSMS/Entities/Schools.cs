using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class Schools
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string Motto { get; set; }
        public string SchoolLogoUrl { get; set; }
        public long? SchoolTypeId { get; set; }
        public long? StateId { get; set; }
        public long? LocalGovtId { get; set; }
        public long? DistrictId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }


        [ForeignKey("SchoolTypeId")]
        public virtual SchoolType SchoolType { get; set; }

        [ForeignKey("StateId")]
        public virtual States States { get; set; }

        [ForeignKey("LocalGovtId")]
        public virtual LocalGovt LocalGovt { get; set; }

        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
    }
}
