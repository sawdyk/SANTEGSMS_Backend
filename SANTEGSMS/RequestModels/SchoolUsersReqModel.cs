using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class SchoolUsersReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public IEnumerable<RoleIds> RoleIds { get; set; } //A list of Roles
    }

    public class RoleIds
    {
        public long Id { get; set; }
    }
}
