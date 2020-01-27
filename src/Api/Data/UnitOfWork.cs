using Data.Repositories;
using System;

namespace Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ProjectRepository _projectRepository;
        private readonly UserRepository _userRepository;
        private readonly WorkItemRepository _workItemRepository;
        private readonly CommentRepository _commentRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _projectRepository = new ProjectRepository(_dbContext);
            _userRepository = new UserRepository(_dbContext);
            _workItemRepository = new WorkItemRepository(_dbContext);
            _commentRepository = new CommentRepository(_dbContext);
        }

        public ProjectRepository ProjectRepository => _projectRepository ?? new ProjectRepository(_dbContext);
        public UserRepository UserRepository => _userRepository ?? new UserRepository(_dbContext);
        public WorkItemRepository WorkItemRepository => _workItemRepository ?? new WorkItemRepository(_dbContext);
        public CommentRepository CommentRepository => _commentRepository ?? new CommentRepository(_dbContext);

        #region Dispose
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
