﻿using AutoMapper;
using Data.Models;
using Models.DTOs;
using Models.DTOs.Comment;
using Models.DTOs.Project;
using Models.DTOs.ProjectRole;
using Models.DTOs.WorkItem;

namespace Services.Mapper
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<CreateUserDto, User>()
                    .ForMember(dst => dst.UserId, opt => opt.Ignore())
                    .ForMember(dst => dst.Comments, opt => opt.Ignore())
                    .ForMember(dst => dst.AssignedTo, opt => opt.Ignore())
                    .ForMember(dst => dst.CreatedWorkItems, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<UserDto, User>()
                    .ForMember(dst => dst.UserId, src => src.MapFrom<int>(e => e.Id))
                    .ForMember(dst => dst.Comments, opt => opt.Ignore())
                    .ForMember(dst => dst.AssignedTo, opt => opt.Ignore())
                    .ForMember(dst => dst.CreatedWorkItems, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<CreateProjectDto, Project>()
                    .ForMember(dst => dst.ProjectId, opt => opt.Ignore())
                    .ForMember(dst => dst.WorkItems, opt => opt.Ignore())
                    .ForMember(dst => dst.ProjectName, src => src.MapFrom<string>(e => e.Name))
                    .ReverseMap();
                cfg.CreateMap<ProjectDto, Project>()
                    .ForMember(dst => dst.WorkItems, opt => opt.Ignore())
                    .ForMember(dst => dst.ProjectId, src => src.MapFrom(e => e.Id))
                    .ForMember(dst => dst.ProjectName, src => src.MapFrom(e => e.Name))
                    .ReverseMap();
                cfg.CreateMap<CreateWorkItemDto, WorkItem>()
                    .ForMember(dst => dst.WorkItemId, opt => opt.Ignore())
                    .ForMember(dst => dst.Assignee, opt => opt.Ignore())
                    .ForMember(dst => dst.Author, opt => opt.Ignore())
                    .ForMember(dst => dst.Comments, opt => opt.Ignore())
                    .ForMember(dst => dst.Project, opt => opt.Ignore())
                    .ForMember(dst => dst.Status, opt => opt.Ignore())
                    .ForMember(dst => dst.WorkItemType, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<WorkItemDto, WorkItem>()
                    .ForMember(dst => dst.WorkItemId, src => src.MapFrom(e => e.Id))
                    .ForMember(dst => dst.Assignee, opt => opt.Ignore())
                    .ForMember(dst => dst.Author, opt => opt.Ignore())
                    .ForMember(dst => dst.Comments, opt => opt.Ignore())
                    .ForMember(dst => dst.Project, opt => opt.Ignore())
                    .ForMember(dst => dst.Status, opt => opt.Ignore())
                    .ForMember(dst => dst.WorkItemType, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<CreateCommentDto, Comment>()
                    .ForMember(dst => dst.CommentId, opt => opt.Ignore())
                    .ForMember(dst => dst.Author, opt => opt.Ignore())
                    .ForMember(dst => dst.WorkItem, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<CommentDto, Comment>()
                    .ForMember(dst => dst.CommentId, src => src.MapFrom(e => e.Id))
                    .ForMember(dst => dst.Author, opt => opt.Ignore())
                    .ForMember(dst => dst.WorkItem, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<CreateProjectRoleDto, ProjectRole>()
                    .ForMember(dst => dst.ProjectRoleId, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<ProjectRoleDto, ProjectRole>()
                    .ForMember(dst => dst.ProjectRoleId, src => src.MapFrom(p => p.Id))
                    .ReverseMap();
            });
            return config;
        }
    }
}
