using Data;
using Data.Interfaces;
using FluentValidation;
using Models.DTOs;

namespace Services.Validators
{
    public class CommentValidator : AbstractValidator<CommentDto>
    {
        public CommentValidator(IWorkItemRepository workItemRepository)
        {
            RuleFor(x => x.Body).NotEmpty().Length(1, 200);
            RuleFor(x => x.WorkItemId).NotEmpty()
                .Must(workItemId => workItemRepository.GetById(workItemId).Result != null)
                .WithMessage("Foreign key constraint failure");
        }
    }
}
