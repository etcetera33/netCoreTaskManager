using Microsoft.AspNetCore.Http;
using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<FileDto>> Upload(IEnumerable<IFormFile> file);
        Task<FileDto> GetById(int fileId);
        Task Delete(IEnumerable<FileDto> file);
        Task Delete(FileDto file, int workItemId);
    }
}
