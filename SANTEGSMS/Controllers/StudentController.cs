using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SANTEGSMS.IRepos;
using SANTEGSMS.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SANTEGSMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepo _studentRepo;

        public StudentController(IStudentRepo studentRepo)
        {
            _studentRepo = studentRepo;
        }

        [HttpPost("createStudent")]
        [Authorize]
        public async Task<IActionResult> createStudentAsync(StudentCreationReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.createStudentAsync(obj);

            return Ok(result);
        }

        [HttpPost("addStudentToExistingParent")]
        [Authorize]
        public async Task<IActionResult> addStudentToExistingParentAsync(StudentParentExistCreationReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.addStudentToExistingParentAsync(obj);

            return Ok(result);
        }

        [HttpPost("studentLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> studentLoginAsync(StudentLoginReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.studentLoginAsync(obj);

            return Ok(result);
        }

        [HttpGet("studentById")]
        [Authorize]
        public async Task<IActionResult> getStudentByIdAsync(Guid studentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.getStudentByIdAsync(studentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpPost("assignStudentToClass")]
        [Authorize]
        public async Task<IActionResult> assignStudentToClassAsync(AssignStudentToClassReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.assignStudentToClassAsync(obj);

            return Ok(result);
        }

        [HttpGet("studentParent")]
        [Authorize]
        public async Task<IActionResult> getStudentParentAsync(Guid studentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.getStudentParentAsync(studentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("assignedStudent")]
        [Authorize]
        public async Task<IActionResult> getAllAssignedStudentAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.getAllAssignedStudentAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("unAssignedStudent")]
        [Authorize]
        public async Task<IActionResult> getAllUnAssignedStudentAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.getAllUnAssignedStudentAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("studentInSchool")]
        [Authorize]
        public async Task<IActionResult> getAllStudentInSchoolAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.getAllStudentInSchoolAsync(schoolId);

            return Ok(result);
        }


        [HttpGet("studentInCampus")]
        [Authorize]
        public async Task<IActionResult> getAllStudentInCampusAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.getAllStudentInCampusAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("studentsBySessionId")]
        [Authorize]
        public async Task<IActionResult> getStudentsBySessionIdAsync(long schoolId, long campusId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.getStudentsBySessionIdAsync(schoolId, campusId, sessionId);

            return Ok(result);
        }


        [HttpPost("moveStudentToNewClassAndClassGrade")]
        [Authorize]
        public async Task<IActionResult> moveStudentToNewClassAndClassGradeAsync(MoveStudentReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.moveStudentToNewClassAndClassGradeAsync(obj);

            return Ok(result);
        }


        [HttpPost("bulkCreationOfStudent")]
        [Authorize]
        public async Task<IActionResult> createStudentFromExcelAsync([FromForm] BulkStudentReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.createStudentFromExcelAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateStudentDetails")]
        [Authorize]
        public async Task<IActionResult> updateStudentDetailsAsync(Guid studentId, UpdateStudentReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.updateStudentDetailsAsync(studentId, obj);

            return Ok(result);
        }


        [HttpDelete("deleteStudentsAssignedToClass")]
        [Authorize]
        public async Task<IActionResult> deleteStudentsAssignedToClassAsync(DeleteStudentAssignedReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.deleteStudentsAssignedToClassAsync(obj);

            return Ok(result);
        }

        [HttpDelete("deleteStudent")]
        [Authorize]
        public async Task<IActionResult> deleteStudentAsync(Guid studentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.deleteStudentAsync(studentId, schoolId, campusId);

            return Ok(result);
        }

        //***********************-----------------STUDENT DUPLICATE---------------------------------------------

        [HttpGet("studentDuplicates")]
        [Authorize]
        public async Task<IActionResult> getAllStudentDuplicatesAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.getAllStudentDuplicatesAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("studentDuplicateByStudentId")]
        [Authorize]
        public async Task<IActionResult> getStudentDuplicateByStudentIdAsync(Guid studentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.getStudentDuplicateByStudentIdAsync(studentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpPut("updateStudentDuplicate")]
        [Authorize]
        public async Task<IActionResult> updateStudentDuplicateAsync(StudentDuplicateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.updateStudentDuplicateAsync(obj);

            return Ok(result);
        }

        [HttpPut("deleteStudentDuplicate")]
        [Authorize]
        public async Task<IActionResult> deleteStudentDuplicateAsync(Guid studentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.deleteStudentDuplicateAsync(studentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpPost("forgotPassword")]
        [Authorize]
        public async Task<IActionResult> forgotPasswordAsync(string admissionNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.forgotPasswordAsync(admissionNumber);

            return Ok(result);
        }

        [HttpPut("changePassword")]
        [Authorize]
        public async Task<IActionResult> changePasswordAsync(string admissionNumber, string oldPassword, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.changePasswordAsync(admissionNumber, oldPassword, newPassword);

            return Ok(result);
        }

        [HttpGet("studentWithoutParentsInfo")]
        [Authorize]
        public async Task<IActionResult> getAllStudentWithoutParentsInfoAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _studentRepo.getAllStudentWithoutParentsInfoAsync(schoolId, campusId);

            return Ok(result);
        }
    }
}
