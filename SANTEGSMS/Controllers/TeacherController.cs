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
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepo _teacherRepo;

        public TeacherController(ITeacherRepo teacherRepo)
        {
            _teacherRepo = teacherRepo;
        }

        //Creates a new Teacher
        [HttpPost("createTeacher")]
        [Authorize]
        public async Task<IActionResult> createTeacher([FromBody] TeacherCreateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.createTeacherAsync(obj);

            return Ok(result);
        }

        //
        [HttpGet("teacherById")]
        [Authorize]
        public async Task<IActionResult> getTeacherByIdAsync(Guid teacherId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getTeacherByIdAsync(teacherId, schoolId, campusId);

            return Ok(result);
        }


        //
        [HttpGet("teachers")]
        [Authorize]
        public async Task<IActionResult> getAllTeachersAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getAllTeachersAsync(schoolId, campusId);

            return Ok(result);
        }

        //
        [HttpGet("teacherRoles")]
        [Authorize]
        public async Task<IActionResult> getTeacherRolesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getTeacherRolesAsync();

            return Ok(result);
        }

        [HttpGet("teachersByRoleId")]
        [Authorize]
        public async Task<IActionResult> getAllTeachersByRoleIdAsync(long roleId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getAllTeachersByRoleIdAsync(roleId, schoolId, campusId);

            return Ok(result);
        }

        //Assign a Teacher to a Class
        [HttpPost("assignTeacherToClassGrade")]
        [Authorize]
        public async Task<IActionResult> assignTeacherToClassGradeAsync(AssignTeacherToClassReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.assignTeacherToClassGradeAsync(obj);

            return Ok(result);
        }

        [HttpGet("classGradeAssignedToTeacher")]
        [Authorize]
        public async Task<IActionResult> getAllClassGradeAssignedToTeacherAsync(long schoolId, long campusId, Guid teacherId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getAllClassGradeAssignedToTeacherAsync(schoolId, campusId, teacherId);

            return Ok(result);
        }

        [HttpGet("assignedTeachers")]
        [Authorize]
        public async Task<IActionResult> getAssignedTeachersAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getAssignedTeachersAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("rolesAssignedToTeacher")]
        [Authorize]
        public async Task<IActionResult> getAllRolesAssignedToTeacherAsync(Guid teacherId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getAllRolesAssignedToTeacherAsync(teacherId, schoolId, campusId);

            return Ok(result);
        }


        //--------------------------------------ATTENDANCE---------------------------------------------------------------------------

        [HttpPost("takeClassAttendance")]
        [Authorize]
        public async Task<IActionResult> takeClassAttendanceAsync(TakeAttendanceReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.takeClassAttendanceAsync(obj);

            return Ok(result);
        }

        [HttpGet("classAttendance")]
        [Authorize]
        public async Task<IActionResult> getClassAttendanceAsync(long classId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getClassAttendanceAsync(classId, attendanceDate, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("classGradeAttendance")]
        [Authorize]
        public async Task<IActionResult> getClassGradeAttendanceAsync(long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getClassGradeAttendanceAsync(classId, classGradeId, attendanceDate, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("classAttendanceByPeriodId")]
        [Authorize]
        public async Task<IActionResult> getClassAttendanceByPeriodIdAsync(long classId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getClassAttendanceByPeriodIdAsync(classId, attendanceDate, schoolId, campusId, periodId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("classGradeAttendanceByPeriodId")]
        [Authorize]
        public async Task<IActionResult> getClassGradeAttendanceByPeriodIdAsync(long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getClassGradeAttendanceByPeriodIdAsync(classId, classGradeId, attendanceDate, schoolId, campusId, periodId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("studentAttendance")]
        [Authorize]
        public async Task<IActionResult> getStudentAttendanceAsync(Guid studentId, long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getStudentAttendanceAsync(studentId, classId, classGradeId, attendanceDate, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("studentAttendanceByPeriodId")]
        [Authorize]
        public async Task<IActionResult> getStudentAttendanceByPeriodIdAsync(Guid studentId, long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _teacherRepo.getStudentAttendanceByPeriodIdAsync(studentId, classId, classGradeId, attendanceDate, schoolId, campusId, periodId, termId, sessionId);

            return Ok(result);
        }
    }
}
