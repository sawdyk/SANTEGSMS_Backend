using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.RequestModels
{
    public class StudentCreationReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public string StudentFirstName { get; set; }
        [Required]
        public string StudentLastName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        public long GenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string YearOfAdmission { get; set; }
        public string StateOfOrigin { get; set; }
        public string LocalGovt { get; set; }
        public string Religion { get; set; }
        public string HomeAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ProfilePictureUrl { get; set; }

        //Parents Data
        [Required]
        public string ParentFirstName { get; set; }
        [Required]
        public string ParentLastName { get; set; }
        [Required]
        public string ParentEmail { get; set; }
        [Required]
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

    public class StudentParentExistCreationReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public Guid ParentId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public long GenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string YearOfAdmission { get; set; }
        public string StateOfOrigin { get; set; }
        public string LocalGovt { get; set; }
        public string Religion { get; set; }
        public string HomeAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ProfilePictureUrl { get; set; }

    }


    public class UpdateStudentReqModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public string StudentFirstName { get; set; }
        [Required]
        public string StudentLastName { get; set; }
        public string MiddleName { get; set; }
        public long GenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string YearOfAdmission { get; set; }
        public string StateOfOrigin { get; set; }
        public string LocalGovt { get; set; }
        public string Religion { get; set; }
        public string HomeAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
