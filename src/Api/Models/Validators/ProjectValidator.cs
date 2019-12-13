using FluentValidation;
using Models.DTOs;

namespace Models.Validators
{
    public class ProjectValidator : AbstractValidator<ProjectDto> 
    {
        public ProjectValidator()
        {
            RuleFor(x => x.Name).Length(1, 30);
            RuleFor(x => x.Slug).Length(1, 30);
        }
    }
}
