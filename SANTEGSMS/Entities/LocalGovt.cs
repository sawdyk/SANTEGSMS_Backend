using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class LocalGovt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? StateId { get; set; }
        public string LocalGovtName { get; set; }

        [ForeignKey("StateId")]
        public virtual States States { get; set; }
    }
}
