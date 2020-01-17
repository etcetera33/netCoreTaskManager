using Contracts;
using Core.Adapters;
using EntitiesObserver.Handlers;
using MassTransit;
using Models.DTOs;
using NSubstitute;
using Services.Interfaces;
using System.Threading.Tasks;
using Xunit;
using Core.Enums;

namespace EntitiesObserver.Tests
{
    public class WorkItemCreatedHandlerTests
    {
        private readonly IWorkItemAuditService _workItemAuditService;
        private readonly ILoggerAdapter<WorkItemCreatedHandler> _logger;
        private readonly IUserService _userService;
        private readonly IBus _bus;
        private readonly WorkItemCreatedHandler _handler;
        private readonly ConsumeContext<WorkItemCreated> _context;

        public WorkItemCreatedHandlerTests()
        {
            _workItemAuditService = Substitute.For<IWorkItemAuditService>();
            _logger = Substitute.For<ILoggerAdapter<WorkItemCreatedHandler>>();
            _userService = Substitute.For<IUserService>();
            _bus = Substitute.For<IBus>();
            _context = Substitute.For<ConsumeContext<WorkItemCreated>>();

            
            _userService.GetById(1).Returns(UserDto);
            _userService.GetById(2).Returns<UserDto>(value => null);

            _workItemAuditService.Create(Arg.Any<int>(), Arg.Any<WIAuditStatuses>(), Arg.Any<WorkItemHistoryDto>()).Returns(WorkItemAuditDto);

            _handler = new WorkItemCreatedHandler(_workItemAuditService, _logger, _userService, _bus);
        }

        [Theory]
        [InlineData(1, 1)]
        public async Task Should_SuccessfullyInvoke(int userId, int workItemId)
        {
            _context.Message.Returns(new WorkItemCreated { NewWorkItem = ValidNewWorkItem, WorkItemId = workItemId });

            await _handler.Consume(_context);

            await _userService.Received(1).GetById(userId);

            await _bus.Received(1).Publish(Arg.Is<EmailSend>(x => x.Body == $"Dear, {UserDto.FullName}! You are the new assignee for the work item #{workItemId}" && x.To == UserDto.Email && x.Subject == "New work item assignee"));

            //_logger.Received(1).Information($"Bus published EmailSend contract with email: {UserDto.Email}. WorkItemId: {workItemId}");
            _logger.Received(2).Information(Arg.Any<string>());

            //_logger.Received(1).Information($"Successfully logged work item creation. WorkItemAuditId: {WorkItemAuditDto.Id}");
        }

        #region Helpers
        public WorkItemHistoryDto ValidNewWorkItem => new WorkItemHistoryDto { AssigneeId = 1, Description = "Create footer,Description=Footer must contain a contact information and a fotter menu", Priority = 1, Progress = 1, StatusId = 1, Title = "Title", WorkItemTypeId = 1, AuthorId = 1, ProjectId = 1 };
        public WorkItemHistoryDto UnValidNewWorkItemWithRightAssignee => new WorkItemHistoryDto { AssigneeId = 2, };
        public UserDto UserDto => new UserDto { Id = 1, Email = "test@gmail.com", FullName = "Test FullName", Login = "test.login", Password = "111111", Position = "Developer", RoleId = 1 };
        public WorkItemAuditDto WorkItemAuditDto => new WorkItemAuditDto { Id = 1, NewWorkItem = null, OldWorkItem = null, StatusId = 1, WorkItemId = 1 };
        #endregion
    }
}
