using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Project;
using Models.DTOs.ProjectRole;
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
        public async Task<IActionResult> Create(CreateProjectDto projectDto)
        {
            await _projectService.Create(projectDto);
            return Ok();
        }

        [HttpGet("{id}", Name = "GetProject")]
        public async Task<IActionResult> Get(int projectId)
        {
            var project = await _projectService.GetById(projectId);
            return new OkObjectResult(project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CreateProjectDto projectDto)
        {
            await _projectService.Update(id, projectDto);
            return Ok();
        }

        [HttpGet()]
        [Route("{projectId:int}/Roles")]
        public async Task<IActionResult> GetRoles(int projectId)
        {
            var roles = await _projectService.GetProjectRoles(projectId);

            return Ok(roles);
        }

        [HttpPost]
        [Route("Roles/Create")]
        public async Task<IActionResult> CreateRole(CreateProjectRoleDto roleDto)
        {
            await _projectService.GiveRole(roleDto);

            return Ok();
        }

        [HttpPut]
        [Route("Roles/{projectRoleId:int}")]
        public async Task<IActionResult> UpdateRole(int projectRoleId, ProjectRoleDto roleDto)
        {
            await _projectService.ChangeRole(projectRoleId, roleDto);

            return Ok();
        }
    }
}
