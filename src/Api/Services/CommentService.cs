using AutoMapper;
using Data;
using Data.Models;
using Models.DTOs.Comment;
using Services.Interfaces;
using Services.Mapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class CommentService: ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Create(CreateCommentDto commentDto)
        {
            var comment = _mapper.Map<CreateCommentDto, Comment>(commentDto); 
            await _unitOfWork.CommentRepository.Create(comment);
        }

        public async Task<IEnumerable<CommentDto>> GetWorkItemsComments(int workItemId)
        {
            var comments = await Task.Run(() => _unitOfWork.CommentRepository.GetCommentsByWorkItemId(workItemId));
            var commentsDto = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDto>>(comments);
            return commentsDto;
        }
    }
}
