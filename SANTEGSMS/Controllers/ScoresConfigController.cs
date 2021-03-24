using Microsoft.AspNetCore.Authorization;
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
    public class ScoresConfigController : ControllerBase
    {
        private readonly IScoresConfigRepo _scoresConfigRepo;

        public ScoresConfigController(IScoresConfigRepo scoresConfigRepo)
        {
            _scoresConfigRepo = scoresConfigRepo;
        }

        [HttpGet("scoreCategory")]
        [Authorize]
        public async Task<IActionResult> getAllScoreCategoryAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.getAllScoreCategoryAsync();

            return Ok(result);
        }

        [HttpGet("scoreCategoryById")]
        [Authorize]
        public async Task<IActionResult> getScoreCategoryByIdAsync(long scoreCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.getScoreCategoryByIdAsync(scoreCategoryId);

            return Ok(result);
        }

        [HttpPost("createScoreGrades")]
        [Authorize]
        public async Task<IActionResult> createScoreGradesAsync(ScoreGradeCreateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.createScoreGradesAsync(obj);

            return Ok(result);
        }


        [HttpGet("scoreGrades")]
        [Authorize]
        public async Task<IActionResult> getAllScoreGradesAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.getAllScoreGradesAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("scoreGradeByClassId")]
        [Authorize]
        public async Task<IActionResult> getScoreGradeByClassIdAsync(long classId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.getScoreGradeByClassIdAsync(classId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("scoreGradeById")]
        [Authorize]
        public async Task<IActionResult> getScoreGradeByIdAsync(long scoreGradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.getScoreGradeByIdAsync(scoreGradeId);

            return Ok(result);
        }

        [HttpPut("updateScoreGrade")]
        [Authorize]
        public async Task<IActionResult> updateScoreGradeAsync(long scoreGradeId, ScoreGradeCreateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.updateScoreGradeAsync(scoreGradeId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteScoreGrade")]
        [Authorize]
        public async Task<IActionResult> deleteScoreGradeAsync(long scoreGradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.deleteScoreGradeAsync(scoreGradeId);

            return Ok(result);
        }

        //------------------------Score Category Configuration---------------------------------------------------------//

        [HttpPost("createScoreCategoryConfig")]
        [Authorize]
        public async Task<IActionResult> createScoreCategoryConfigAsync(ScoreCategoryConfigReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.createScoreCategoryConfigAsync(obj);

            return Ok(result);
        }

        [HttpGet("scoreCategoryConfig")]
        [Authorize]
        public async Task<IActionResult> getAllScoreCategoryConfigAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.getAllScoreCategoryConfigAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("scoreCategoryConfigById")]
        [Authorize]
        public async Task<IActionResult> getScoreCategoryConfigByIdAsync(long scoreCategoryConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.getScoreCategoryConfigByIdAsync(scoreCategoryConfigId);

            return Ok(result);
        }

        [HttpPut("updateScoreCategoryConfig")]
        [Authorize]
        public async Task<IActionResult> updateScoreCategoryConfigAsync(long scoreCategoryConfigId, ScoreCategoryConfigReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.updateScoreCategoryConfigAsync(scoreCategoryConfigId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteScoreCategoryConfig")]
        [Authorize]
        public async Task<IActionResult> deleteScoreCategoryConfigAsync(long scoreCategoryConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.deleteScoreCategoryConfigAsync(scoreCategoryConfigId);

            return Ok(result);
        }

        //------------------------Score SubCategory Configuration---------------------------------------------------------//

        [HttpPost("createScoreSubCategoryConfig")]
        [Authorize]
        public async Task<IActionResult> createScoreSubCategoryConfigAsync(ScoreSubCategoryConfigReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.createScoreSubCategoryConfigAsync(obj);

            return Ok(result);
        }

        [HttpGet("scoreSubCategoryConfig")]
        [Authorize]
        public async Task<IActionResult> getAllScoreSubCategoryConfigAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.getAllScoreSubCategoryConfigAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("scoreSubCategoryConfigById")]
        [Authorize]
        public async Task<IActionResult> getScoreSubCategoryConfigByIdAsync(long scoreSubCategoryConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.getScoreSubCategoryConfigByIdAsync(scoreSubCategoryConfigId);

            return Ok(result);
        }

        [HttpPut("updateScoreSubCategoryConfig")]
        [Authorize]
        public async Task<IActionResult> updateScoreSubCategoryConfigAsync(long scoreSubCategoryConfigId, ScoreSubCategoryConfigReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.updateScoreSubCategoryConfigAsync(scoreSubCategoryConfigId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteScoreSubCategoryConfig")]
        [Authorize]
        public async Task<IActionResult> deleteScoreSubCategoryConfigAsync(long scoreSubCategoryConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _scoresConfigRepo.deleteScoreSubCategoryConfigAsync(scoreSubCategoryConfigId);

            return Ok(result);
        }
    }
}
