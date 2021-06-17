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
    public class ReportCardController : ControllerBase
    {
        private readonly IReportCardRepo _reportCardRepo;
        private readonly IReportCardDataGenerateRepo _reportCardDataGenerateRepo;

        public ReportCardController(IReportCardRepo reportCardRepo, IReportCardDataGenerateRepo reportCardDataGenerateRepo)
        {
            _reportCardRepo = reportCardRepo;
            _reportCardDataGenerateRepo = reportCardDataGenerateRepo;
        }

        [HttpPost("computeResultAndSubjectPosition")]
        [Authorize]
        public async Task<IActionResult> computeResultAndSubjectPositionAsync(ComputeResultPositionReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardRepo.computeResultAndSubjectPositionAsync(obj);

            return Ok(result);
        }

        [HttpGet("computedResult")]
        [Authorize]
        public async Task<IActionResult> getAllComputedResultAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardRepo.getAllComputedResultAsync(classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computedResultByStudentId")]
        [Authorize]
        public async Task<IActionResult> getComputedResultByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardRepo.getComputedResultByStudentIdAsync(studentId, classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteComputedResultByStudentId")]
        [Authorize]
        public async Task<IActionResult> deleteComputedResultByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardRepo.deleteComputedResultByStudentIdAsync(studentId, classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteAllComputedResult")]
        [Authorize]
        public async Task<IActionResult> deleteAllComputedResultAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardRepo.deleteAllComputedResultAsync(classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpPost("reportCardDataByStudentId")]
        [Authorize]
        public async Task<IActionResult> getReportCardDataByStudentIdAsync(ReportCardDataReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardDataGenerateRepo.getReportCardDataByStudentIdAsync(obj);

            return Ok(result);
        }
    }
}
