using Contracts;
using Core.Adapters;
using MassTransit;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace EntitiesObserver.Handlers
{
    public class WorkItemDeletedHandler : IConsumer<WorkItemDeleted>
    {
        private readonly ILoggerAdapter<WorkItemDeletedHandler> _logger;
        private readonly IWorkItemAuditService _workItemAuditService;

        public WorkItemDeletedHandler(ILoggerAdapter<WorkItemDeletedHandler> logger, IWorkItemAuditService workItemAuditService)
        {
            _logger = logger;
            _workItemAuditService = workItemAuditService;
        }

        public async Task Consume(ConsumeContext<WorkItemDeleted> context)
        {
            try
            {
                if (context.Message == null)
                {
                    throw new ArgumentNullException("Message is null");
                }

                int workItemId = context.Message.WorkItemId;

                if (workItemId < 1)
                {
                    throw new ArgumentException($"Work item should be equal to 1 or higher. Current value: {workItemId}");
                }

                var createdEnitty = await _workItemAuditService.LogWorkItemDeletion(workItemId, context.Message.OldWorkItem);

                _logger.Information($"Successfully logged work item deletion. WorkItemAuditId: {createdEnitty.Id}");
            }
            catch(Exception exception)
            {
                _logger.Error(exception.Message);
                return;
            }
        }
    }
}
