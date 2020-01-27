using AutoMapper;
using Core.Enums;
using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Options;
using Models.DTOs;
using Services.Helpers;
using Services.Interfaces;
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
            userDto.Password = PasswordHasher.Hash(userDto.Password,
                _passwordHasher.Value.Salt,
                _passwordHasher.Value.IterationCount,
                _passwordHasher.Value.BytesRequested);
            var user = _mapper.Map<UserDto, User>(userDto);
            var createdEntity = await _userRepository.Create(user);

            return _mapper.Map<User, UserDto>(createdEntity);
        }

        public async Task<UserDto> GetUserByLoginAsync(UserDto userDto)
        {
            var user = await _userRepository.FindUserByLoginAsync(userDto.Login);

            if (user == null)
            {
                return null;
            }

            var isPasswordMatch = PasswordHasher.PasswordHashValid(userDto.Password, user.Password, _passwordHasher.Value.Salt, _passwordHasher.Value.IterationCount, _passwordHasher.Value.BytesRequested);

            if (!isPasswordMatch)
            {
                return null;
            }

            return _mapper.Map<User, UserDto>(user);
        }

        public async Task<UserDto> GetById(int userId)
        {
            var user = await _userRepository.GetById(userId);
            var userDto = _mapper.Map<User, UserDto>(user);

            return userDto;
        }

        public async Task Update(int userId, UserDto userDto)
        {
            var user = _mapper.Map<UserDto, User>(userDto);
            await _userRepository.Update(userId, user);
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
    }
}
