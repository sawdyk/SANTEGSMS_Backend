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
    public class SchoolUsersController : ControllerBase
    {
        private readonly ISchoolUsersRepo _schoolUsersRepo;
        public SchoolUsersController(ISchoolUsersRepo schoolUsersRepo)
        {
            _schoolUsersRepo = schoolUsersRepo;
        }

        //-------------------- SCHOOL USERS -----------------------------------------------

        [HttpPost("createSchoolUsers")]
        [Authorize]
        public async Task<IActionResult> createSchoolUsersAsync(SchoolUsersReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolUsersRepo.createSchoolUsersAsync(obj);

            return Ok(result);
        }

        [HttpPost("schoolUserLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> schoolUserLoginAsync(LoginReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolUsersRepo.schoolUserLoginAsync(obj);

            return Ok(result);
        }


        [HttpGet("schoolUsersByRoleId")]
        [Authorize]
        public async Task<IActionResult> getSchoolUsersByRoleIdAsync(long schoolId, long campusId, long roleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolUsersRepo.getSchoolUsersByRoleIdAsync(schoolId, campusId, roleId);

            return Ok(result);
        }

        [HttpPut("updateSchoolUserDetails")]
        [Authorize]
        public async Task<IActionResult> updateSchoolUserDetailsAsync(Guid schoolUserId, UpdateSchoolUsersDetailsReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolUsersRepo.updateSchoolUserDetailsAsync(schoolUserId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteSchoolUsers")]
        [Authorize]
        public async Task<IActionResult> deleteSchoolUsersAsync(Guid schoolUserId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolUsersRepo.deleteSchoolUsersAsync(schoolUserId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("schoolUsersBySchoolId")]
        [Authorize]
        public async Task<IActionResult> getSchoolUsersBySchoolIdAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolUsersRepo.getSchoolUsersBySchoolIdAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("schoolAdminsBySchoolId")]
        [Authorize]
        public async Task<IActionResult> getSchoolAdminsBySchoolIdAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolUsersRepo.getSchoolAdminsBySchoolIdAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("schoolAdminsByCampusId")]
        [Authorize]
        public async Task<IActionResult> getSchoolAdminsByCampusIdAsync(long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolUsersRepo.getSchoolAdminsByCampusIdAsync(campusId);

            return Ok(result);
        }


        [HttpGet("schoolUsersByCampuslId")]
        [Authorize]
        public async Task<IActionResult> getSchoolUsersByCampuslIdAsync(long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolUsersRepo.getSchoolUsersByCampuslIdAsync(campusId);

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

            var result = await _schoolUsersRepo.forgotPasswordAsync(email);

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

            var result = await _schoolUsersRepo.changePasswordAsync(email, oldPassword, newPassword);

            return Ok(result);
        }
    }
}
