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

        [HttpPost("reportCardDataByStudentIdAndPin")]
        [Authorize]
        public async Task<IActionResult> getReportCardDataByStudentIdAndPinAsync(ReportCardDataWithPinReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardDataGenerateRepo.getReportCardDataByStudentIdAndPinAsync(obj);

            return Ok(result);
        }


        //----------------------------REPORT CARD PIN GENERATION----------------------------------------------------

        [HttpPost("generatePins")]
        [Authorize]
        public async Task<IActionResult> generatePinsAsync(PinCreateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardRepo.generatePinsAsync(obj);

            return Ok(result);
        }

        [HttpGet("pinById")]
        [Authorize]
        public async Task<IActionResult> getPinByIdAsync(long pinId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardRepo.getPinByIdAsync(pinId);

            return Ok(result);
        }

        [HttpGet("pins")]
        [Authorize]
        public async Task<IActionResult> getAllPinsAsync(long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardRepo.getAllPinsAsync(schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

       
        [HttpGet("pinsByStatus")]
        [Authorize]
        public async Task<IActionResult> getPinsByStatusAsync(long schoolId, long campusId, long termId, long sessionId, bool isUsed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardRepo.getPinsByStatusAsync(schoolId, campusId, termId, sessionId, isUsed);

            return Ok(result);
        }

        [HttpDelete("deletePins")]
        [Authorize]
        public async Task<IActionResult> deletePinsAsync(long pinId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardRepo.deletePinsAsync(pinId);

            return Ok(result);
        }
    }
}
