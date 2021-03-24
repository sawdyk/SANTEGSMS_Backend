using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class SchoolUserRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid? UserId { get; set; }
        public long? RoleId { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("UserId")]
        public virtual SchoolUsers SchoolUsers { get; set; }

        [ForeignKey("RoleId")]
        public virtual SchoolRoles SchoolRoles { get; set; }
    }
}
