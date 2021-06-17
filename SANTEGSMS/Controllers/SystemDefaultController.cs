using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SANTEGSMS.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SANTEGSMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemDefaultController : ControllerBase
    {
        private readonly ISystemDefaultRepo _systemDefaultRepo;

        public SystemDefaultController(ISystemDefaultRepo systemDefaultRepo)
        {
            _systemDefaultRepo = systemDefaultRepo;
        }

        //---------------------------------SchoolTypes------------------------------------------------

        [HttpGet("schoolTypes")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolTypesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAllSchoolTypesAsync();

            return Ok(result);
        }

        [HttpGet("schoolTypeById")]
        [Authorize]
        public async Task<IActionResult> getSchoolTypeByIdAsync(long schoolTypeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getSchoolTypeByIdAsync(schoolTypeId);

            return Ok(result);
        }

        //---------------------------------States-----------------------------------------------------

        [HttpGet("states")]
        [Authorize]
        public async Task<IActionResult> getAllStatesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAllStatesAsync();

            return Ok(result);
        }

        [HttpGet("statesById")]
        [Authorize]
        public async Task<IActionResult> getStatesByIdAsync(long stateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getStatesByIdAsync(stateId);

            return Ok(result);
        }

        //---------------------------------Gender-----------------------------------------------------

        [HttpGet("gender")]
        [Authorize]
        public async Task<IActionResult> getAllGenderAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAllGenderAsync();

            return Ok(result);
        }

        [HttpGet("genderById")]
        [Authorize]
        public async Task<IActionResult> getGenderByIdAsync(long genderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getGenderByIdAsync(genderId);

            return Ok(result);
        }

        //-----------------------Class or Alumni---------------------------------------------------------------------

        [HttpGet("classOrAlumni")]
        [AllowAnonymous]
        public async Task<IActionResult> getClassOrAlumniAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getClassOrAlumniAsync();

            return Ok(result);
        }

        [HttpGet("classOrAlumniById")]
        [AllowAnonymous]
        public async Task<IActionResult> getClassOrAlumniByIdAsync(long classOrAlumniId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getClassOrAlumniByIdAsync(classOrAlumniId);

            return Ok(result);
        }

        //-----------------------Attendance Period---------------------------------------------------------------------

        [HttpGet("attendancePeriod")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllAttendancePeriodAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAllAttendancePeriodAsync();

            return Ok(result);
        }

        [HttpGet("attendancePeriodById")]
        [AllowAnonymous]
        public async Task<IActionResult> getAttendancePeriodByIdAsync(long periodId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAttendancePeriodByIdAsync(periodId);

            return Ok(result);
        }

        [HttpGet("activeInActiveStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> getActiveInActiveStatusAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getActiveInActiveStatusAsync();

            return Ok(result);
        }

        [HttpGet("activeInActiveStatusById")]
        [AllowAnonymous]
        public async Task<IActionResult> getActiveInActiveStatusByIdAsync(long statusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getActiveInActiveStatusByIdAsync(statusId);

            return Ok(result);
        }

        [HttpGet("schoolSubTypesBySchoolTypeId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllSchoolSubTypesBySchoolTypeIdAsync(long schoolTypeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAllSchoolSubTypesBySchoolTypeIdAsync(schoolTypeId);

            return Ok(result);
        }

    }
}
