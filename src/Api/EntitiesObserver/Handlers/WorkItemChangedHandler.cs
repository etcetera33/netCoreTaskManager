using Contracts;
using Core.Adapters;
using Core.Enums;
using MassTransit;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace EntitiesObserver.Handlers
{
    public class WorkItemChangedHandler: IConsumer<WorkItemUpdated>
    {
        private readonly IWorkItemService _workItemService;
        private readonly IUserService _userService;
        private readonly IBus _bus;
        private readonly ILoggerAdapter<WorkItemChangedHandler> _logger;
        private readonly IWorkItemAuditService _workItemAuditService;

        public WorkItemChangedHandler(IWorkItemService workItemService, IUserService userService, IBus bus, ILoggerAdapter<WorkItemChangedHandler> logger, IWorkItemAuditService workItemAuditService)
        {
            _workItemService = workItemService;
            _userService = userService;
            _bus = bus;
            _logger = logger;
            _workItemAuditService = workItemAuditService;
        }

        public async Task Consume(ConsumeContext<WorkItemUpdated> context)
        {
            try
            {
                if (context.Message == null)
                {
                    throw new ArgumentNullException("Message is null");
                }

                int workItemId = context.Message.WorkItemId;

                var workItem = await _workItemService.GetById(workItemId);

                if (workItem == null)
                {
                    throw new ArgumentException("Work item not found");
                }

                var userData = await _userService.GetById(workItem.AssigneeId);

                if (userData == null)
                {
                    throw new ArgumentNullException("Assignee not found");
                }

                if (context.Message.NewWorkItem.AssigneeId != context.Message.OldWorkItem.AssigneeId)
                {
                    await _bus.Publish(new EmailSend
                    {
                        To = userData.Email,
                        Subject = "New work item assignee",
                        Body = $"Dear, {userData.FullName}! You are the new assignee for the work item #{workItemId}"
                    });

                    _logger.Information($"Bus published EmailSend contract with email: {userData.Email}. WorkItemId: {workItemId}");
                }

                if (context.Message.OldWorkItem != context.Message.NewWorkItem)
                {
                    var createdEntity = await _workItemAuditService.WIUpdated(context.Message.WorkItemId, context.Message.OldWorkItem, newWorkItem: context.Message.NewWorkItem);

                    _logger.Information($"Successfully logged work item editing. WorkItemAuditId: {createdEntity.Id}");
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                return;
            }
        }
    }
}
