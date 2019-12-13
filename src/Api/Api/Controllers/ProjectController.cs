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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var projects = await _projectService.GetAll();
            return Ok(projects);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectDto projectDto)
        {
            var createdProject = await _projectService.Create(projectDto);

            return new JsonResult(createdProject, new { StatusCode = 201} );
        }

        [HttpGet("{id}", Name = "GetProject")]
        public async Task<IActionResult> Get(int projectId)
        {
            var project = await _projectService.GetById(projectId);

            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProjectDto projectDto)
        {
            await _projectService.Update(id, projectDto);

            return NoContent();
        }

    }
}
