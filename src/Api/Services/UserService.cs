﻿using AutoMapper;
using Models.DTOs;
using Data;
using Data.Models;
using Services.Interfaces;
using System.Threading.Tasks;
using Services.Helpers;
using System.Collections.Generic;
using Core.Enums;
using Microsoft.Extensions.Options;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOptions<PasswordHasher> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PasswordHasher> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> RegisterUserAsync(UserDto userDto)
        {
            userDto.Password = PasswordHasher.Hash(userDto.Password,
                _passwordHasher.Value.Salt,
                _passwordHasher.Value.IterationCount,
                _passwordHasher.Value.BytesRequested);
            var user= _mapper.Map<UserDto, User>(userDto);
            var createdEntity = await _unitOfWork.UserRepository.Create(user);

            return _mapper.Map<User, UserDto>(createdEntity);
        }

        public async Task<UserDto> GetUserByLoginAsync(UserDto userDto)
        {
            var user = await _unitOfWork.UserRepository.FindUserByLoginAsync(userDto.Login);
            var isPasswordMatch = PasswordHasher.PasswordHashValid(userDto.Password, user.Password, _passwordHasher.Value.Salt, _passwordHasher.Value.IterationCount, _passwordHasher.Value.BytesRequested);

            if (user == null || !isPasswordMatch)
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

        public async Task<IEnumerable<UserDictionaryDto>> GetUserList()
        {
            var userList = await _unitOfWork.UserRepository.GetAll();

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
