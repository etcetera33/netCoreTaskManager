using Microsoft.AspNetCore.Mvc;
using Models.DTOs.WorkItem;
using Services.Interfaces;
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
        [Route("GetAll")]
        public async Task<IActionResult> Index()
        {
            return new OkObjectResult(await _workItemService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateWorkItemDto workItemDto)
        {
            await _workItemService.Create(workItemDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int workItemId, CreateWorkItemDto workItemDto)
        {
            await _workItemService.Update(workItemId, workItemDto);
            return Ok();
        }
    }
}