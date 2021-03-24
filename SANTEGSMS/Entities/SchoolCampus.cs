using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class SchoolCampus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? SchoolId { get; set; }
        public string CampusName { get; set; }
        public string CampusAddress { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }
    }
}
