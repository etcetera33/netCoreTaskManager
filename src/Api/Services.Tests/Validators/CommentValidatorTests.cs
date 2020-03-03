using Data.Interfaces;
using Data.Models;
using FluentValidation.TestHelper;
using NSubstitute;
using Services.Validators;
using System.Threading.Tasks;
using Xunit;

namespace Services.Tests.Validators
{
    public class CommentValidatorTests
    {
        private readonly CommentValidator _validator;
        private readonly IWorkItemRepository _workItemRepository;

        public CommentValidatorTests()
        {
            _workItemRepository = Substitute.For<IWorkItemRepository>();

            _workItemRepository.GetById(1).Returns(new WorkItem());
            _workItemRepository.GetById(0).Returns<WorkItem>(value => null);

            _validator = new CommentValidator(_workItemRepository);
        }

        [Theory]
        [InlineData("Body", 1)]
        public async Task Should_Succeed_Validation(string body, int workItemId)
        {
            _validator.ShouldNotHaveValidationErrorFor(x => x.Body, body);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WorkItemId, workItemId);

            await _workItemRepository.Received(1).GetById(workItemId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Should_Fail_Body_Validation(string body)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Body, body);

            await _workItemRepository.Received(1).GetById(Arg.Any<int>());
        }

        [Theory]
        [InlineData(0)]
        public async Task Should_Fail_WorkItem_Validation(int workItemId)
        {
            _validator.ShouldHaveValidationErrorFor(x => x.WorkItemId, workItemId)
                .WithErrorMessage("Foreign key constraint failure");

            await _workItemRepository.Received(1).GetById(workItemId);
        }
    }
}
