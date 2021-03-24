using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class TeacherCreateReqModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public IEnumerable<TeacherRoleIds> RoleIds { get; set; } //A list of Teacher Roles (Class or Subject Teacher)
    }
    public class TeacherRoleIds
    {
        public long Id { get; set; }
    }
}
