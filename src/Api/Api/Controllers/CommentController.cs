﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Comment;
using Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IWorkItemService _workItemService;

        public CommentController(ICommentService commentService, IWorkItemService workItemService)
        {
            _commentService = commentService;
            _workItemService = workItemService;
        }

        [HttpGet("work-item/{workItemId}")]
        public async Task<IActionResult> GetComments(int workItemId)
        {
            if (! await _workItemService.WorkItemExists(workItemId))
                return NotFound();

            var comments = await _commentService.GetWorkItemsComments(workItemId);

            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CommentDto commentDto)
        {
            if (! await _workItemService.WorkItemExists(commentDto.WorkItemId))
                return NotFound();

            var comment = await _commentService.Create(commentDto);

            return new JsonResult(comment) { StatusCode = 201};
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _commentService.Remove(id);

            return NoContent();
        }
    }
}