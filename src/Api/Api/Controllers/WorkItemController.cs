using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;
        private readonly IProjectService _projectService;

        public WorkItemController(IWorkItemService workItemService, IProjectService projectService)
        {
            _workItemService = workItemService;
            _projectService = projectService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            if (!await _projectService.ProjectExists(projectId))
            {
                return NotFound();
            }
            
            var workItems = await _workItemService.GetWorkItemsByProjectId(projectId);

            return Ok(workItems);
        }

        [HttpGet("project/{projectId}/current-user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUsersWorkItemsByProjectId(int projectId)
        {
            if (!await _projectService.ProjectExists(projectId))
            {
                return NotFound();
            }
            
            var userId = int.Parse(User.Identity.Name);
            var workItems = await _workItemService.GetWorkItemsByProjectNAssigneeId(projectId, userId);

            return Ok(workItems);
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
        public async Task<IActionResult> Put(int id, WorkItemDto workItemDto)
        {
            await _workItemService.Update(id, workItemDto);

            return NoContent();
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            return Ok(await _workItemService.GetWorkItemTypes());
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatuses()
        {
            return Ok(await _workItemService.GetWorkItemStatuses());
        }
    }
}