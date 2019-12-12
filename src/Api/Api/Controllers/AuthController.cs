using Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Api.Auth;
using System.Net;
using Microsoft.Extensions.Options;

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
        public async Task<IActionResult> Register(CreateUserDto user)
        {
            var userDto = await _userService.RegisterUserAsync(user);
            return new JsonResult(userDto)
            {
                StatusCode = (int) HttpStatusCode.Created
            };
        }

        [Route("Authorize")]
        [HttpPost()]
        public async Task<IActionResult> Authorize(UserDto userDto)
        {
            var user = await _userService.GetUserByLoginAsync(userDto);
            if (user == null)
            {
                return new NotFoundResult();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(((AuthConfig)_config.Value).MinutesLifetime),
                SigningCredentials = new SigningCredentials(
                    AuthConfig.GetKey(((AuthConfig)_config.Value).SecretKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = tokenHandler.WriteToken(securityToken);

            return new JsonResult(new
            {
                token = jwtToken
            });
        }
    }
}