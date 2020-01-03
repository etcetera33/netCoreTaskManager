using Contracts;
using EntitiesObserver.Exceptions;
using MassTransit;
using Services.Interfaces;
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
            int workItemId = context.Message.WorkItemId;

            if (workItemId == 0)
            {
                throw new WorkItemNotProvidedException();
            }

            var workItem = await _workItemService.GetById(workItemId);

            if (workItem == null)
            {
                throw new WorkItemNotFoundException();
            }

            var userData = await _userService.GetById(workItem.AssigneeId);

            if (userData == null)
            {
                throw new UserNotFoundException();
            }

            await _bus.Publish(new WorkItemAssigneeNotify
            {
                To = userData.Email,
                RecieverFullName = userData.FullName,
                WorkItemId = workItemId
            });
        }
    }
}
