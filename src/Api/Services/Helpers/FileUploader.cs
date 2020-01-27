using Core.Adapters;
using Core.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class FileUploader : IFileUploader
    {
        private readonly IOptions<AzureConfig> _azureConfig;
        private readonly ILoggerAdapter<FileUploader> _logger;

        public FileUploader(IOptions<AzureConfig> azureConfig, ILoggerAdapter<FileUploader> logger)
        {
            _azureConfig = azureConfig;
            _logger = logger;
        }

        private CloudBlobContainer ConnectToContainer()
        {
            return CloudStorageAccount.Parse(_azureConfig.Value.ConnectionString)
                    .CreateCloudBlobClient()
                    .GetContainerReference(_azureConfig.Value.ContainerName);
        }

        public async Task<IEnumerable<FileDto>> UploadToAzureAsync(IEnumerable<IFormFile> files)
        {
            try
            {
                var container = ConnectToContainer();

                var filesListDto = new List<FileDto>();

                Stream fileStream;
                foreach (var file in files)
                    using (fileStream = file.OpenReadStream())
                    {

                        fileStream = file.OpenReadStream();

                        CloudBlockBlob blockBlob;

                        do
                        {
                            blockBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        } while (await blockBlob.ExistsAsync() != false);

                        await blockBlob.UploadFromStreamAsync(fileStream);

                        filesListDto.Add(new FileDto
                        {
                            Name = file.FileName,
                            Path = blockBlob.Uri.ToString()
                        });
                    }

                return filesListDto;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return default;
            }
        }

        public async Task DeleteFromAzureAsync(string filePath)
        {
            try
            {
                var container = ConnectToContainer();

                var blockBlob = container.GetBlockBlobReference(filePath);

                await blockBlob.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        public async Task DeleteFromAzureAsync(IEnumerable<FileDto> files)
        {
            foreach (var file in files)
            {
                await DeleteFromAzureAsync(file.Path);
            }
        }
    }
}
