using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.QueryParameters;
using Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var files = Request.Form.Files;
            var entity = await _fileService.Upload(files);
            
            return new JsonResult(entity);
        }

        [HttpDelete]
        [Route("{fileId}")]
        public async Task<IActionResult> Get(int fileId)
        {
            var exists = await _fileService.Exists(fileId);

            if (!exists)
            {
                return NotFound();
            }

            await _fileService.Delete(fileId);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] BaseQueryParameters parameters)
        {
            var response = await _fileService.GetImages(parameters);

            return Ok(response);
        }
    }
}