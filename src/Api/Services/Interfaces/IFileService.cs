using Microsoft.AspNetCore.Http;
using Models.DTOs;
using Models.PaginatedResponse;
using Models.QueryParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFileService
    {
        Task<string> GetFilePath(int fileId);
        Task<IEnumerable<FileDto>> Upload(IEnumerable<IFormFile> file);
        Task<IEnumerable<FileDto>> GetByWorkItemId(int workItemId);
        Task AttachFilesToWorkItem(IEnumerable<FileDto> filesId, int workItemId);
        Task<BasePaginatedResponse<FileDto>> GetImages(BaseQueryParameters parameters);
        Task<int?> GetEntityIdOrNull(int fileId, int workItemId);
        Task DeleteWorkItemFile(int id);
    }
}
