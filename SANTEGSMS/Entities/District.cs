using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? StateId { get; set; }
        public long? LocalGovtId { get; set; }
        public Guid? DistrictAdminId { get; set; }
        public string DistrictName { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("StateId")]
        public virtual States States { get; set; }

        [ForeignKey("LocalGovtId")]
        public virtual LocalGovt LocalGovt { get; set; }

        [ForeignKey("DistrictAdminId")]
        public virtual DistrictAdministrators DistrictAdministrators { get; set; }
    }
}
