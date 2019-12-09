using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Project;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        // GET: api/Project
        [HttpGet]
        public IActionResult Get()
        {
            var projects = _projectService.GetAll();

            return new OkObjectResult(projects);
        }

        [HttpPost]
        [Route("/Create")]
        public async Task<IActionResult> Create(CreateProjectDto projectDto)
        {
            await _projectService.Create(projectDto);
            return Ok();
        }
    }
}
