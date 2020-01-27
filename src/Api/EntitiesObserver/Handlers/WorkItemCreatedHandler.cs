using Contracts;
using Core.Adapters;
using MassTransit;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace EntitiesObserver.Handlers
{
    public class WorkItemCreatedHandler : IConsumer<WorkItemCreated>
    {
        private readonly IWorkItemAuditService _workItemAuditService;
        private readonly ILoggerAdapter<WorkItemCreatedHandler> _logger;
        private readonly IUserService _userService;
        private readonly IBus _bus;

        public WorkItemCreatedHandler(IWorkItemAuditService workItemAuditService, ILoggerAdapter<WorkItemCreatedHandler> logger, IUserService userService, IBus bus)
        {
            _workItemAuditService = workItemAuditService;
            _logger = logger;
            _userService = userService;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<WorkItemCreated> context)
        {
            try
            {
                if (context.Message?.NewWorkItem == null)
                {
                    throw new ArgumentNullException("Work item is not provided");
                }

                var workItem = context.Message.NewWorkItem;

                var userData = await _userService.GetById(workItem.AssigneeId);

                if (userData == null)
                {
                    throw new ArgumentNullException("Assignee not found");
                }

                await _bus.Publish(new EmailSend
                {
                    To = userData.Email,
                    Subject = "New work item assignee",
                    Body = $"Dear, {userData.FullName}! You are the new assignee for the work item #{context.Message.WorkItemId}"
                });

                _logger.Information($"Bus published EmailSend contract with email: {userData.Email}. WorkItemId: {context.Message.WorkItemId}");

                var createdEntity = await _workItemAuditService.WICreated(context.Message.WorkItemId, context.Message.NewWorkItem);

                _logger.Information($"Successfully logged work item creation. WorkItemAuditId: {createdEntity.Id}");
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                return;
            }
        }
    }
}
