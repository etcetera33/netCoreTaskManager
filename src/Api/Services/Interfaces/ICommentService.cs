using Models.DTOs.Comment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetWorkItemsComments(int workItemId);
        Task Create(CreateCommentDto commentDto);
    }
}
