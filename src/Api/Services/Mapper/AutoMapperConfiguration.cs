using AutoMapper;
using Data.Models;
using Models.DTOs;
using Newtonsoft.Json;

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
                cfg.CreateMap<CreateWorkItemDto, WorkItem>()
                    .ForMember(dst => dst.WorkItemId, src => src.MapFrom(e => e.Id))
                    .ForMember(dst => dst.Assignee, opt => opt.Ignore())
                    .ForMember(dst => dst.Author, opt => opt.Ignore())
                    .ForMember(dst => dst.Comments, opt => opt.Ignore())
                    .ForMember(dst => dst.Project, opt => opt.Ignore())
                    .ForMember(dst => dst.Status, opt => opt.Ignore())
                    .ForMember(dst => dst.WorkItemType, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<WorkItemHistoryDto, WorkItem>()
                    .ReverseMap();
                cfg.CreateMap<WorkItemHistoryDto, WorkItemDto>()
                    .ForMember(dst => dst.Id, opt => opt.Ignore())
                    .ForMember(dst => dst.Assignee, opt => opt.Ignore())
                    .ForMember(dst => dst.Author, opt => opt.Ignore())
                    .ForMember(dst => dst.Comments, opt => opt.Ignore())
                    .ForMember(dst => dst.Project, opt => opt.Ignore())
                    .ForMember(dst => dst.WorkItemType, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<CommentDto, Comment>()
                    .ForMember(dst => dst.CommentId, src => src.MapFrom(e => e.Id))
                    .ForMember(dst => dst.CommentBody, src => src.MapFrom(e => e.Body))
                    .ForMember(dst => dst.Author, opt => opt.Ignore())
                    .ForMember(dst => dst.WorkItem, opt => opt.Ignore())
                    .ReverseMap();
                cfg.CreateMap<WorkItemAuditDto, WorkItemAudit>()
                    .ForMember(dst => dst.WorkItemAuditId, src => src.MapFrom(e => e.Id))
                    .ForMember(dest => dest.OldWorkItem, opt =>
                    {
                        opt.MapFrom(src => (WorkItemHistoryDto)JsonConvert.DeserializeObject(src.OldWorkItem));
                    })
                    .ForMember(dest => dest.NewWorkItem, opt =>
                    {
                        opt.MapFrom(src => (WorkItemHistoryDto)JsonConvert.DeserializeObject(src.NewWorkItem));
                    })
                    .ReverseMap();
                cfg.CreateMap<FileDto, File>()
                    .ForMember(dst => dst.FileId, src => src.MapFrom(e => e.Id))
                    .ForMember(dst => dst.FileName, src => src.MapFrom(e => e.Name))
                    .ForMember(dst => dst.FilePath, src => src.MapFrom(e => e.Path))
                    .ReverseMap();
                cfg.CreateMap<FileDto, WorkItemFile> ()
                    .ForMember(dst => dst.FileId, src => src.MapFrom(e => e.Id))
                    .ForPath(dst => dst.File.FileName, src => src.MapFrom(e => e.Name))
                    .ForPath(dst => dst.File.FilePath, src => src.MapFrom(e => e.Path))
                    .ReverseMap();
            });

            return config;
        }
    }
}
