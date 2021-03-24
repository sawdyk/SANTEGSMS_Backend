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
    public class ClassController : ControllerBase
    {
        private readonly IClassRepo _classRepo;

        public ClassController(IClassRepo classRepo)
        {
            _classRepo = classRepo;
        }

        [HttpPost("createClass")]
        [Authorize]
        public async Task<IActionResult> createClass(ClassReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.createClassAsync(obj);

            return Ok(result);
        }

        [HttpPost("createClassGrades")]
        [Authorize]
        public async Task<IActionResult> createClassGrades(ClassGradeReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.createClassGradesAsync(obj);

            return Ok(result);
        }

        [HttpGet("classes")]
        [Authorize]
        public async Task<IActionResult> allClasses(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.getAllClassesAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("classGrades")]
        [Authorize]
        public async Task<IActionResult> allClassGrades(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.getAllClassGradesAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("classByClassId")]
        [Authorize]
        public async Task<IActionResult> classByClassId(long classId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.getClassByClassIdAsync(classId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("classGradesByClassGradeId")]
        [Authorize]
        public async Task<IActionResult> classGradesByClassGradeId(long classGradeId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.getClassGradesByClassGradeIdAsync(classGradeId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("classGradesByClassId")]
        [Authorize]
        public async Task<IActionResult> classGradesByClassId(long classId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.getClassGradesByClassIdAsync(classId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("studentInClass")]
        [Authorize]
        public async Task<IActionResult> getAllStudentInClassAsync(long classId, long schoolId, long campusId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.getAllStudentInClassAsync(classId, schoolId, campusId, sessionId);

            return Ok(result);
        }

        [HttpGet("studentInClassGrade")]
        [Authorize]
        public async Task<IActionResult> getAllStudentInClassGradeAsync(long classId, long classGradeId, long schoolId, long campusId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.getAllStudentInClassGradeAsync(classId, classGradeId, schoolId, campusId, sessionId);

            return Ok(result);
        }

        [HttpGet("studentInClassForCurrentSession")]
        [Authorize]
        public async Task<IActionResult> getAllStudentInClassForCurrentSessionAsync(long classId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.getAllStudentInClassForCurrentSessionAsync(classId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("studentInClassGradeForCurrentSession")]
        [Authorize]
        public async Task<IActionResult> getAllStudentInClassGradeForCurrentSessionAsync(long classId, long classGradeId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.getAllStudentInClassGradeForCurrentSessionAsync(classId, classGradeId, schoolId, campusId);

            return Ok(result);
        }


        [HttpPut("updateClass")]
        [Authorize]
        public async Task<IActionResult> updateClassAsync(long classId, ClassReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.updateClassAsync(classId, obj);

            return Ok(result);
        }

        [HttpPut("updateClassGrade")]
        [Authorize]
        public async Task<IActionResult> updateClassGradeAsync(long classGradeId, ClassGradeReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.updateClassGradeAsync(classGradeId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteClass")]
        [Authorize]
        public async Task<IActionResult> deleteClassAsync(long classId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.deleteClassAsync(classId, schoolId, campusId);

            return Ok(result);
        }

        [HttpDelete("deleteClassGrade")]
        [Authorize]
        public async Task<IActionResult> deleteClassGradeAsync(long classGradeId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _classRepo.deleteClassGradeAsync(classGradeId, schoolId, campusId);

            return Ok(result);
        }
    }
}
