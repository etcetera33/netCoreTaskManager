using Microsoft.AspNetCore.Http;
using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public interface IFileUploader
    {
        Task<IEnumerable<FileDto>> UploadToAzureAsync(IEnumerable<IFormFile> files);
        Task DeleteFromAzureAsync(string filePath);
        Task DeleteFromAzureAsync(IEnumerable<FileDto> files);
    }
}
