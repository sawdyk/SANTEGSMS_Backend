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
    }
}
