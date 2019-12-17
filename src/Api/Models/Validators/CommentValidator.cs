using FluentValidation;
using Models.DTOs;

namespace Models.Validators
{
    public class CommentValidator : AbstractValidator<CommentDto>
    {
        public CommentValidator()
        {
            RuleFor(x => x.Body).NotEmpty().Length(1, 200);
            RuleFor(x => x.WorkItemId).NotEmpty();
            RuleFor(x => x.AuthorId).NotEmpty();
        }

    }
}
