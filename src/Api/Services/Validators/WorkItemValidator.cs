using Data;
using FluentValidation;
using Models.DTOs;

namespace Services.Validators
{
    public class WorkItemValidator : AbstractValidator<WorkItemDto>
    {
        public WorkItemValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.ProjectId).NotEmpty().Must(projectId =>
            {
                return dbContext.Projects.Find(projectId) != null;
            }).WithMessage("Foreign key constraint failure");
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.StatusId).NotEmpty().InclusiveBetween(1, 4);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.AssigneeId).NotEmpty().Must(assigneeId =>
            {
                return dbContext.Users.Find(assigneeId) != null;
            }).WithMessage("Foreign key constraint failure");
            RuleFor(x => x.Priority).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.Progress).NotEmpty().InclusiveBetween(0, 100);
            RuleFor(x => x.WorkItemTypeId).NotEmpty().InclusiveBetween(1, 2);
        }
    }
}
