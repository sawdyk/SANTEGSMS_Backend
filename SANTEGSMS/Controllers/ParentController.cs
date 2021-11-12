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
    public class ParentController : ControllerBase
    {
        private readonly IParentRepo _parentRepo;

        public ParentController(IParentRepo parentRepo)
        {
            _parentRepo = parentRepo;
        }

        [HttpPost("parentLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> parentLoginAsync(LoginReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.parentLoginAsync(obj);

            return Ok(result);
        }

        [HttpGet("parentDetailsByEmail")]
        [Authorize]
        public async Task<IActionResult> getParentDetailsByEmailAsync(string email, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getParentDetailsByEmailAsync(email, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("parentDetailsById")]
        [Authorize]
        public async Task<IActionResult> getParentDetailsByIdAsync(Guid parentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getParentDetailsByIdAsync(parentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("parent")]
        [Authorize]
        public async Task<IActionResult> getAllParentAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getAllParentAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("parentChild")]
        [Authorize]
        public async Task<IActionResult> getAllParentChildAsync(Guid parentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getAllParentChildAsync(parentId, schoolId, campusId);

            return Ok(result);
        }
        //-------------------------------------ChildrenProfile-----------------------------------------------
        [HttpPost("childrenProfile")]
        [Authorize]
        public async Task<IActionResult> getChildrenProfileAsync([FromBody] ChildrenProfileReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildrenProfileAsync(obj);

            return Ok(result);
        }
        //-------------------------------------ChildrenAttendance-----------------------------------------------
        [HttpPost("childrenAttendanceBySessionId")]
        [Authorize]
        public async Task<IActionResult> getChildrenAttendanceBySessionIdAsync(IList<Guid> childrenId, Guid parentId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildrenAttendanceBySessionIdAsync(childrenId, parentId, sessionId);

            return Ok(result);
        }

        [HttpPost("childrenAttendanceByTermId")]
        [Authorize]
        public async Task<IActionResult> getChildrenAttendanceByTermIdAsync(IList<Guid> childrenId, Guid parentId, long termId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildrenAttendanceByTermIdAsync(childrenId, parentId, termId);

            return Ok(result);
        }

        [HttpPost("childrenAttendanceByDate")]
        [Authorize]
        public async Task<IActionResult> getChildrenAttendanceByDateAsync(IList<Guid> childrenId, Guid parentId, DateTime fromDate, DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildrenAttendanceByDateAsync(childrenId, parentId, fromDate, toDate);

            return Ok(result);
        }

        //-------------------------------------ChildrenSubject-----------------------------------------------
        [HttpPost("childrenSubject")]
        [Authorize]
        public async Task<IActionResult> getChildrenSubjectAsync(IList<Guid> childrenId, Guid parentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildrenSubjectAsync(childrenId, parentId);

            return Ok(result);
        }
        //-------------------------------------ChildAttendance-----------------------------------------------
        [HttpGet("childAttendanceBySessionId")]
        [Authorize]
        public async Task<IActionResult> getChildAttendanceBySessionIdAsync(Guid childId, Guid parentId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildAttendanceBySessionIdAsync(childId, parentId, sessionId);

            return Ok(result);
        }

        [HttpGet("childAttendanceByTermId")]
        [Authorize]
        public async Task<IActionResult> getChildAttendanceByTermIdAsync(Guid childId, Guid parentId, long termId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildAttendanceByTermIdAsync(childId, parentId, termId);

            return Ok(result);
        }

        [HttpGet("childAttendanceByDate")]
        [Authorize]
        public async Task<IActionResult> getChildAttendanceByDateAsync(Guid childId, Guid parentId, DateTime fromDate, DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildAttendanceByDateAsync(childId, parentId, fromDate, toDate);

            return Ok(result);
        }

        //-------------------------------------ChildSubject-----------------------------------------------
        [HttpGet("childSubject")]
        [Authorize]
        public async Task<IActionResult> getChildSubjectAsync(Guid childId, Guid parentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildSubjectAsync(childId, parentId);

            return Ok(result);
        }

        [HttpPost("forgotPassword")]
        [Authorize]
        public async Task<IActionResult> forgotPasswordAsync(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.forgotPasswordAsync(email);

            return Ok(result);
        }

        [HttpPut("changePassword")]
        [Authorize]
        public async Task<IActionResult> changePasswordAsync(string email, string oldPassword, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.changePasswordAsync(email, oldPassword, newPassword);

            return Ok(result);
        }

        [HttpPut("updateParentDetails")]
        [Authorize]
        public async Task<IActionResult> updateParentDetailsAsync(Guid parentId, UpdateParentReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.updateParentDetailsAsync(parentId, obj);

            return Ok(result);
        }

        [HttpGet("parentInSchoolPerSession")]
        [Authorize]
        public async Task<IActionResult> getAllParentInSchoolPerSessionAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getAllParentInSchoolPerSessionAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("allParentInSchoolPerSession")]
        [Authorize]
        public async Task<IActionResult> getAllParentInSchoolPerSessionAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getAllParentInSchoolPerSessionAsync(schoolId);

            return Ok(result);
        }
    }
}
