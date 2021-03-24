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
    public class LocalGovtController : ControllerBase
    {
        private readonly ILocalGovtRepo _superAdminRepo;

        public LocalGovtController(ILocalGovtRepo superAdminRepo)
        {
            _superAdminRepo = superAdminRepo;
        }

        //-------------------------------States---------------------------------------------------------------

        [HttpPost("createStates")]
        [Authorize]
        public async Task<IActionResult> createStatesAsync(StateReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.createStatesAsync(obj);

            return Ok(result);
        }

        //-------------------------------Local Govt---------------------------------------------------------------

        [HttpPost("createLocalGovt")]
        [Authorize]
        public async Task<IActionResult> createLocalGovtAsync(LocalGovtReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.createLocalGovtAsync(obj);

            return Ok(result);
        }

        [HttpGet("localGovtById")]
        [Authorize]
        public async Task<IActionResult> getLocalGovtByIdAsync(long localGovtId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.getLocalGovtByIdAsync(localGovtId);

            return Ok(result);
        }

        [HttpGet("localGovtInStates")]
        [Authorize]
        public async Task<IActionResult> getAllLocalGovtInStatesAsync(long stateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.getAllLocalGovtInStatesAsync(stateId);

            return Ok(result);
        }

        [HttpPut("updateLocalGovt")]
        [Authorize]
        public async Task<IActionResult> updateLocalGovtAsync(long localGovtId, LocalGovtReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.updateLocalGovtAsync(localGovtId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteLocalGovt")]
        [Authorize]
        public async Task<IActionResult> deleteLocalGovtAsync(long localGovtId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _superAdminRepo.deleteLocalGovtAsync(localGovtId);

            return Ok(result);
        }
    }
}
