﻿using AutoMapper;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Models.DTOs;
using Services.Helpers;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly IFileUploader _fileUploader;
        private readonly IWorkItemFileRepository _workItemFileRepository;

        public FileService(IFileRepository fileRepository, IMapper mapper, IFileUploader fileUploader, IWorkItemFileRepository workItemFileRepository)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
            _fileUploader = fileUploader;
            _workItemFileRepository = workItemFileRepository;
        }

        public async Task<IEnumerable<FileDto>> Upload(IEnumerable<IFormFile> files)
        {
            var azureFiles = await _fileUploader.UploadToAzureAsync(files);

            var entities = _mapper.Map<IEnumerable<FileDto>, IEnumerable<File>>(azureFiles);
            var createdEntity = await _fileRepository.Create(entities);

            return _mapper.Map<IEnumerable<File>, IEnumerable<FileDto>>(createdEntity);
        }

        public async Task<string> GetFilePath(int fileId)
        {
            var file = await _fileRepository.GetById(fileId);

            if (file == null)
            {
                return default;
            }

            return file.FilePath;
        }

        public async Task AttachFilesToWorkItem(IEnumerable<FileDto> files, int workItemId)
        {
            var entity = new List<WorkItemFile>();
            var filesId = files.Select(x => x.Id);

            Parallel.ForEach(files, file =>
            {
                entity.Add(new WorkItemFile { FileId = file.Id, WorkItemId = workItemId });
            });

            await _workItemFileRepository.AddRange(entity);
        }


        public async Task<IEnumerable<FileDto>> GetByWorkItemId(int workItemId)
        {
            var entities = await _workItemFileRepository.GetByWorkItemId(workItemId);

            return _mapper.Map<IEnumerable<WorkItemFile>, IEnumerable<FileDto>>(entities);
        }

        public async Task<bool> RemoveAttachedFileToWorkItem(int fileId, int workItemid)
        {
            var entity = await _workItemFileRepository.GetByFileNWorkItemId(fileId, workItemid);

            if (entity == null)
            {
                return false;
            }

            await _workItemFileRepository.Delete(entity.WorkItemFileId);

            return true;
        }

        public async Task<int?> GetEntityIdOrNull(int fileId, int workItemId)
        {
            var entity = await _workItemFileRepository.GetByFileNWorkItemId(fileId, workItemId);

            if (entity == null)
            {
                return default;
            }

            return entity.WorkItemFileId;
        }

        public async Task<FileDto> GetById(int fileId)
        {
            var entity = await _fileRepository.GetById(fileId);

            return _mapper.Map<File, FileDto>(entity);
        }

        public async Task DeleteRange(IEnumerable<FileDto> files)
        {
            var filesEntity = _mapper.Map<IEnumerable<FileDto>, IEnumerable<File>>(files);
            await _fileRepository.DeleteRange(filesEntity);

            foreach (var file in files)
            {
                await _fileUploader.DeleteFromAzureAsync(file.Path);
            }
        }

        public async Task Delete(FileDto file, int workItemId)
        {
            /// TODO wrap into transaction

            var workItemFileEntity = await _workItemFileRepository.GetByFileNWorkItemId(file.Id, workItemId);

            if (workItemFileEntity != null)
            {
                await _workItemFileRepository.Delete(workItemFileEntity.WorkItemFileId);
            }

            await _fileRepository.Delete(file.Id);
            await _fileUploader.DeleteFromAzureAsync(file.Path);
        }
    }
}
