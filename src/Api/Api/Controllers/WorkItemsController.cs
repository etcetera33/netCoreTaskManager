using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Models.QueryParameters;
using Services.Interfaces;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/projects/{projectId}/[controller]")]
    [ApiController]
    public class WorkItemsController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;
        private readonly IProjectService _projectService;

        public WorkItemsController(IWorkItemService workItemService, IProjectService projectService)
        {
            _workItemService = workItemService;
            _projectService = projectService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int projectId, [FromQuery] WorkItemQueryParameters parameters)
        {
            if (parameters.AssigneeId == 0) {
                parameters.AssigneeId = int.Parse(User.Identity.Name);
            }
            
            var workItem = await _workItemService.Paginate(projectId, parameters);

            if (workItem == null)
            {
                return NotFound();
            }

            return Ok(workItem);
        }

        /*[HttpGet("project/{projectId}")]
        [Authorize]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            if (!await _projectService.ProjectExists(projectId))
            {
                return NotFound();
            }
            
            var workItems = await _workItemService.GetWorkItemsByProjectId(projectId);

            return Ok(workItems);
        }*/

        /*[HttpGet("project/{projectId}/current-user")]
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
        }*/

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