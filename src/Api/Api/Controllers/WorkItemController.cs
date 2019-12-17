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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var workItems = await _workItemService.GetAll();

            return Ok(workItems);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            if (!await _projectService.ProjectExists(projectId))
                return NotFound();

            var workItems = await _workItemService.GetWorkItemsByProjectId(projectId);

            return Ok(workItems);
        }

        [HttpPost]
        public async Task<IActionResult> Post(WorkItemDto workItemDto)
        {
            var workItem = await _workItemService.Create(workItemDto);

            return new JsonResult(workItem) { StatusCode = 201 };
        }

        [HttpGet("{id}", Name = "GetWorkItem")]
        public async Task<IActionResult> Get(int id)
        {
            var workItem = await _workItemService.GetById(id);

            if (workItem == null)
                return NotFound();

            return Ok(workItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, WorkItemDto workItemDto)
        {
            await _workItemService.Update(id, workItemDto);

            return NoContent();
        }

        /*[HttpGet("user/{projectId}")]
        public async Task<IActionResult> GetByUser(int projectId)
        {
            //return await _workItemService.GetWorkItemsByProjectNAssigneeId(projectId,
                // import auth here and paste UserId
            //);
        }*/
    }
}