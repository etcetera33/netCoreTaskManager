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
        private readonly ICommentService _commentService;

        public WorkItemController(IWorkItemService workItemService, ICommentService commentService)
        {
            _workItemService = workItemService;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var workItems = await _workItemService.GetAll();

            return Ok(workItems);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateWorkItemDto workItemDto)
        {
            var workItem = await _workItemService.Create(workItemDto);

            return new JsonResult(workItem) { StatusCode = (int)HttpStatusCode.Created};
        }

        [HttpGet("{id}", Name = "GetWorkItem")]
        public async Task<IActionResult> Get(int projectId)
        {
            var project = await _workItemService.GetById(projectId);

            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int workItemId, CreateWorkItemDto workItemDto)
        {
            await _workItemService.Update(workItemId, workItemDto);

            return NoContent();
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments(int workItemId)
        {
            var comments = await _commentService.GetWorkItemsComments(workItemId);
            
            return Ok(comments);
        }

        [HttpPost("{id}/comments/create")]
        public async Task<IActionResult> CreateComment(CreateCommentDto commentDto)
        {
            var comment = await _commentService.Create(commentDto);
            return Ok(comment);
        }
    }
}