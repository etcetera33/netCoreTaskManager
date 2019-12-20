﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Linq;
using System.Security.Claims;
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

        [Authorize]
        public async Task<IActionResult> Get()
        {
            var user = await _userService.GetById(int.Parse(User.Identity.Name));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("current/work-items")]
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

        [HttpGet("role")]
        public IActionResult GetRole()
        {
            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var roleClaimType = userIdentity.RoleClaimType;

            var role = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault();
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role.Value);
        }

        [HttpGet("roles")]
        public IActionResult GetRolesDictionary()
        {
            return Ok(_userService.GetRolesDictionary());
        }
    }
}