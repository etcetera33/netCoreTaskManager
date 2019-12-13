using AutoMapper;
using Models.DTOs;
using Data;
using Data.Models;
using Services.Interfaces;
using System.Threading.Tasks;
using Services.Helpers;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordhasher;

        public UserService(IUnitOfWork unitOfWork, IMapper imapper, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = imapper;
            _passwordhasher = passwordHasher;
        }
        public async Task<UserDto> RegisterUserAsync(UserDto userDto)
        {
            userDto.Password = _passwordhasher.Hash(userDto.Password); 
            var user= _mapper.Map<UserDto, User>(userDto);
            var createdEntity = await _unitOfWork.UserRepository.Create(user);

            return _mapper.Map<User, UserDto>(createdEntity);
        }

        public async Task<UserDto> GetUserByLoginAsync(UserDto userDto)
        {
            var user = await _unitOfWork.UserRepository.FindUserByLoginAsync(userDto.Login);

            if (user == null || user.Password != _passwordhasher.Hash(userDto.Password))
            {
                return null;
            }

            return _mapper.Map<User, UserDto>(user);
        }

        public async Task<UserDto> GetById(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId);
            var userDto = _mapper.Map<User, UserDto>(user);

            return userDto;
        }

        public async Task Update(int userId, UserDto userDto)
        {
            var user = _mapper.Map<UserDto, User>(userDto);
            await _unitOfWork.UserRepository.Update(userId, user);
        }
    }
}
