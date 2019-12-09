using Models.DTOs;
using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(CreateUserDto user);
        User LogIn(User user);
        IEnumerable<UserDto> GetAll();
        UserDto GetUserByLoginPassword(UserDto user);
    }
}
