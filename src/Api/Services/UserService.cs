using AutoMapper;
using Models.DTOs;
using Data;
using Data.Models;
using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Mapper;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper imapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = AutoMapperConfiguration.Configure().CreateMapper();
        }
        public User LogIn(User user)
        {
            throw new System.NotImplementedException();
        }
        public async Task RegisterUserAsync(CreateUserDto userDto)
        {
            var user= _mapper.Map<CreateUserDto, User>(userDto);

            await _unitOfWork.UserRepository.Create(user);
        }
        public IEnumerable<UserDto> GetAll()
        {
            var list = _unitOfWork.UserRepository.GetAll();
            var newList = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(list);
            return newList;
        }

        public UserDto GetUserByLoginPassword(UserDto userDto)
        {
            var user = _unitOfWork.UserRepository.FindUserByLoginPassword(userDto.Login, userDto.Password);
            return _mapper.Map<User, UserDto>(user); 
        }
    }
}
