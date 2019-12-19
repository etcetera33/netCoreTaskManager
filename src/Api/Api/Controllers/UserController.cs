using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Get()
        {
            var user = await _userService.GetById(int.Parse(User.Identity.Name));

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("dictionary")]
        [Authorize]
        public async Task<IActionResult> GetDictionary()
        {
            return Ok(await _userService.GetUserList());
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRolesDictionary()
        {
            return Ok(await _userService.GetRolesDictionary());
        }
    }
}