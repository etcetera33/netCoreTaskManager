using Microsoft.AspNetCore.Mvc;
using Models.QueryParameters;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public TestController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Route("1")]
        public async Task<IActionResult> Get([FromQuery] BaseQueryParameters parameters)
        {
            var returnValue = await _projectService.GetProjects(parameters);

            return Ok(returnValue);
        }

        [HttpGet]
        [Route("2")]
        public async Task<IActionResult> Get()
        {
            return Ok("2222222");
        }
    }
}