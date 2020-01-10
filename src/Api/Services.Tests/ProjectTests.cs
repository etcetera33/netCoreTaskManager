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
            _projectRepository.GetAll().Returns(ProjectModelList);
            _projectRepository.Create(Arg.Any<Project>()).Returns(ProjectModel);
            _projectRepository.GetById(Arg.Any<int>()).Returns(ProjectModel);
            _projectRepository.PaginateFiltered(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>()).Returns(ProjectModelList);
            _projectRepository.GetFilteredDataCountAsync(Arg.Any<string>()).Returns(1);
            
            _projectService = Substitute.For<ProjectService>(_projectRepository, _mapper);
        }

        [Fact]
        public async Task GetAll_Should_Successfully_Return_Collection()
        {
            var actual = await _projectService.GetAll();
            var expected = ProjectDtoList;

            await _projectRepository.Received(1).GetAll();
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact]
        public async Task GetAll_Should_Be_Recieved_Once()
        {
            await _projectService.GetAll();

            await _projectRepository.Received(1).GetAll();
        }

        [Fact]
        public async Task Create_Should_Successfully_Create_Project()
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

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Fact]
        public async Task Create_Should_Be_Recieved_Once()
        {
            await _projectService.Create(ProjectDto);

            await _projectRepository.Received(1).Create(Arg.Any<Project>());
        }

        [Fact]
        public async Task Create_Should_Call_Repo_With_Correct_Args()
        {
            await _projectService.Create(ProjectDto);

            await _projectRepository.Received().Create(Arg.Is<Project>(
                project => project.ProjectName == ProjectModel.ProjectName
                    && project.Owner == ProjectModel.Owner
                    && project.OwnerId == ProjectModel.OwnerId
                    && project.WorkItems == ProjectModel.WorkItems
                ));
        }

        [Theory]
        [InlineData(1)]
        public async Task GetById_Should_Return_Proper_Project(int projectId)
        {
            var expected = ProjectDto;
            var actual = await _projectService.GetById(projectId);

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Theory]
        [InlineData(1)]
        public async Task GetById_Should_Should_Be_Recieved_Once(int projectId)
        {
            await _projectService.GetById(projectId);

            await _projectRepository.Received(1).GetById(Arg.Any<int>());
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

            Assert.True(exists);
        }

        [Theory]
        [InlineData(1123)]
        public async Task ProjectExists_Should_Return_False(int projectId)
        {
            var projectRepo = _projectRepository;
            projectRepo.GetById(Arg.Any<int>()).Returns<Project>(value => null);

            var service = Substitute.For<ProjectService>(projectRepo, _mapper);

            var exists = await service.ProjectExists(projectId);

            Assert.False(exists);
        }

        [Theory]
        [InlineData(1)]
        public async Task ProjectExists_Should_Be_Recieved_Once(int projectId)
        {
            await _projectService.ProjectExists(projectId);

            await _projectRepository.Received(1).GetById(projectId);
        }

        [Theory]
        [InlineData(1, 1, "")]
        public async Task GetPaginatedDataAsync_Should_Return_Proper_Values(int itemsPerPage, int page, string search)
        {
            var expected = new BasePaginatedResponse<ProjectDto>
            {
                EntityList = ProjectDtoList,
                PagesCount = 1
            };
            var actual = await _projectService.GetPaginatedDataAsync(new BaseQueryParameters { ItemsPerPage = itemsPerPage, Page = page, Search = search });

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }

        [Theory]
        [InlineData(1, 1, "")]
        public async Task GetPaginatedDataAsync_Should_Be_Recieved_Once(int itemsPerPage, int page, string search)
        {
            await _projectService.GetPaginatedDataAsync(new BaseQueryParameters { ItemsPerPage = itemsPerPage, Page = page, Search = search });

            await _projectRepository.Received(1).GetFilteredDataCountAsync(Arg.Any<string>());
            await _projectRepository.Received(1).PaginateFiltered(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>());
        }

        #region Helpers
        public ProjectDto ProjectDto => new ProjectDto { Description = "Descr", Name = "Person1", Owner = null, OwnerId = 1, Id = 1 };
        public Project ProjectModel => new Project { Description = "Descr", ProjectName = "Person1", Owner = null, OwnerId = 1, ProjectId = 1 };
        public List<ProjectDto> ProjectDtoList => new List<ProjectDto> { new ProjectDto { Description = "Descr", Name = "Person1", Owner = null, OwnerId = 1, Id = 1 } };
        public List<Project> ProjectModelList => new List<Project> { new Project { Description = "Descr", ProjectName = "Person1", Owner = null, OwnerId = 1, ProjectId = 1 } };
        #endregion
    }
}
