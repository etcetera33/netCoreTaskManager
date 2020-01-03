using Contracts;
using EntitiesObserver.Helpers;
using EntitiesObserver.Interfaces;
using MassTransit;
using System.Threading.Tasks;

namespace EntitiesObserver.Handlers
{
    class WorkItemChangedHandler: IConsumer<WorkItemChanged>
    {
        private readonly IHtmlBuilder _htmlBuilder;

        public WorkItemChangedHandler(IHtmlBuilder htmlBuilder)
        {
            _htmlBuilder = htmlBuilder;
        }

        public async Task Consume(ConsumeContext<WorkItemChanged> context)
        {
            var email = context.Message.Email;

            email.To = context.Message.AssigneeEmail;
            email.Subject = "Work item assignee";
            email.Body = _htmlBuilder.GetWorkItemChangedEmailString(context.Message.AssigneeFullName, context.Message.WorkItemId, context.Message.Email.DisplayName);

            await Mailer.SendMessage(email) ;
        }
    }
}
