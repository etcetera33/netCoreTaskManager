using Contracts;
using EntitiesObserver.Exceptions;
using MassTransit;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace EntitiesObserver.Handlers
{
    class WorkItemChangedHandler: IConsumer<WorkItemChanged>
    {
        private readonly IWorkItemService _workItemService;
        private readonly IUserService _userService;
        private readonly IBus _bus;

        public WorkItemChangedHandler(IWorkItemService workItemService, IUserService userService, IBus bus)
        {
            _workItemService = workItemService;
            _userService = userService;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<WorkItemChanged> context)
        {
            if (context.Message == null)
            {
                throw new InvalidOperationException("Message is null");
            }

            int workItemId = context.Message.WorkItemId;

            var workItem = await _workItemService.GetById(context.Message.WorkItemId);

            if (workItem == null)
            {
                throw new WorkItemNotFoundException();
            }

            var userData = await _userService.GetById(workItem.AssigneeId);

            if (userData == null)
            {
                throw new UserNotFoundException();
            }

            await _bus.Publish(new EmailSend
            {
                To = userData.Email,
                Subject = "New work item assignee",
                Body = $"Dear, {userData.FullName}! You are the new assignee for the work item #{workItemId}"
            });
        }
    }
}
