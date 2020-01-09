using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IWorkItemRepository : IBaseRepository<WorkItem>
    {
        Task<IEnumerable<WorkItem>> PaginateFiltered(Expression<Func<WorkItem, bool>> expression, int offset, int itemsCount);
        Task<int> GetFilteredDataCountAsync(Expression<Func<WorkItem, bool>> expression);
        Task<IEnumerable<WorkItem>> GetTopFivePriorityItems(int assigneeId);
    }
}
