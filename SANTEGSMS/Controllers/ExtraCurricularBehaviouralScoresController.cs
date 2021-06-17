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
    public class ExtraCurricularBehaviouralScoresController : ControllerBase
    {
        private readonly IExtraCurricularBehavioralScoresRepo _extraCurricularBehavioralScoresRepo;

        public ExtraCurricularBehaviouralScoresController(IExtraCurricularBehavioralScoresRepo extraCurricularBehavioralScoresRepo)
        {
            _extraCurricularBehavioralScoresRepo = extraCurricularBehavioralScoresRepo;
        }

        [HttpPost("uploadExtraCurricularBehavioralScores")]
        [Authorize]
        public async Task<IActionResult> uploadExtraCurricularBehavioralScoresAsync([FromBody] UploadScoreReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _extraCurricularBehavioralScoresRepo.uploadExtraCurricularBehavioralScoresAsync(obj);

            return Ok(result);
        }

        [HttpPost("uploadSingleStudentExtraCurricularBehavioralScore")]
        [Authorize]
        public async Task<IActionResult> uploadSingleStudentExtraCurricularBehavioralScoreAsync([FromBody] UploadScorePerStudentReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _extraCurricularBehavioralScoresRepo.uploadSingleStudentExtraCurricularBehavioralScoreAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateExtraCurricularBehavioralScores")]
        [Authorize]
        public async Task<IActionResult> updateExtraCurricularBehavioralScoresAsync([FromBody] UploadScoreReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _extraCurricularBehavioralScoresRepo.updateExtraCurricularBehavioralScoresAsync(obj);

            return Ok(result);
        }

        [HttpGet("extraCurricularBehavioralScores")]
        [Authorize]
        public async Task<IActionResult> getExtraCurricularBehavioralScoresAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _extraCurricularBehavioralScoresRepo.getExtraCurricularBehavioralScoresAsync(schoolId, campusId, classId, classGradeId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("extraCurricularBehavioralScoresByStudentIdAndCategoryId")]
        [Authorize]
        public async Task<IActionResult> getExtraCurricularBehavioralScoresByStudentIdAndCategoryIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _extraCurricularBehavioralScoresRepo.getExtraCurricularBehavioralScoresByStudentIdAndCategoryIdAsync(studentId, schoolId, campusId, classId, classGradeId, categoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("extraCurricularBehavioralScoresByStudentId")]
        [Authorize]
        public async Task<IActionResult> getExtraCurricularBehavioralScoresByStudentIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _extraCurricularBehavioralScoresRepo.getExtraCurricularBehavioralScoresByStudentIdAsync(studentId, schoolId, campusId, classId, classGradeId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteExtraCurricularBehavioralScoresForAllStudent")]
        [Authorize]
        public async Task<IActionResult> deleteExtraCurricularBehavioralScoresForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _extraCurricularBehavioralScoresRepo.deleteExtraCurricularBehavioralScoresForAllStudentAsync(schoolId, campusId, classId, classGradeId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteExtraCurricularBehavioralScoresForSingleStudent")]
        [Authorize]
        public async Task<IActionResult> deleteExtraCurricularBehavioralScoresForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _extraCurricularBehavioralScoresRepo.deleteExtraCurricularBehavioralScoresForSingleStudentAsync(studentId, schoolId, campusId, classId, classGradeId, categoryId, subCategoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteExtraCurricularBehavioralScoresPerCategoryForAllStudent")]
        [Authorize]
        public async Task<IActionResult> deleteExtraCurricularBehavioralScoresPerCategoryForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _extraCurricularBehavioralScoresRepo.deleteExtraCurricularBehavioralScoresPerCategoryForAllStudentAsync(schoolId, campusId, classId, classGradeId, categoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteExtraCurricularBehavioralScoresPerCategoryForSingleStudent")]
        [Authorize]
        public async Task<IActionResult> deleteExtraCurricularBehavioralScoresPerCategoryForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _extraCurricularBehavioralScoresRepo.deleteExtraCurricularBehavioralScoresPerCategoryForSingleStudentAsync(studentId, schoolId, campusId, classId, classGradeId, categoryId, termId, sessionId);

            return Ok(result);
        }
    }
}