using FluentValidation;
using Models.DTOs;

namespace Models.Validators
{
    public class WorkItemValidator : AbstractValidator<WorkItemDto>
    {
        public WorkItemValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.StatusId).NotEmpty().InclusiveBetween(1, 4);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.AssigneeId).NotEmpty();
            RuleFor(x => x.Priority).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.Progress).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.WorkItemTypeId).NotEmpty().InclusiveBetween(1, 2);
        }
    }
}
