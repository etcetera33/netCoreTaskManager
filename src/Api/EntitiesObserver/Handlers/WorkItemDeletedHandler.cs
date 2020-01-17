using Contracts;
using Core.Adapters;
using Core.Enums;
using MassTransit;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace EntitiesObserver.Handlers
{
    public class WorkItemDeletedHandler : IConsumer<WorkItemDeleted>
    {
        private readonly IWorkItemService _workItemService;
        private readonly IUserService _userService;
        private readonly IBus _bus;
        private readonly ILoggerAdapter<WorkItemDeletedHandler> _logger;
        private readonly IWorkItemAuditService _workItemAuditService;

        public WorkItemDeletedHandler(IWorkItemService workItemService, IUserService userService, IBus bus, ILoggerAdapter<WorkItemDeletedHandler> logger, IWorkItemAuditService workItemAuditService)
        {
            _workItemService = workItemService;
            _userService = userService;
            _bus = bus;
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

                if (workItemId == 0)
                {
                    throw new ArgumentException("Work item should not be equal 0");
                }

                var createdEnitty = await _workItemAuditService.Create(workItemId, WIAuditStatuses.Deleted, oldWorkItem: context.Message.OldWorkItem);

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
