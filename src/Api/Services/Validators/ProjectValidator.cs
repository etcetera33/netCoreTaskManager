using FluentValidation;
using Models.DTOs;

namespace Services.Validators
{
    public class ProjectValidator : AbstractValidator<ProjectDto>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 50);
        }
    }
}
