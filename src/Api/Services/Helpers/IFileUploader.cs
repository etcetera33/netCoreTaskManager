using Microsoft.AspNetCore.Http;
using Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public interface IFileUploader
    {
        Task<IEnumerable<FileDto>> UploadToAzureAsync(IEnumerable<IFormFile> files);
    }
}
