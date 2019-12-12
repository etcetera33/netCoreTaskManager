using Models.DTOs;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(CreateUserDto user);
        Task<UserDto> GetUserByLoginAsync(UserDto user);
        Task<UserDto> GetById(int userId);
        Task Update(int userId, CreateUserDto userDto);
    }
}
