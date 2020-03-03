using Data.Interfaces;
using FluentValidation;
using Models.DTOs;

namespace Services.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.FullName).NotEmpty().Length(1, 40);
            RuleFor(x => x.Email).MustAsync(async (email, cancelletion) =>
            {
                return !(await userRepository.UserWithEmailExists(email));
            }).NotEmpty().Length(1, 50);
            RuleFor(x => x.Position).NotNull().Length(1, 40);
        }
    }
}
