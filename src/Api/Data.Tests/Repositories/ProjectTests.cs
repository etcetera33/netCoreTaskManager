using Data.Interfaces;
using Data.Models;
using Data.Repositories;
using NSubstitute;
using System.Collections.Generic;
using System.Data.Entity;
using Xunit;

namespace Data.Tests.Repositories
{
    public class ProjectTests
    {
        private IEnumerable<Project> _projects = new List<Project>
        {
            new Project{ ProjectId = 1, ProjectName = "Project1", Description = "Descr Project1", Owner = null, OwnerId = 1, WorkItems = null},
            new Project{ ProjectId = 2, ProjectName = "Project2", Description = "Descr Project2", Owner = null, OwnerId = 1, WorkItems = null},
            new Project{ ProjectId = 3, ProjectName = "Project3", Description = "Descr Project3", Owner = null, OwnerId = 1, WorkItems = null},
            new Project{ ProjectId = 4, ProjectName = "Project4", Description = "Descr Project4", Owner = null, OwnerId = 1, WorkItems = null}
        };

        private readonly IBaseRepository<Project> _baseRepo;
        private readonly ProjectRepository _repo;

        public ProjectTests()
        {
            var db = Substitute.For<ApplicationDbContext>();
            db.Set<Project>();

            var _baseRepo = Substitute.For<IBaseRepository<Project>>();
            var _repo = Substitute.For<ProjectRepository>();
        }

        [Fact]
        public void Should_Return_Success()
        {
            _repo.GetAll();
        }
    }
}
