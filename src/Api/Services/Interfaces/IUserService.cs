using Models.DTOs;
using Models.PaginatedResponse;
using Models.QueryParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(UserDto user);
        Task<UserDto> GetById(int userId);
        Task<IEnumerable<UserDictionaryDto>> GetUserList();
        IEnumerable<object> GetRolesDictionary();
        Task<UserDto> GetByExternalId(string externalId);
        Task<BasePaginatedResponse<UserDto>> Paginate(BaseQueryParameters parameters);
        Task Update(int id, ModerateUserDto userDto);
    }
}
