using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetWorkItemsComments(int workItemId);
        Task<CommentDto> Create(CommentDto commentDto);
        Task Remove(int commentId);
        Task<bool> CommentExists(int commentId);
    }
}
