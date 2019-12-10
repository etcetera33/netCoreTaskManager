using Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Api.Auth;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("Register")]
        [HttpPost()]
        public async Task<IActionResult> Register(CreateUserDto user)
        {
            await _userService.RegisterUserAsync(user);
            return Ok();
        }

        [Route("getme")]
        [Authorize]
        public IActionResult GetMe()
        {
            return Ok($"Ваш логин: {User.Identity.Name}");
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
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(AuthConfig.MINUTES_LIFETIME),
                SigningCredentials = new SigningCredentials(AuthConfig.GetKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var jwtToken = tokenHandler.WriteToken(securityToken);

            return new OkObjectResult(new
            {
                token = jwtToken
            });
        }
    }
}