using AutoMapper;
using Core.Enums;
using Data.Interfaces;
using Data.Models;
using Models.DTOs;
using Newtonsoft.Json;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class WorkItemAuditService : IWorkItemAuditService
    {
        private readonly IWorkItemAuditRepository _wiAuditRepository;
        private readonly IMapper _mapper;

        public WorkItemAuditService(IWorkItemAuditRepository wiAuditRepository, IMapper mapper)
        {
            _wiAuditRepository = wiAuditRepository;
            _mapper = mapper;
        }

        private async Task<WorkItemAuditDto> Log(int workItemId, WIAuditStatuses status, WorkItemHistoryDto oldWorkItem = null, WorkItemHistoryDto newWorkItem = null)
        {
            var oldEntity = _mapper.Map<WorkItemHistoryDto, WorkItem>(oldWorkItem);
            var newEntity = _mapper.Map<WorkItemHistoryDto, WorkItem>(newWorkItem);

            var createdEntity = await _wiAuditRepository.Create(new WorkItemAudit
            {
                WorkItemId = workItemId,
                Status = status,
                OldWorkItem = JsonConvert.SerializeObject(oldEntity),
                NewWorkItem = JsonConvert.SerializeObject(newEntity)
            });

            return _mapper.Map<WorkItemAudit, WorkItemAuditDto>(createdEntity);
        }

        public async Task<IEnumerable<WorkItemAuditDto>> GetWorkItemsHistoryById(int workItemId)
        {
            var wiHistory = await _wiAuditRepository.GetByWorkItemId(workItemId);

            return _mapper.Map<IEnumerable<WorkItemAudit>, IEnumerable<WorkItemAuditDto>>(wiHistory);
        }

        public async Task<WorkItemAuditDto> WICreated(int workItemId, WorkItemHistoryDto newWorkItem)
        {
            return await Log(workItemId, WIAuditStatuses.Created, newWorkItem: newWorkItem);
        }

        public async Task<WorkItemAuditDto> WIDeleted(int workItemId, WorkItemHistoryDto oldWorkItem)
        {
            return await Log(workItemId, WIAuditStatuses.Deleted, oldWorkItem: oldWorkItem);
        }

        public async Task<WorkItemAuditDto> WIUpdated(int workItemId, WorkItemHistoryDto oldWorkItem, WorkItemHistoryDto newWorkItem)
        {
            return await Log(workItemId, WIAuditStatuses.Deleted, oldWorkItem, newWorkItem);
        }
    }
}
