using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class SchoolSubTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SchoolTypeId { get; set; }
        public string SubTypeName { get; set; }

        [ForeignKey("SchoolTypeId")]
        public virtual SchoolType SchoolType { get; set; }
    }
}
