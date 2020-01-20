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
    public class WorkItemDeletedHandlerTests
    {
        private readonly ILoggerAdapter<WorkItemDeletedHandler> _logger;
        private readonly IWorkItemAuditService _workItemAuditService;
        private readonly ConsumeContext<WorkItemDeleted> _context;
        private readonly WorkItemDeletedHandler _handler;

        public WorkItemDeletedHandlerTests()
        {
            _logger = Substitute.For<ILoggerAdapter<WorkItemDeletedHandler>>();
            _workItemAuditService = Substitute.For<IWorkItemAuditService>();

            _context = Substitute.For<ConsumeContext<WorkItemDeleted>>();

            _handler = new WorkItemDeletedHandler(_logger, _workItemAuditService);
        }

        [Theory]
        [InlineData(1)]
        public async Task Should_Successfully_Invoke(int workItemId)
        {
            _context.Message.Returns(new WorkItemDeleted { WorkItemId = workItemId, OldWorkItem = OldWorkItem });

            await _handler.Consume(_context);

            await _workItemAuditService.Received(1).WIDeleted(workItemId, Arg.Is<WorkItemHistoryDto>(value => value == OldWorkItem));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public async Task Should_Not_Recieve_Audit_Service_WorkItemId_Argument_Is_Invalid(int workItemId)
        {
            _context.Message.Returns(new WorkItemDeleted { WorkItemId = workItemId });

            await _handler.Consume(_context);

            await _workItemAuditService.DidNotReceive().WIDeleted(Arg.Any<int>(), Arg.Any<WorkItemHistoryDto>());
        }

        [Fact]
        public async Task Should_Not_Recieve_Audit_Service_Message_Is_Null()
        {
            _context.Message.Returns(value => null);

            await _handler.Consume(_context);

            await _workItemAuditService.DidNotReceive().WIDeleted(Arg.Any<int>(), Arg.Any<WorkItemHistoryDto>());
        }

        #region Helpers
        public WorkItemHistoryDto OldWorkItem => new WorkItemHistoryDto { AssigneeId = 1, Description = "Create footer,Description=Footer must contain a contact information and a fotter menu", Priority = 1, Progress = 1, StatusId = 1, Title = "Title", WorkItemTypeId = 1, AuthorId = 1, ProjectId = 1 };
        #endregion
    }
}
