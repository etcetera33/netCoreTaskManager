using Data.Repositories;

namespace Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ProjectRepository _projectRepository;
        private readonly UserRepository _userRepository;
        private readonly WorkItemRepository _workItemRepository;
        private readonly CommentRepository _commentRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ProjectRepository ProjectRepository => _projectRepository ?? new ProjectRepository(_dbContext);
        public UserRepository UserRepository => _userRepository ?? new UserRepository(_dbContext);
        public WorkItemRepository WorkItemRepository => _workItemRepository ?? new WorkItemRepository(_dbContext);
        public CommentRepository CommentRepository => _commentRepository ?? new CommentRepository(_dbContext);

        /// TODO : Реализовать интерфейс IDisposable
    }
}
