using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.QueryParameters;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/projects/{projectId}/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;
        private readonly IFileService _fileService;

        public WorkItemsController(IWorkItemService workItemService, IFileService fileService)
        {
            _workItemService = workItemService;
            _fileService = fileService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int projectId, [FromQuery] WorkItemQueryParameters parameters)
        {
            if (parameters.AssigneeId == null)
            {
                parameters.AssigneeId = int.Parse(User.Identity.Name);
            }
            
            var workItem = await _workItemService.Paginate(projectId, parameters);

            if (workItem == null)
            {
                return NotFound();
            }

            return Ok(workItem);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(WorkItemDto data)
        {
            data.AuthorId = int.Parse(User.Identity.Name);

            if (data.AuthorId == 0)
            {
                return NotFound("User not found");
            }

            var workItem = await _workItemService.Create(data);

            return new JsonResult(workItem) { StatusCode = 201 };
        }

        [HttpGet("{id}", Name = "GetWorkItem")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var workItem = await _workItemService.GetById(id);

            if (workItem == null)
            {
                return NotFound();
            }
            
            return Ok(workItem);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Developer,Owner")]
        public async Task<IActionResult> Put(int id, WorkItemDto workItemDto)
        {
            await _workItemService.Update(id, workItemDto);

            return NoContent();
        }

        [HttpGet("types")]
        [Authorize]
        public IActionResult GetTypes()
        {
            return Ok(_workItemService.GetWorkItemTypes());
        }

        [HttpGet("statuses")]
        [Authorize]
        public IActionResult GetStatuses()
        {
            return Ok(_workItemService.GetWorkItemStatuses());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Delete(int id)
        {
            var workItemExists = await _workItemService.WorkItemExists(id);

            if (!workItemExists)
            {
                return NotFound();
            }

            await _workItemService.Delete(id);

            return NoContent();
        }
    }
}