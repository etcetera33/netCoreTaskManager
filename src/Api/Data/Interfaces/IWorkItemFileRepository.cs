using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IWorkItemFileRepository : IBaseRepository<WorkItemFile>
    {
        Task AddRange(IEnumerable<WorkItemFile> entityList);
        Task<IEnumerable<WorkItemFile>> GetByWorkItemId(int workItemId);
        Task<WorkItemFile> GetByFileNWorkItemId(int fileId, int workItemId);
    }
}
