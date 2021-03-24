using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Entities
{
    public class Students
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string UserName { get; set; }
        public string AdmissionNumber { get; set; }
        public long? GenderId { get; set; }
        public long StaffStatus { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string YearOfAdmission { get; set; }
        public string StateOfOrigin { get; set; }
        public string LocalGovt { get; set; }
        public string Religion { get; set; }
        public string HomeAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Status { get; set; }
        public string StudentStatus { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public long? SchoolId { get; set; }
        public long? CampusId { get; set; }
        public bool IsAssignedToClass { get; set; }
        public bool hasParent { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("SchoolId")]
        public virtual Schools Schools { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampus SchoolCampus { get; set; }

        [ForeignKey("GenderId")]
        public virtual Gender Gender { get; set; }
    }
}
