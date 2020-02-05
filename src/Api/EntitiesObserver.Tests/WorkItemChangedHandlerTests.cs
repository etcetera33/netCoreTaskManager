using Contracts;
using Core.Adapters;
using EntitiesObserver.Handlers;
using MassTransit;
using Models.DTOs;
using NSubstitute;
using Services.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace EntitiesObserver.Tests
{
    public class WorkItemChangedHandlerTests
    {
        private readonly IWorkItemService _workItemService;
        private readonly IUserService _userService;
        private readonly IBus _bus;
        private readonly ConsumeContext<WorkItemUpdated> _consumeContext;
        private readonly WorkItemChangedHandler _handler;
        private readonly ILoggerAdapter<WorkItemChangedHandler> _logger;
        private readonly IWorkItemAuditService _workItemAuditService;

        public WorkItemChangedHandlerTests()
        {
            _workItemService = Substitute.For<IWorkItemService>();
            _userService = Substitute.For<IUserService>();
            _consumeContext = Substitute.For<ConsumeContext<WorkItemUpdated>>();
            _bus = Substitute.For<IBus>();
            _logger = Substitute.For<ILoggerAdapter<WorkItemChangedHandler>>();
            _workItemAuditService = Substitute.For<IWorkItemAuditService>();

            _workItemService.GetById(1).Returns(WorkItem);
            _workItemService.GetById(2).Returns(WorkItemWithNotValidAssignee);
            _workItemService.GetById(3).Returns<WorkItemDto>(value => null);

            _userService.GetById(1).Returns(User);
            _userService.GetById(2).Returns<UserDto>(value => null);


            _handler = new WorkItemChangedHandler(_workItemService, _userService, _bus, _logger, _workItemAuditService);
        }

        [Theory]
        [InlineData(1)]
        public async Task Should_Publish_And_Update_Entity(int workItemId)
        {
            _consumeContext.Message.Returns(new WorkItemUpdated { WorkItemId = workItemId, OldWorkItem = OldWorkItem, NewWorkItem = CompletelyNewWorkItem });

            await _handler.Consume(_consumeContext);

            await _bus.Received(1).Publish(Arg.Is<EmailSend>(
               x => x.To == User.Email
                   && x.Subject == "New work item assignee"
                   && x.Body == $"Dear, {User.FullName}! You are the new assignee for the work item #{workItemId}"
               ));

            await _workItemAuditService.Received(1).WIUpdated(workItemId, Arg.Any<WorkItemHistoryDto>(), Arg.Any<WorkItemHistoryDto>());
        }

        [Theory]
        [InlineData(1)]
        public async Task Should_Not_Publish_But_Should_Update_Entity(int workItemId)
        {
            _consumeContext.Message.Returns(new WorkItemUpdated { WorkItemId = workItemId, OldWorkItem = OldWorkItem, NewWorkItem = NewWorkItemWithSameAssignee });

            await _handler.Consume(_consumeContext);

            await _bus.DidNotReceive().Publish(Arg.Any<EmailSend>());

            await _workItemAuditService.Received(1).WIUpdated(workItemId, Arg.Any<WorkItemHistoryDto>(), Arg.Any<WorkItemHistoryDto>());
        }

        [Theory]
        [InlineData(1)]
        public async Task Should_Not_Publish_And_Update_Entity(int workItemId)
        {
            _consumeContext.Message.Returns(new WorkItemUpdated { WorkItemId = workItemId, OldWorkItem = OldWorkItem, NewWorkItem = IdenticalNewWorkItem });

            await _handler.Consume(_consumeContext);

            await _bus.DidNotReceive().Publish(Arg.Any<EmailSend>());

            await _workItemAuditService.DidNotReceive().WIUpdated(Arg.Any<int>(), Arg.Any<WorkItemHistoryDto>(), Arg.Any<WorkItemHistoryDto>());
        }

        [Fact]
        public async Task Should_Throw_Message_ArgumentNullException()
        {
            _consumeContext.Message.Returns(value => null);
            await _handler.Consume(_consumeContext);

            await _workItemService.DidNotReceive().GetById(Arg.Any<int>());

            await _userService.DidNotReceive().GetById(Arg.Any<int>());

            await _bus.DidNotReceive().Publish(Arg.Any<EmailSend>());

            await _workItemAuditService.DidNotReceive().WIUpdated(Arg.Any<int>(), Arg.Any<WorkItemHistoryDto>(), Arg.Any<WorkItemHistoryDto>());
        }

        [Theory]
        [InlineData(0)]
        public async Task Should_Throw_Work_Item_Id_ArgumentException(int workItemId)
        {
            _consumeContext.Message.Returns(new WorkItemUpdated { WorkItemId = workItemId });
            await _handler.Consume(_consumeContext);

            await _workItemService.Received(1).GetById(workItemId);

            await _userService.DidNotReceive().GetById(Arg.Any<int>());

            await _bus.DidNotReceive().Publish(Arg.Any<EmailSend>());

            await _workItemAuditService.DidNotReceive().WIUpdated(Arg.Any<int>(), Arg.Any<WorkItemHistoryDto>(), Arg.Any<WorkItemHistoryDto>());
        }

        [Theory]
        [InlineData(3)]
        public async Task Should_Throw_Work_Item_ArgumentException(int workItemId)
        {
            _consumeContext.Message.Returns(new WorkItemUpdated { WorkItemId = workItemId });
            await _handler.Consume(_consumeContext);

            await _workItemService.Received(1).GetById(workItemId);

            await _userService.DidNotReceive().GetById(Arg.Any<int>());

            await _bus.DidNotReceive().Publish(Arg.Any<EmailSend>());

            await _workItemAuditService.DidNotReceive().WIUpdated(Arg.Any<int>(), Arg.Any<WorkItemHistoryDto>(), Arg.Any<WorkItemHistoryDto>());
        }

        [Theory]
        [InlineData(2)]
        public async Task Should_Throw_User_ArgumentNullException(int workItemId)
        {
            _consumeContext.Message.Returns(new WorkItemUpdated { WorkItemId = workItemId });
            await _handler.Consume(_consumeContext);

            await _workItemService.Received(1).GetById(workItemId);

            await _userService.Received(1).GetById(WorkItemWithNotValidAssignee.AssigneeId);

            await _bus.DidNotReceive().Publish(Arg.Any<EmailSend>());

            await _workItemAuditService.DidNotReceive().WIUpdated(Arg.Any<int>(), Arg.Any<WorkItemHistoryDto>(), Arg.Any<WorkItemHistoryDto>());
        }

        #region Helpers
        public WorkItemDto WorkItem => new WorkItemDto { Id = 1, Title = "Project 1", Description = "Descr of Project 1", ProjectId = 1, Priority = 10, AssigneeId = 1, Progress = 10, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null };
        public WorkItemDto WorkItemWithNotValidAssignee => new WorkItemDto { Id = 1, Title = "Project 1", Description = "Descr of Project 1", ProjectId = 1, Priority = 10, AssigneeId = 2, Progress = 10, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null };
        public WorkItemHistoryDto OldWorkItem => new WorkItemHistoryDto { AssigneeId = 1, Description = "Create footer,Description=Footer must contain a contact information and a fotter menu", Priority = 1, Progress = 1, StatusId = 1, Title = "Title", WorkItemTypeId = 1, AuthorId = 1, ProjectId = 1 };
        public WorkItemHistoryDto IdenticalNewWorkItem => new WorkItemHistoryDto { AssigneeId = 1, Description = "Create footer,Description=Footer must contain a contact information and a fotter menu", Priority = 1, Progress = 1, StatusId = 1, Title = "Title", WorkItemTypeId = 1, AuthorId = 1, ProjectId = 1 };
        public WorkItemHistoryDto NewWorkItemWithSameAssignee => new WorkItemHistoryDto { AssigneeId = 1, Description = "Create header", Priority = 2, Progress = 2, StatusId = 2, Title = "Title2", WorkItemTypeId = 1, AuthorId = 1, ProjectId = 1 };
        public WorkItemHistoryDto CompletelyNewWorkItem => new WorkItemHistoryDto { AssigneeId = 2, Description = "Create header", Priority = 2, Progress = 2, StatusId = 2, Title = "Title2", WorkItemTypeId = 1, AuthorId = 1, ProjectId = 1 };
        public UserDto User => new UserDto { Id = 1, Email = "test@test.com", FullName = "Test Name", Position = "Test Position", RoleId = 1 };
        public WorkItemUpdated WorkItemChanged => new WorkItemUpdated { WorkItemId = 1 };
        #endregion
    }
}
