using Microsoft.AspNetCore.Mvc;
using SANTEGSMS.Helpers;
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
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadRepo _uploadFilesRepo;

        public FileUploadController(IFileUploadRepo uploadFilesRepo)
        {
            _uploadFilesRepo = uploadFilesRepo;
        }

        [DisableRequestSizeLimit]
        [HttpPost("uploadFiles")]
        public async Task<IActionResult> uploadFilesAsync([FromForm] FileUploadReqModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.uploadFilesAsync(obj);

            return Ok(result);
        }

        [HttpGet("appTypes")]
        public async Task<IActionResult> getAllAppTypesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.getAllAppTypesAsync();

            return Ok(result);
        }

        [HttpGet("appTypesById")]
        public async Task<IActionResult> getAppTypesByIdAsync(long appId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.getAppTypesByIdAsync(appId);

            return Ok(result);
        }

        [HttpGet("supportedFileExtensions")]
        public async Task<IActionResult> getAllSupportedFileExtensionsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.getAllSupportedFileExtensionsAsync();

            return Ok(result);
        }

        [HttpGet("folderTypesByAppId")]
        public async Task<IActionResult> getAllFolderTypesByAppIdAsync(long appId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.getAllFolderTypesByAppIdAsync(appId);

            return Ok(result);
        }

        [HttpGet("folderTypeById")]
        public async Task<IActionResult> getFolderTypeByIdAsync(long folderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.getFolderTypeByIdAsync(folderId);

           
            return Ok(result);
        }
    }
}
