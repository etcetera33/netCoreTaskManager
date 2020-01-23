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
using Models.QueryParameters;
using Models.PaginatedResponse;
using Services.Interfaces;

namespace Services.Tests
{
    public class ProjectServiceTests
    {
        private readonly ProjectService _projectService;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectServiceTests()
        {
            _mapper = AutoMapperConfiguration.Configure().CreateMapper();

            _projectRepository = Substitute.For<IProjectRepository>();
            _projectRepository.Create(Arg.Any<Project>()).Returns(ProjectModel);
            _projectRepository.PaginateFiltered(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>()).Returns(ProjectModelList);
            _projectRepository.GetFilteredDataCountAsync(Arg.Any<string>()).Returns(1);

            _projectRepository.GetById(1).Returns(ProjectModel);
            _projectRepository.GetById(2).Returns<Project>(val => null);

            var _redis = Substitute.For<IRedisService>();

            _projectService = new ProjectService(_projectRepository, _redis, _mapper);
        }

        [Fact]
        public async Task Create_Should_Successfully_Map_Created_Project()
        {
            var actual = await _projectService.Create(new ProjectDto
                {
                    Description = "Descr",
                    Name = "Person1",
                    Owner = null,
                    OwnerId = 1
                });

            var expected = new ProjectDto
            {
                Description = "Descr",
                Name = "Person1",
                Owner = null,
                OwnerId = 1,
                Id = 1
            };

            await _projectRepository.Received(1).Create(Arg.Is<Project>(
                project => project.ProjectName == ProjectModel.ProjectName
                    && project.Owner == ProjectModel.Owner
                    && project.OwnerId == ProjectModel.OwnerId
                    && project.WorkItems == ProjectModel.WorkItems
                ));
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Theory]
        [InlineData(1)]
        public async Task GetById_Should_Return_Proper_Project(int projectId)
        {
            var expected = ProjectDto;
            var actual = await _projectService.GetById(projectId);

            await _projectRepository.Received(1).GetById(Arg.Any<int>());
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Theory]
        [InlineData(0)]
        public async Task GetById_Should_Return_Null(int projectId)
        {
            var actual = await _projectService.GetById(projectId);

            await _projectRepository.DidNotReceive().GetById(Arg.Any<int>());
            Assert.Null(actual);
        }

        [Theory]
        [InlineData(1)]
        public async Task Update_Should_Recieve_Proper_Data(int projectId)
        {
            await _projectService.Update(projectId, ProjectDto);

            await _projectRepository.Received(1).Update(projectId, Arg.Is<Project>(
                proj => ProjectModel.ProjectName == proj.ProjectName
                    && ProjectModel.Owner == proj.Owner
                    && ProjectModel.OwnerId == proj.OwnerId
                    && ProjectModel.ProjectId == proj.ProjectId
                    && ProjectModel.Description == proj.Description
                )
            );
        }

        [Theory]
        [InlineData(1)]
        public async Task ProjectExists_Should_Return_True(int projectId)
        {
            var exists = await _projectService.ProjectExists(projectId);

            await _projectRepository.Received(1).GetById(projectId);
            Assert.True(exists);
        }

        [Theory]
        [InlineData(2)]
        public async Task ProjectExists_Should_Return_False(int projectId)
        {
            var exists = await _projectService.ProjectExists(projectId);

            await _projectRepository.Received(1).GetById(projectId);
            Assert.False(exists);
        }

        [Theory]
        [InlineData(10, 1, "")]
        public async Task GetPaginatedDataAsync_Should_Return_Proper_Values(int itemsPerPage, int page, string search)
        {
            var expected = new BasePaginatedResponse<ProjectDto>
            {
                EntityList = ProjectDtoList,
                PagesCount = 1
            };
            var actual = await _projectService.GetProjects(new BaseQueryParameters { ItemsPerPage = itemsPerPage, Page = page, Search = search });

            await _projectRepository.Received(1).GetFilteredDataCountAsync(Arg.Any<string>());
            await _projectRepository.Received(1).PaginateFiltered(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>());
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        #region Helpers
        public ProjectDto ProjectDto => new ProjectDto { Description = "Descr", Name = "Person1", Owner = null, OwnerId = 1, Id = 1 };
        public Project ProjectModel => new Project { Description = "Descr", ProjectName = "Person1", Owner = null, OwnerId = 1, ProjectId = 1 };
        public List<ProjectDto> ProjectDtoList => new List<ProjectDto> { new ProjectDto { Description = "Descr", Name = "Person1", Owner = null, OwnerId = 1, Id = 1 } };
        public List<Project> ProjectModelList => new List<Project> { new Project { Description = "Descr", ProjectName = "Person1", Owner = null, OwnerId = 1, ProjectId = 1 } };
        #endregion
    }
}
