using Contracts;
using EntitiesObserver.Handlers;
using MassTransit;
using Models.DTOs;
using NSubstitute;
using Services.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EntitiesObserver.Tests
{
    public class WorkItemChangedHandlerTests
    {
        private readonly IWorkItemService _workItemService;
        private readonly IUserService _userService;
        private readonly IBus _bus;
        private readonly ConsumeContext<WorkItemChanged> _consumeContext;
        private readonly WorkItemChangedHandler _handler;

        public WorkItemChangedHandlerTests()
        {
            _workItemService = Substitute.For<IWorkItemService>();
            _userService = Substitute.For<IUserService>();
            _consumeContext = Substitute.For<ConsumeContext<WorkItemChanged>>();
            _bus = Substitute.For<IBus>();

            _workItemService.GetById(1).Returns(WorkItem);
            _workItemService.GetById(2).Returns(WorkItemWithNotValidAssignee);
            _workItemService.GetById(3).Returns<WorkItemDto>(value => null);

            _userService.GetById(1).Returns(User);
            _userService.GetById(2).Returns<UserDto>(value => null);

            _handler = new WorkItemChangedHandler(_workItemService, _userService, _bus);
        }

        [Theory]
        [InlineData(1)]
        public async Task Should_Successfully_Publish_To_Bus(int workItemId)
        {
            _consumeContext.Message.Returns(new WorkItemChanged { WorkItemId = workItemId });

            await _handler.Consume(_consumeContext);

            await _bus.Received(1).Publish(Arg.Any<EmailSend>());

            await _bus.Received(1).Publish(Arg.Is<EmailSend>(
                x => x.To == User.Email
                    && x.Subject == "New work item assignee"
                    && x.Body == $"Dear, {User.FullName}! You are the new assignee for the work item #{workItemId}"
                ));
        }

        [Fact]
        public async Task Should_Throw_Message_ArgumentNullException()
        {
            _consumeContext.Message.Returns(value => null);

            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Consume(_consumeContext));
        }

        [Theory]
        [InlineData(0)]
        public async Task Should_Throw_Work_Item_Id_ArgumentException(int workItemId)
        {
            _consumeContext.Message.Returns(new WorkItemChanged { WorkItemId = workItemId});

            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Consume(_consumeContext));
        }

        [Theory]
        [InlineData(3)]
        public async Task Should_Throw_Work_Item_ArgumentException(int workItemId)
        {
            _consumeContext.Message.Returns(new WorkItemChanged { WorkItemId = workItemId });

            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Consume(_consumeContext));
        }

        [Theory]
        [InlineData(2)]
        public async Task Should_Throw_User_ArgumentNullException(int workItemId)
        {
            _consumeContext.Message.Returns(new WorkItemChanged { WorkItemId = workItemId });

            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Consume(_consumeContext));
        }

        #region Helpers
        public WorkItemDto WorkItem => new WorkItemDto { Id = 1, Title = "Project 1", Description = "Descr of Project 1", ProjectId = 1, Priority = 10, AssigneeId = 1, Progress = 10, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null };
        public WorkItemDto WorkItemWithNotValidAssignee => new WorkItemDto { Id = 1, Title = "Project 1", Description = "Descr of Project 1", ProjectId = 1, Priority = 10, AssigneeId = 2, Progress = 10, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null };
        public UserDto User => new UserDto { Id = 1, Email = "test@test.com", FullName = "Test Name", Login = "test.user", Position = "Test Position", RoleId = 1};
        public WorkItemChanged WorkItemChanged => new WorkItemChanged { WorkItemId = 1 };
        #endregion
    }
}
