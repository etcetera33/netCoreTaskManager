using FluentValidation;
using Models.DTOs.WorkItem;

namespace Models.Validators
{
    public class WorkItemValidator : AbstractValidator<WorkItemDto>
    {
        public WorkItemValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.WorkItemTypeId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.StatusId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.AuthorId).NotEmpty();
            RuleFor(x => x.AssigneeId).NotEmpty();
        }
    }
}
