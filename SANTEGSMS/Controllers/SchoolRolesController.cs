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
    public class SchoolRolesController : ControllerBase
    {
        private readonly ISchoolRolesRepo _schoolRolesRepo;

        public SchoolRolesController(ISchoolRolesRepo schoolRolesRepo)
        {
            _schoolRolesRepo = schoolRolesRepo;
        }

        [HttpGet("schoolRoles")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolRolesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRolesRepo.getAllSchoolRolesAsync();

            return Ok(result);
        }

        [HttpGet("schoolRolesByRoleId")]
        [Authorize]
        public async Task<IActionResult> getSchoolRolesByRoleIdAsync(long schoolRoleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRolesRepo.getSchoolRolesByRoleIdAsync(schoolRoleId);

            return Ok(result);
        }

        [HttpGet("schoolRolesForSchoolUserCreation")]
        [Authorize]
        public async Task<IActionResult> getSchoolRolesForSchoolUserCreationAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRolesRepo.getSchoolRolesForSchoolUserCreationAsync();

            return Ok(result);
        }

        [HttpPost("assignRolesToSchoolUsers")]
        [Authorize]
        public async Task<IActionResult> assignRolesToSchoolUsersAsync(AssignRolesReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRolesRepo.assignRolesToSchoolUsersAsync(obj);

            return Ok(result);
        }


        [HttpDelete("deleteRolesAssignedToSchoolUsers")]
        [Authorize]
        public async Task<IActionResult> deleteRolesAssignedToSchoolUsersAsync(DeleteRolesAssignedReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRolesRepo.deleteRolesAssignedToSchoolUsersAsync(obj);

            return Ok(result);
        }
    }
}
