using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services.Interfaces;
using System.Collections.Generic;
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

        [HttpPost]
        [Authorize(Roles = "Developer,Owner")]
        public async Task<IActionResult> Create(ProjectDto projectDto)
        {
            var createdProject = await _projectService.Create(projectDto);

            return new JsonResult(createdProject) { StatusCode = 201} ;
        }

        [HttpGet("{id}", Name = "GetProject")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var project = await _projectService.GetById(id);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Developer,Owner")]
        public async Task<IActionResult> Put(int id, ProjectDto projectDto)
        {
            await _projectService.Update(id, projectDto);

            return NoContent();
        }

        [HttpGet("paginate")]
        [Authorize]
        public async Task<IActionResult> Paginate([FromQuery] int page = 1, [FromQuery] string search = "")
        {
            object returnValue;

            if (string.IsNullOrEmpty(search))
            {
                returnValue = await _projectService.GetPaginatedDataAsync(page);
            }
            else
            {
                returnValue = await _projectService.GetPaginatedDataAsync(page, search);
            }
            
            return Ok(returnValue);
        }
    }
}
