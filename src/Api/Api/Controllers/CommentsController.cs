using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IWorkItemService _workItemService;

        public CommentsController(ICommentService commentService, IWorkItemService workItemService)
        {
            _commentService = commentService;
            _workItemService = workItemService;
        }

        [HttpGet("work-item/{workItemId}")]
        [Authorize]
        public async Task<IActionResult> GetComments(int workItemId)
        {
            if (!await _workItemService.WorkItemExists(workItemId))
            {
                return NotFound();
            }

            var comments = await _commentService.GetWorkItemsComments(workItemId);

            return Ok(comments);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CommentDto commentDto)
        {
            commentDto.AuthorId = int.Parse(User.Identity.Name);
            var comment = await _commentService.Create(commentDto);

            return new JsonResult(comment) { StatusCode = 201 };
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _commentService.CommentExists(id))
            {
                return NotFound();
            }

            if (await _commentService.GetCommentAuthorId(id) != int.Parse(User.Identity.Name))
            {
                return Unauthorized();
            }

            await _commentService.Remove(id);

            return NoContent();
        }
    }
}
