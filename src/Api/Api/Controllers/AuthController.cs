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

        /*[Route("Authorize")]
        [HttpPost()]
        public async Task<IActionResult> Authorize([CustomizeValidator(Properties = "Login, Password")] UserDto userDto)
        {
            var user = await _userService.GetUserByLoginAsync(userDto);

            if (user == null)
            {
                return new NotFoundResult();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_config.Value.MinutesLifetime),
                SigningCredentials = new SigningCredentials(
                    AuthConfig.GetKey(_config.Value.SecretKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = tokenHandler.WriteToken(securityToken);

            return new JsonResult(new
            {
                token = jwtToken
            });
        }*/

        [Route("Authorize")]
        [HttpPost("{externalId}")]
        public async Task<IActionResult> Authorize(string externalId)
        {
            var user = await _userService.GetByExternalId(externalId);

            if (user == null)
            {
                return new NotFoundResult();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_config.Value.MinutesLifetime),
                SigningCredentials = new SigningCredentials(
                    AuthConfig.GetKey(_config.Value.SecretKey),
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