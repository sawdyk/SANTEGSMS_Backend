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
    public class LessonNoteController : ControllerBase
    {
        private readonly ILessonNoteRepo _lessonNoteRepo;

        public LessonNoteController(ILessonNoteRepo lessonNoteRepo)
        {
            _lessonNoteRepo = lessonNoteRepo;
        }

        [HttpPost("createLessonNotes")]
        [Authorize]
        public async Task<IActionResult> createLessonNotesAsync([FromBody] LessonNoteCreateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.createLessonNotesAsync(obj);

            return Ok(result);
        }

        [HttpGet("lessonNotesById")]
        [Authorize]
        public async Task<IActionResult> getLessonNotesByIdAsync(long lessonNoteId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.getLessonNotesByIdAsync(lessonNoteId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("lessonNotesByClassGradeId")]
        [Authorize]
        public async Task<IActionResult> getLessonNotesByClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.getLessonNotesByClassGradeIdAsync(classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("lessonNotesBySubjectId")]
        [Authorize]
        public async Task<IActionResult> getLessonNotesBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.getLessonNotesBySubjectIdAsync(subjectId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("lessonNotesByTeacherId")]
        [Authorize]
        public async Task<IActionResult> getLessonNotesByTeacherIdAsync(Guid teacherId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.getLessonNotesByTeacherIdAsync(teacherId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpPut("updateLessonNotes")]
        [Authorize]
        public async Task<IActionResult> updateLessonNotesAsync(long lessonNoteId, LessonNoteCreateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.updateLessonNotesAsync(lessonNoteId, obj);

            return Ok(result);
        }

        [HttpPut("approveLessonNotes")]
        [Authorize]
        public async Task<IActionResult> approveLessonNotesAsync(long lessonNoteId, long statusId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.approveLessonNotesAsync(lessonNoteId, statusId, schoolId, campusId);

            return Ok(result);
        }

        [HttpDelete("deleteLessonNotes")]
        [Authorize]
        public async Task<IActionResult> deleteLessonNotesAsync(long lessonNoteId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.deleteLessonNotesAsync(lessonNoteId, schoolId, campusId);

            return Ok(result);
        }


        //------------------------------------------------------------------------SUBJECTNOTES-------------------------------------------------------------------------------------------------------

        [HttpPost("createSubjectNotes")]
        [Authorize]
        public async Task<IActionResult> createSubjectNotesAsync([FromBody] SubjectNoteCreateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.createSubjectNotesAsync(obj);

            return Ok(result);
        }

        [HttpGet("subjectNotesById")]
        [Authorize]
        public async Task<IActionResult> getSubjectNotesByIdAsync(long subjectNoteId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.getSubjectNotesByIdAsync(subjectNoteId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("subjectNotesByClassGradeId")]
        [Authorize]
        public async Task<IActionResult> getSubjectNotesByClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.getSubjectNotesByClassGradeIdAsync(classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("subjectNotesBySubjectId")]
        [Authorize]
        public async Task<IActionResult> getSubjectNotesBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.getSubjectNotesBySubjectIdAsync(subjectId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("subjectNotesByTeacherId")]
        [Authorize]
        public async Task<IActionResult> getSubjectNotesByTeacherIdAsync(Guid teacherId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.getSubjectNotesByTeacherIdAsync(teacherId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpPut("updateSubjectNotes")]
        [Authorize]
        public async Task<IActionResult> updateSubjectNotesAsync(long subjectNoteId, SubjectNoteCreateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.updateSubjectNotesAsync(subjectNoteId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteSubjectNotes")]
        [Authorize]
        public async Task<IActionResult> deleteSubjectNotesAsync(long subjectNoteId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _lessonNoteRepo.deleteSubjectNotesAsync(subjectNoteId, schoolId, campusId);

            return Ok(result);
        }
    }
}