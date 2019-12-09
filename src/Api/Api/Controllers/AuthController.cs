using Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using Api.Auth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        public IActionResult Authorize(UserDto userDto)
        {
            var user = _userService.GetUserByLoginPassword(userDto);
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