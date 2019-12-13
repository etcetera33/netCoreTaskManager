using AutoMapper;
using Data;
using Data.Models;
using Models.DTOs;
using Services.Interfaces;
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

        public async Task<CommentDto> Create(CommentDto commentDto)
        {
            commentDto.SentAt = DateTime.Now;

            var comment = _mapper.Map<CommentDto, Comment>(commentDto);
            var createdEntity = await _unitOfWork.CommentRepository.Create(comment);

            return _mapper.Map<Comment, CommentDto>(createdEntity);
        }

        public async Task<IEnumerable<CommentDto>> GetWorkItemsComments(int workItemId)
        {
            var comments = await _unitOfWork.CommentRepository.GetCommentsByWorkItemIdAsync(workItemId);
            var commentsDto = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDto>>(comments);

            return commentsDto;
        }

        public async Task Remove(int commentId)
        {
            await _unitOfWork.CommentRepository.Delete(commentId);
        }

        public async Task<bool> CommentExists(int commentId)
        {
            return (await _unitOfWork.CommentRepository.GetById(commentId) != null);
        }
    }
}
