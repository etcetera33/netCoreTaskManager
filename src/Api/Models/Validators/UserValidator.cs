﻿using FluentValidation;
using Models.DTOs;

namespace Models.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().Length(1, 40);
            RuleFor(x => x.Login).NotEmpty().Length(1, 40);
            RuleFor(x => x.Password).NotEmpty().Length(1, 40);
            RuleFor(x => x.Position).NotNull().Length(1, 40);
            RuleFor(x => x.RoleId).NotNull().ExclusiveBetween(1, 3);
        }        
    }
}
