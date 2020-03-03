using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.QueryParameters;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWorkItemService _workItemService;
        public UsersController(IUserService userService, IWorkItemService workItemService)
        {
            _userService = userService;
            _workItemService = workItemService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get([FromQuery] BaseQueryParameters queryParameters)
        {
            var user = await _userService.Paginate(queryParameters);

            return Ok(user);
        }

        [Authorize]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetById(id);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(int id, ModerateUserDto userDto)
        {
            await _userService.Update(id, userDto);

            return NoContent();
        }

        [Authorize]
        [Route("current")]
        public async Task<IActionResult> GetCurrent()
        {
            var user = await _userService.GetById(int.Parse(User.Identity.Name));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("current/work-items")]
        [Authorize]
        public async Task<IActionResult> GetWorkItems()
        {
            var currentUserId = int.Parse(User.Identity.Name);

            return Ok(await _workItemService.GetTopFivePriorityItems(currentUserId));
        }

        [HttpGet("dictionary")]
        [Authorize]
        public async Task<IActionResult> GetDictionary()
        {
            return Ok(await _userService.GetUserList());
        }

        [HttpGet("roles")]
        public IActionResult GetRolesDictionary()
        {
            return Ok(_userService.GetRolesDictionary());
        }
    }
}