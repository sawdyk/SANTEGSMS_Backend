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
    public class BroadSheetController : ControllerBase
    {
        private readonly IBroadSheetRepo _broadSheetRepo;
        public BroadSheetController(IBroadSheetRepo broadSheetRepo)
        {
            _broadSheetRepo = broadSheetRepo;
        }

        [HttpPost("createBroadsheetGradingConfig")]
        [Authorize]
        public async Task<IActionResult> createBroadsheetGradingConfigAsync(BroadsheetGradeReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _broadSheetRepo.createBroadsheetGradingConfigAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateBroadsheetGradingConfig")]
        [Authorize]
        public async Task<IActionResult> updateBroadsheetGradingConfigAsync(BroadsheetGradeReqModel obj, long broadSheetConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _broadSheetRepo.updateBroadsheetGradingConfigAsync(obj, broadSheetConfigId);

            return Ok(result);
        }

        [HttpGet("broadsheetGradingConfigById")]
        [Authorize]
        public async Task<IActionResult> getBroadsheetGradingConfigByIdAsync(long broadSheetConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _broadSheetRepo.getBroadsheetGradingConfigByIdAsync(broadSheetConfigId);

            return Ok(result);
        }

        [HttpGet("broadsheetGradingConfig")]
        [Authorize]
        public async Task<IActionResult> getBroadsheetGradingConfigAsync(long schoolId, long campusId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _broadSheetRepo.getBroadsheetGradingConfigAsync(schoolId, campusId, sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteBroadsheetGradingConfig")]
        [Authorize]
        public async Task<IActionResult> deleteBroadsheetGradingConfigAsync(long broadSheetConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _broadSheetRepo.deleteBroadsheetGradingConfigAsync(broadSheetConfigId);

            return Ok(result);
        }


        [HttpPost("createBroadsheetRemarkConfig")]
        [Authorize]
        public async Task<IActionResult> createBroadsheetRemarkConfigAsync(BroadSheetRemarkReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _broadSheetRepo.createBroadsheetRemarkConfigAsync(obj);

            return Ok(result);
        }

        [HttpGet("broadsheetRemarkConfig")]
        [Authorize]
        public async Task<IActionResult> getBroadsheetRemarkConfigAsync(long schoolId, long campusId, long classId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _broadSheetRepo.getBroadsheetRemarkConfigAsync(schoolId, campusId, classId);

            return Ok(result);
        }

        [HttpPost("generateClassBroadsheet")]
        [Authorize]
        public async Task<IActionResult> generateClassBroadsheetAsync(long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _broadSheetRepo.generateClassBroadsheetAsync(schoolId, campusId, classId, classGradeId, termId, sessionId);

            return Ok(result);
        }

        [HttpPost("generatePerformanceAnalysisSheet")]
        [Authorize]
        public async Task<IActionResult> generatePerformanceAnalysisSheetAsync(PerformanceAnalysisReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _broadSheetRepo.generatePerformanceAnalysisSheetAsync(obj);

            return Ok(result);
        }


    }
}
