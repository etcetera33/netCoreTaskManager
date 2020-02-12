using AutoMapper;
using Core.Enums;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Options;
using Models.DTOs;
using Models.PaginatedResponse;
using Models.QueryParameters;
using Services.Helpers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<PasswordHasher> _passwordHasher;

        public UserService(IUserRepository userRepository, IMapper mapper, IOptions<PasswordHasher> passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> RegisterUserAsync(UserDto userDto)
        {
            var user = _mapper.Map<UserDto, User>(userDto);
            var createdEntity = await _userRepository.Create(user);

            return _mapper.Map<User, UserDto>(createdEntity);
        }

        public async Task<UserDto> GetById(int userId)
        {
            var user = await _userRepository.GetById(userId);
            var userDto = _mapper.Map<User, UserDto>(user);

            return userDto;
        }

        public async Task<IEnumerable<UserDictionaryDto>> GetUserList()
        {
            var userList = await _userRepository.GetAll();

            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDictionaryDto>>(userList);
        }

        public IEnumerable<object> GetRolesDictionary()
        {
            var enumRoles = new List<object>();

            foreach (var item in System.Enum.GetValues(typeof(Roles)))
            {

                enumRoles.Add(new
                {
                    Id = (int)item,
                    Name = item.ToString()
                });
            }

            return enumRoles;
        }

        public async Task<UserDto> GetByExternalId(string externalId)
        {
            var user = await _userRepository.GetByExternalId(externalId);

            return _mapper.Map<User, UserDto>(user);
        }

        public async Task<BasePaginatedResponse<UserDto>> Paginate(BaseQueryParameters parameters)
        {
            var userList = await _userRepository.Paginate(
            offset: (parameters.Page - 1) * parameters.ItemsPerPage,
            itemsCount: parameters.ItemsPerPage,
            search: parameters.Search
            );

            var userDtoList = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(userList);
            var rowsCount = await _userRepository.GetFilteredDataCountAsync(parameters.Search);

            var pagesCount = (int)Math.Ceiling((decimal)rowsCount / parameters.ItemsPerPage);

            return new BasePaginatedResponse<UserDto>
            {
                EntityList = userDtoList,
                PagesCount = pagesCount
            };
        }

        public async Task Update(int id, ModerateUserDto userDto)
        {
            await _userRepository.Patch(id, userDto);
        }
    }
}
