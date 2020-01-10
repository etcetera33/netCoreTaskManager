using AutoMapper;
using Data.Interfaces;
using Data.Models;
using MassTransit;
using Models.DTOs;
using NSubstitute;
using Services.Mapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq.Expressions;

namespace Services.Tests
{
    public class WorkItemServiceTests
    {
        private readonly WorkItemService _workItemService;
        private readonly IWorkItemRepository _workItemRepository;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public WorkItemServiceTests()
        {
            _mapper = AutoMapperConfiguration.Configure().CreateMapper();
            _bus = Substitute.For<IBus>();

            //_workItemRepository.PaginateFiltered(Arg.Any<Expression<Func<WorkItem, bool>>>(), Arg.Any<int>(), Arg.Any<int>()).Returns();
            //_workItemRepository.Pag
            //_workItemRepository = Substitute.For<IProjectRepository>();
            //_workItemRepository.GetAll().Returns(ProjectModelList);
            //_workItemRepository.Create(Arg.Any<Project>()).Returns(ProjectModel);
            //_workItemRepository.GetById(Arg.Any<int>()).Returns(ProjectModel);
            //_workItemRepository.PaginateFiltered(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>()).Returns(ProjectModelList);
            //_workItemRepository.GetFilteredDataCountAsync(Arg.Any<string>()).Returns(1);

            _workItemService = Substitute.For<WorkItemService>(_workItemRepository, _mapper);
        }

        /*[Theory]
        [InlineData(1, 10, 1, "", null)]*/
        public async Task Paginate_Should_Return_Proper_Value(int projectId, int itemsPerPage, int page, string search, int? assigneeId)
        {

        }

        #region Helpers
        private WorkItemDto WorkItemDto => new WorkItemDto { Id = 1, Title = "Project 1", Description = "Descr of Project 1", ProjectId = 1, Priority = 10, AssigneeId = 1, Progress = 10, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null };
        private WorkItem WorkItemModel => new WorkItem { WorkItemId = 1, Title = "Title", Description = "Descr of the work item", ProjectId = 1, Project = null, AssigneeId = 1, Assignee = null, AuthorId = 1, Author = null, Comments = null, Priority = 1, Progress = 80, StatusId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature };
        private List<WorkItemDto> WorkItemDtoList => new List<WorkItemDto>
        {
            new WorkItemDto { Id = 1, Title = "Project 1", Description = "Descr of Project 1", ProjectId = 1, Priority = 10, AssigneeId = 1, Progress = 10, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
            new WorkItemDto { Id = 2, Title = "Project 2", Description = "Descr of Project 2", ProjectId = 2, Priority = 20, AssigneeId = 1, Progress = 20, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
            new WorkItemDto { Id = 3, Title = "Project 3", Description = "Descr of Project 3", ProjectId = 3, Priority = 30, AssigneeId = 2, Progress = 30, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
            new WorkItemDto { Id = 4, Title = "Project 4", Description = "Descr of Project 4", ProjectId = 4, Priority = 40, AssigneeId = 2, Progress = 40, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
            new WorkItemDto { Id = 5, Title = "Project 5", Description = "Descr of Project 5", ProjectId = 5, Priority = 50, AssigneeId = 3, Progress = 50, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
            new WorkItemDto { Id = 6, Title = "Project 6", Description = "Descr of Project 6", ProjectId = 6, Priority = 60, AssigneeId = 4, Progress = 60, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
        };
        private List<WorkItem> WorkItemModelList => new List<WorkItem>
        {
            new WorkItem { WorkItemId = 1, Title = "Project 1", Description = "Descr of Project 1", ProjectId = 1, Priority = 10, AssigneeId = 1, Progress = 10, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
            new WorkItem { WorkItemId = 2, Title = "Project 2", Description = "Descr of Project 2", ProjectId = 2, Priority = 20, AssigneeId = 1, Progress = 20, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
            new WorkItem { WorkItemId = 3, Title = "Project 3", Description = "Descr of Project 3", ProjectId = 3, Priority = 30, AssigneeId = 2, Progress = 30, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
            new WorkItem { WorkItemId = 4, Title = "Project 4", Description = "Descr of Project 4", ProjectId = 4, Priority = 40, AssigneeId = 2, Progress = 40, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
            new WorkItem { WorkItemId = 5, Title = "Project 5", Description = "Descr of Project 5", ProjectId = 5, Priority = 50, AssigneeId = 3, Progress = 50, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
            new WorkItem { WorkItemId = 6, Title = "Project 6", Description = "Descr of Project 6", ProjectId = 6, Priority = 60, AssigneeId = 4, Progress = 60, StatusId = 1, AuthorId = 1, WorkItemTypeId = (int)Core.Enums.WorkItemTypes.Feature, Project = null, Assignee = null, Author = null, Comments = null},
        };
        #endregion
    }
}
