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

        [HttpPost]
        [Route("work-items/{workItemId}")]
        public async Task<IActionResult> Create([CustomizeValidator(Properties = "Id")] IEnumerable<FileDto> files, int workItemId)
        {
            await _fileService.AttachFilesToWorkItem(files, workItemId);

            return Ok();
        }

        [HttpGet]
        [Route("work-items/{workItemId}")]
        public async Task<IActionResult> Get(int workItemId)
        {
            var list = await _fileService.GetByWorkItemId(workItemId);

            return Ok(list);
        }

        [HttpDelete]
        [Route("{fileId}/work-items/{workItemId}")]
        public async Task<IActionResult> Get(int fileId, int workItemId)
        {
            var id = await _fileService.GetEntityIdOrNull(fileId, workItemId);

            if (id == null)
            {
                return NotFound();
            }

            await _fileService.DeleteWorkItemFile(id.Value);

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