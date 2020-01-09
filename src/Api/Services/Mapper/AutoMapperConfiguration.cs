using AutoMapper;
using Data.Models;
using Models.DTOs;

namespace Services.Mapper
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserDictionaryDto, User>()
                    .ForMember(dst => dst.UserId, src => src.MapFrom(e => e.Id))
                    .ReverseMap();
                cfg.CreateMap<UserDto, User>()
                    .ForMember(dst => dst.UserId, src => src.MapFrom(e => e.Id))
                    .ForMember(dst => dst.Comments, opt => opt.Ignore())
                    .ForMember(dst => dst.AssignedTo, opt => opt.Ignore())
                    .ForMember(dst => dst.CreatedWorkItems, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<ProjectDto, Project>()
                    .ForMember(dst => dst.WorkItems, opt => opt.Ignore())
                    .ForMember(dst => dst.ProjectId, src => src.MapFrom(e => e.Id))
                    .ForMember(dst => dst.ProjectName, src => src.MapFrom(e => e.Name))
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
                cfg.CreateMap<CommentDto, Comment>()
                    .ForMember(dst => dst.CommentId, src => src.MapFrom(e => e.Id))
                    .ForMember(dst => dst.CommentBody, src => src.MapFrom(e => e.Body))
                    .ForMember(dst => dst.Author, opt => opt.Ignore())
                    .ForMember(dst => dst.WorkItem, opt => opt.Ignore())
                    .ReverseMap();
            });

            return config;
        }
    }
}
