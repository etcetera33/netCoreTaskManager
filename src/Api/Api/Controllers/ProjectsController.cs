using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.QueryParameters;
using Models.DTOs;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] BaseQueryParameters parameters)
        {
            var returnValue = await _projectService.GetPaginatedDataAsync(parameters);

            return Ok(returnValue);
        }

        [HttpPost]
        [Authorize(Roles = "Developer,Owner")]
        public async Task<IActionResult> Create(ProjectDto projectDto)
        {
            projectDto.OwnerId = int.Parse(User.Identity.Name);
            var createdProject = await _projectService.Create(projectDto);

            return new JsonResult(createdProject) { StatusCode = 201} ;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var project = await _projectService.GetById(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Developer,Owner")]
        public async Task<IActionResult> Put(int id, ProjectDto projectDto)
        {
            await _projectService.Update(id, projectDto);

            return NoContent();
        }
    }
}
