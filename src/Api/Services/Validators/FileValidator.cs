using FluentValidation;
using Models.DTOs;

namespace Services.Validators
{
    public class FileValidator : AbstractValidator<FileDto>
    {
        public FileValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 25);
            RuleFor(x => x.Path).NotEmpty().Length(10, 80);
        }
    }
}
