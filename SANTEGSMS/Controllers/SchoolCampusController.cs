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
    public class SchoolCampusController : ControllerBase
    {
        private readonly ISchoolCampusRepo _campusRepo;

        public SchoolCampusController(ISchoolCampusRepo campusRepo)
        {
            _campusRepo = campusRepo;
        }

        //------------------------ SCHOOL CAMPUS -----------------------------------------------------------

        [HttpPost("createSchoolCampus")]
        [Authorize]
        public async Task<IActionResult> createSchoolCampusAsync(SchoolCampusReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _campusRepo.createSchoolCampusAsync(obj);

            return Ok(result);
        }

        [HttpGet("schoolCampus")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolCampusAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _campusRepo.getAllSchoolCampusAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("schoolCampusById")]
        [Authorize]
        public async Task<IActionResult> getSchoolCampusByIdAsync(long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _campusRepo.getSchoolCampusByIdAsync(campusId);

            return Ok(result);
        }

        [HttpPut("updateCampusDetails")]
        [Authorize]
        public async Task<IActionResult> updateCampusDetailsAsync(long campusId, SchoolCampusReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _campusRepo.updateCampusDetailsAsync(campusId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteSchoolCampus")]
        [Authorize]
        public async Task<IActionResult> deleteSchoolCampusAsync(long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _campusRepo.deleteSchoolCampusAsync(campusId);

            return Ok(result);
        }
    }
}
