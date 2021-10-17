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
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictRepo _districtRepo;

        public DistrictController(IDistrictRepo districtRepo)
        {
            _districtRepo = districtRepo;
        }

        //-------------------------------Districts---------------------------------------------------------------

        [HttpPost("createDistrict")]
        [Authorize]
        public async Task<IActionResult> createDistrictAsync(DistrictReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.createDistrictAsync(obj);

            return Ok(result);
        }

        [HttpGet("districts")]
        [Authorize]
        public async Task<IActionResult> getAllDistrictsAsync(long localGovtId, long stateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.getAllDistrictsAsync(localGovtId, stateId);

            return Ok(result);
        }

        [HttpGet("districtInLocalGovt")]
        [Authorize]
        public async Task<IActionResult> getAllDistrictInLocalGovtAsync(long localGovtId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.getAllDistrictInLocalGovtAsync(localGovtId);

            return Ok(result);
        }

        [HttpGet("districtInState")]
        [Authorize]
        public async Task<IActionResult> getAllDistrictInStateAsync(long stateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.getAllDistrictInStateAsync(stateId);

            return Ok(result);
        }


        [HttpGet("districtById")]
        [Authorize]
        public async Task<IActionResult> getDistrictByIdAsync(long districtId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.getDistrictByIdAsync(districtId);

            return Ok(result);
        }

        [HttpPut("updateDistrict")]
        [Authorize]
        public async Task<IActionResult> updateDistrictAsync(long districtId, DistrictReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.updateDistrictAsync(districtId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteDistrict")]
        [Authorize]
        public async Task<IActionResult> deleteDistrictAsync(long districtId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.deleteDistrictAsync(districtId);

            return Ok(result);
        }

        //-------------------------------Districts Administrator---------------------------------------------------------------

        [HttpPost("createDistrictAdministrator")]
        [Authorize]
        public async Task<IActionResult> createDistrictAdministratorAsync(DistrictAdminReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.createDistrictAdministratorAsync(obj);

            return Ok(result);
        }

        [HttpGet("districtAssignedToDistrictAdministrator")]
        [Authorize]
        public async Task<IActionResult> getAllDistrictAssignedToDistrictAdministratorAsync(Guid districtAdminId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.getAllDistrictAssignedToDistrictAdministratorAsync(districtAdminId);

            return Ok(result);
        }

        [HttpGet("districtAdministratorById")]
        [Authorize]
        public async Task<IActionResult> getDistrictAdministratorByIdAsync(Guid districtAdminId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.getDistrictAdministratorByIdAsync(districtAdminId);

            return Ok(result);
        }

        [HttpGet("allDistrictAdministrator")]
        [Authorize]
        public async Task<IActionResult> getAllDistrictAdministratorAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _districtRepo.getAllDistrictAdministratorAsync();

            return Ok(result);
        }
    }
}
