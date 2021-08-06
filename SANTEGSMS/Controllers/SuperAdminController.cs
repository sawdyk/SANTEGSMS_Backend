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
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminRepo _superAdminRepo;

        public SuperAdminController(ISuperAdminRepo superAdminRepo)
        {
            _superAdminRepo = superAdminRepo;
        }

        [HttpPost("superAdminLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> superAdminLoginAsync(LoginReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.superAdminLoginAsync(obj);

            return Ok(result);
        }

        [HttpGet("schools")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.getAllSchoolsAsync();

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

            var result = await _superAdminRepo.forgotPasswordAsync(email);

            return Ok(result);
        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<IActionResult> changePasswordAsync(string email, string oldPassword, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.changePasswordAsync(email, oldPassword, newPassword);

            return Ok(result);
        }

        [HttpPut("approveOrDeclineSchoolCreation")]
        [Authorize]
        public async Task<IActionResult> approveOrDeclineSchoolCreationAsync(bool isApproved, long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.approveOrDeclineSchoolCreationAsync(isApproved, schoolId);

            return Ok(result);
        }

        [HttpPut("enableOrDisableSchoolAccount")]
        [Authorize]
        public async Task<IActionResult> enableOrDisableSchoolAccountAsync(bool isEnabled, long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.enableOrDisableSchoolAccountAsync(isEnabled, schoolId);

            return Ok(result);
        }

        [HttpPut("updateSuperAdminDetails")]
        [Authorize]
        public async Task<IActionResult> updateSuperAdminDetailsAsync(Guid superAdminId, UpdateSuperAdminReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.updateSuperAdminDetailsAsync(superAdminId, obj);

            return Ok(result);
        }
    }
}
