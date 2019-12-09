using Data.Repositories;

namespace Data
{
    public interface IUnitOfWork
    {
        ProjectRepository ProjectRepository { get; }
        UserRepository UserRepository { get; }
        WorkItemRepository WorkItemRepository { get; }
        CommentRepository CommentRepository { get; }
    }
}
