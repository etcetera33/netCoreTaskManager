using NSubstitute;
using Xunit;
using Data.Interfaces;
using Data.Models;
using System.Collections.Generic;
using Models.DTOs;
using System.Threading.Tasks;
using AutoMapper;
using Services.Mapper;
using Newtonsoft.Json;

namespace Services.Tests
{
    public class ProjectTests
    {
        private readonly ProjectService _projectService;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectTests()
        {
            _mapper = AutoMapperConfiguration.Configure().CreateMapper();
            _projectRepository = Substitute.For<IProjectRepository>();
            _projectRepository.GetAll().Returns(GetProjectModelList());
            _projectRepository.Create(Arg.Any<Project>()).Returns(GetCreatedModelEntity());

            _projectService = Substitute.For<ProjectService>(_projectRepository, _mapper);
        }

        [Fact]
        public async Task GetAll_Should_Successfully_Return_Collection()
        {
            var actual = await _projectService.GetAll();
            var expected = GetProjectDtoList();

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Theory]
        [InlineData("Desc", "Name", null, 1)]
        public async Task Create_Should_Successfully_Create_Project(string descr, string name, UserDto owner, int ownerId)
        {
            var actual = await _projectService.Create(new ProjectDto
            {
                Description = descr,
                Name = name,
                Owner = owner,
                OwnerId = ownerId
            });
            var expected = GetCreatedDtoEntity();
            
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        #region Helpers
        private static List<Project> GetProjectModelList()
        {
            var list = new List<Project>()
            {
                new Project{Description = "Descr", ProjectId = 1, ProjectName = "Person1", Owner = null, OwnerId = 1}
            };

            return list;
        }

        private static List<ProjectDto> GetProjectDtoList()
        {
            var list = new List<ProjectDto>()
            {
                new ProjectDto { Description = "Descr", Id = 1, Name = "Person1", Owner = null, OwnerId = 1 }
            };

            return list;
        }

        private static ProjectDto GetCreatedDtoEntity()
        {
            return new ProjectDto
            {
                Description = "Descr",
                Name = "Person1",
                Owner = null,
                OwnerId = 1
            };
        }

        private static Project GetCreatedModelEntity()
        {
            return new Project
            {
                Description = "Descr",
                ProjectName = "Person1",
                Owner = null,
                OwnerId = 1
            };
        }
        #endregion
    }
}
