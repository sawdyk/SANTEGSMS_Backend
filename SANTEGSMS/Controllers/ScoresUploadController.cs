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
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ScoresUploadController : ControllerBase
    {
        private readonly IScoreUploadRepo _scoreUploadRepo;

        public ScoresUploadController(IScoreUploadRepo scoreUploadRepo)
        {
            _scoreUploadRepo = scoreUploadRepo;
        }

        [HttpPost("uploadScores")]
        [Authorize]
        public async Task<IActionResult> uploadScoresAsync([FromBody] UploadSubjectScoreReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.uploadScoresAsync(obj);

            return Ok(result);
        }

        [HttpGet("scoresBySubjectId")]
        [Authorize]
        public async Task<IActionResult> getScoresBySubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.getScoresBySubjectIdAsync(schoolId, campusId, classId, classGradeId, subjectId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("scoresByStudentIdAndSubjectId")]
        [Authorize]
        public async Task<IActionResult> getScoresByStudentIdAndSubjectIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.getScoresByStudentIdAndSubjectIdAsync(studentId, schoolId, campusId, classId, classGradeId, subjectId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("allScoresByStudentId")]
        [Authorize]
        public async Task<IActionResult> getAllScoresByStudentIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.getAllScoresByStudentIdAsync(studentId, schoolId, campusId, classId, classGradeId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpPut("updateScores")]
        [Authorize]
        public async Task<IActionResult> updateScoresAsync([FromBody] UploadSubjectScoreReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.updateScoresAsync(obj);

            return Ok(result);
        }

        [HttpPost("uploadSingleStudentScore")]
        [Authorize]
        public async Task<IActionResult> uploadSingleStudentScoreAsync([FromBody] UploadScorePerSubjectAndStudentReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.uploadSingleStudentScoreAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateSingleStudentScores")]
        [Authorize]
        public async Task<IActionResult> updateSingleStudentScoresAsync([FromBody] UploadScorePerSubjectAndStudentReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.updateSingleStudentScoresAsync(obj);

            return Ok(result);
        }

        [HttpDelete("deleteScoresPerSubjectForSingleStudent")]
        [Authorize]
        public async Task<IActionResult> deleteScoresPerSubjectForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.deleteScoresPerSubjectForSingleStudentAsync(studentId, schoolId, campusId, classId, classGradeId, subjectId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }


        [HttpDelete("deleteScoresPerSubjectForAllStudent")]
        [Authorize]
        public async Task<IActionResult> deleteScoresPerSubjectForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.deleteScoresPerSubjectForAllStudentAsync(schoolId, campusId, classId, classGradeId, subjectId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteScoresPerCategoryForSingleStudent")]
        [Authorize]
        public async Task<IActionResult> deleteScoresPerCategoryForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.deleteScoresPerCategoryForSingleStudentAsync(studentId, schoolId, campusId, classId, classGradeId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }


        [HttpDelete("deleteScoresPerCategoryForAllStudent")]
        [Authorize]
        public async Task<IActionResult> deleteScoresPerCategoryForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.deleteScoresPerCategoryForAllStudentAsync(schoolId, campusId, classId, classGradeId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpPost("studentAndSubjectScoresExtended")]
        [Authorize]
        public async Task<IActionResult> getAllStudentAndSubjectScoresExtendedAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId, IList<SubjectId> subjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.getAllStudentAndSubjectScoresExtendedAsync(schoolId, campusId, classId, classGradeId, categoryId, subCategoryId, termId, sessionId, subjectId);

            return Ok(result);
        }

        [HttpPost("createScoreUploadSheetTemplate")]
        [Authorize]
        public async Task<IActionResult> createScoreUploadSheetTemplateAsync([FromBody] ScoreUploadSheetTemplateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.createScoreUploadSheetTemplateAsync(obj);

            return Ok(result);
        }

        [HttpGet("scoreSheetTemplateById")]
        [Authorize]
        public async Task<IActionResult> getScoreSheetTemplateByIdAsync(long scoreSheetTemplateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.getScoreSheetTemplateByIdAsync(scoreSheetTemplateId);

            return Ok(result);
        }

        [HttpGet("usedScoreSheetTemplate")]
        [Authorize]
        public async Task<IActionResult> getAllUsedScoreSheetTemplateAsync(long schoolId, long campusId, long classId, long classGradeId, Guid teacherId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.getAllUsedScoreSheetTemplateAsync(schoolId, campusId, classId, classGradeId, teacherId);

            return Ok(result);
        }

        [HttpGet("unUsedScoreSheetTemplate")]
        [Authorize]
        public async Task<IActionResult> getAllUnUsedScoreSheetTemplateAsync(long schoolId, long campusId, long classId, long classGradeId, Guid teacherId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.getAllUnUsedScoreSheetTemplateAsync(schoolId, campusId, classId, classGradeId, teacherId);

            return Ok(result);
        }

        [HttpPost("bulkScoresUpload")]
        [Authorize]
        public async Task<IActionResult> bulkScoresUploadAsync([FromForm] BulkScoresUploadReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.bulkScoresUploadAsync(obj);

            return Ok(result);
        }

        [HttpGet("studentGradeBookScoresPerSubjectAndCategory")]
        [Authorize]
        public async Task<IActionResult> studentGradeBookScoresPerSubjectAndCategoryAsync(Guid studentId, long schoolId, long campusId, long categoryId, long subCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoreUploadRepo.studentGradeBookScoresPerSubjectAndCategoryAsync(studentId, schoolId, campusId, categoryId, subCategoryId);

            return Ok(result);
        }
    }
}