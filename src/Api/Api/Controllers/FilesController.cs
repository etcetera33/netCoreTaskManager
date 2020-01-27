using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [Route("")]
        public async Task<IActionResult> Delete(IEnumerable<FileDto> files)
        {
            if (files is null)
            {
                return NotFound("Files can not be found");
            }

            await _fileService.DeleteRange(files);

            return Ok();
        }

        [HttpDelete]
        [Route("{fileId}/{workItemId}")]
        public async Task<IActionResult> Delete(int fileId, int workItemId)
        {
            var file = await _fileService.GetById(fileId);

            if (file == null)
            {
                return NotFound("File not found");
            }

            if (workItemId == 0)
            {
                return NotFound("work item not found");
            }

            await _fileService.Delete(file, workItemId);

            return Ok();
        }
    }
}