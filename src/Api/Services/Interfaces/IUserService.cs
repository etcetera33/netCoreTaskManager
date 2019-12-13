﻿using Models.DTOs;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(UserDto user);
        Task<UserDto> GetUserByLoginAsync(UserDto user);
        Task<UserDto> GetById(int userId);
        Task Update(int userId, UserDto userDto);
    }
}
