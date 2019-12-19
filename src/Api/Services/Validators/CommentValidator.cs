using Data;
using FluentValidation;
using Models.DTOs;

namespace Services.Validators
{
    public class CommentValidator : AbstractValidator<CommentDto>
    {
        public CommentValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.Body).NotEmpty().Length(1, 200);
            RuleFor(x => x.WorkItemId).NotEmpty().Must(WorkItemId =>
            {
                return dbContext.WorkItems.Find(WorkItemId) != null;
            }).WithMessage("Foreign key constraint failure");
        }
    }
}
