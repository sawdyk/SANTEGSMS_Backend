using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SANTEGSMS.IRepos;
using SANTEGSMS.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentRepo _assignmentRepo;

        public AssignmentsController(IAssignmentRepo assignmentRepo)
        {
            _assignmentRepo = assignmentRepo;
        }

        //--------------------------------------------------------------ASSIGNMENTS------------------------------------------------------------------------------------------------------

        [HttpPost("createAssignment")]
        [Authorize]
        public async Task<IActionResult> createAssignmentAsync(AssignmentCreationReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.createAssignmentAsync(obj);

            return Ok(result);
        }

        [HttpGet("assignmentById")]
        [Authorize]
        public async Task<IActionResult> getAssignmentByIdAsync(long assignmentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAssignmentByIdAsync(assignmentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("assignmentBySubjectId")]
        [Authorize]
        public async Task<IActionResult> getAssignmentBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAssignmentBySubjectIdAsync(subjectId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpPut("updateAssignment")]
        [Authorize]
        public async Task<IActionResult> updateAssignmentAsync(long assignmentId, AssignmentCreationReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.updateAssignmentAsync(assignmentId, obj);

            return Ok(result);
        }


        [HttpDelete("deleteAssignment")]
        [Authorize]
        public async Task<IActionResult> deleteAssignmentAsync(long assignmentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.deleteAssignmentAsync(assignmentId, schoolId, campusId);

            return Ok(result);
        }


        //-----------------------------------------------SUBMIT AND GRADE ASSIGNMENTS-------------------------------------------------

        [HttpPost("submitAssignment")]
        [Authorize]
        public async Task<IActionResult> submitAssignmentAsync(SubmitAssignmentReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.submitAssignmentAsync(obj);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentById")]
        [Authorize]
        public async Task<IActionResult> getSubmittedAssignmentByIdAsync(long assignmentSubmittedId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getSubmittedAssignmentByIdAsync(assignmentSubmittedId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentsByAssignmentId")]
        [Authorize]
        public async Task<IActionResult> getAllSubmittedAssignmentsByAssignmentIdAsync(long classId, long classGradeId, long assignmentId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllSubmittedAssignmentsByAssignmentIdAsync(classId, classGradeId, assignmentId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentsByStudentIdAndAssignmentId")]
        [Authorize]
        public async Task<IActionResult> getAllSubmittedAssignmentsByStudentIdAndAssignmentIdAsync(Guid studentId, long classId, long classGradeId, long assignmentId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllSubmittedAssignmentsByStudentIdAndAssignmentIdAsync(studentId, classId, classGradeId, assignmentId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("unSubmittedAssignmentsByStudentId")]
        [Authorize]
        public async Task<IActionResult> getAllUnSubmittedAssignmentsByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllUnSubmittedAssignmentsByStudentIdAsync(studentId, classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("unSubmittedAssignmentsByIndividualStudentId")]
        [Authorize]
        public async Task<IActionResult> getAllUnSubmittedAssignmentsByIndividualStudentIdAsync(Guid studentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllUnSubmittedAssignmentsByIndividualStudentIdAsync(studentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentsByIndividualStudentId")]
        [Authorize]
        public async Task<IActionResult> getSubmittedAssignmentsByIndividualStudentIdAsync(Guid studentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getSubmittedAssignmentsByIndividualStudentIdAsync(studentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpPut("updateSubmittedAssignments")]
        [Authorize]
        public async Task<IActionResult> updateSubmittedAssignmentsAsync(long assignmentSubmittedId, SubmitAssignmentReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.updateSubmittedAssignmentsAsync(assignmentSubmittedId, obj);

            return Ok(result);
        }


        [HttpDelete("deleteSubmittedAssignments")]
        [Authorize]
        public async Task<IActionResult> deleteSubmittedAssignmentsAsync(long assignmentSubmittedId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.deleteSubmittedAssignmentsAsync(assignmentSubmittedId, schoolId, campusId);

            return Ok(result);
        }

        [HttpPut("gradeSubmittedAssignments")]
        [Authorize]
        public async Task<IActionResult> gradeSubmittedAssignmentsAsync(GradeAssignmentsReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.gradeSubmittedAssignmentsAsync(obj);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentsBySubjectId")]
        [Authorize]
        public async Task<IActionResult> getSubmittedAssignmentsBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getSubmittedAssignmentsBySubjectIdAsync(subjectId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }


        [HttpGet("unSubmittedAssignmentsByClassIdAndClassGradeId")]
        [Authorize]
        public async Task<IActionResult> getUnSubmittedAssignmentsByClassIdAndClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getUnSubmittedAssignmentsByClassIdAndClassGradeIdAsync(classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentsByClassIdAndClassGradeId")]
        [Authorize]
        public async Task<IActionResult> getSubmittedAssignmentsByClassIdAndClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getSubmittedAssignmentsByClassIdAndClassGradeIdAsync(classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }


        [HttpGet("submittedAssignmentsByStudentId")]
        [Authorize]
        public async Task<IActionResult> getAllSubmittedAssignmentsByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllSubmittedAssignmentsByStudentIdAsync(studentId, classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("assignmentByTeacherId")]
        [Authorize]
        public async Task<IActionResult> getAssignmentByTeacherIdAsync(Guid teacherId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAssignmentByTeacherIdAsync(teacherId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentsByTeacherId")]
        [Authorize]
        public async Task<IActionResult> getAllSubmittedAssignmentsByTeacherIdAsync(Guid teacherId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllSubmittedAssignmentsByTeacherIdAsync(teacherId, classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentsByTeacherIdAndSubjectId")]
        [Authorize]
        public async Task<IActionResult> getAllSubmittedAssignmentsByTeacherIdAndSubjectIdAsync(Guid teacherId, long subjectId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllSubmittedAssignmentsByTeacherIdAndSubjectIdAsync(teacherId, subjectId, classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }
    }
}
