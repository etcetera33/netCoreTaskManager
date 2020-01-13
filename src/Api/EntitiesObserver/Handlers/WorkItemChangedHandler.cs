using Contracts;
using MassTransit;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace EntitiesObserver.Handlers
{
    public class WorkItemChangedHandler: IConsumer<WorkItemChanged>
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
                throw new ArgumentNullException("Message is null");
            }

            int workItemId = context.Message.WorkItemId;

            if (workItemId == 0)
            {
                throw new ArgumentException("Work item should not be equal 0");
            }

            var workItem = await _workItemService.GetById(workItemId);

            if (workItem == null)
            {
                throw new ArgumentNullException("Work item not found");
            }

            var userData = await _userService.GetById(workItem.AssigneeId);

            if (userData == null)
            {
                throw new ArgumentNullException("Assignee not found");
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
