using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Comment;
using Models.DTOs.WorkItem;
using Services.Interfaces;
using System.Net;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        private readonly IWorkItemService _workItemService;

        public WorkItemController(IWorkItemService workItemService, ICommentService commentService)
        {
            _workItemService = workItemService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var workItems = await _workItemService.GetAll();

            return Ok(workItems);
        }

        [HttpPost]
        public async Task<IActionResult> Post(WorkItemDto workItemDto)
        {
            var workItem = await _workItemService.Create(workItemDto);

            return new JsonResult(workItem) { StatusCode = (int)HttpStatusCode.Created};
        }

        [HttpGet("{id}", Name = "GetWorkItem")]
        public async Task<IActionResult> Get(int projectId)
        {
            var project = await _workItemService.GetById(projectId);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int workItemId, WorkItemDto workItemDto)
        {
            await _workItemService.Update(workItemId, workItemDto);

            return NoContent();
        }
    }
}