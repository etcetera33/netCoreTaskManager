using Core.Configs;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs;
using Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IOptions<AuthConfig> _config;

        public AuthController(IUserService userService, IOptions<AuthConfig> config)
        {
            _userService = userService;
            _config = config;
        }

        [Route("Register")]
        [HttpPost()]
        public async Task<IActionResult> Register(UserDto user)
        {
            var userDto = await _userService.RegisterUserAsync(user);
            return new JsonResult(userDto)
            {
                StatusCode = (int)HttpStatusCode.Created
            };
        }
    }
}