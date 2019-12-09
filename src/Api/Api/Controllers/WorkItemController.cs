using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return new OkObjectResult(_workItemService.GetAll());
        }
    }
}