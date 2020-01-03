using Api.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.DTOs;
using Models.QueryParameters;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/projects/{projectId}/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;

        public WorkItemsController(IWorkItemService workItemService)
        {
            _workItemService = workItemService;
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
        public async Task<IActionResult> Post(WorkItemDto workItemDto)
        {
            workItemDto.AuthorId = int.Parse(User.Identity.Name);
            var workItem = await _workItemService.Create(workItemDto);

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
    }
}