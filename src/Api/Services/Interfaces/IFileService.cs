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
        Task<IEnumerable<FileDto>> Upload(IEnumerable<IFormFile> file);
        Task<BasePaginatedResponse<FileDto>> GetImages(BaseQueryParameters parameters);
        Task<FileDto> GetById(int fileId);
        Task Delete(FileDto file);
    }
}
