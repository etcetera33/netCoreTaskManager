using AutoMapper;
using Data.Interfaces;
using Data.Models;
using Models.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IRedisService redisService)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _redisService = redisService;
        }

        public async Task<CommentDto> Create(CommentDto commentDto)
        {
            commentDto.SentAt = DateTime.Now;

            var comment = _mapper.Map<CommentDto, Comment>(commentDto);
            var createdEntity = await _commentRepository.Create(comment);

            return _mapper.Map<Comment, CommentDto>(createdEntity);
        }

        public async Task<IEnumerable<CommentDto>> GetWorkItemsComments(int workItemId)
        {
            var comments = await _commentRepository.GetCommentsByWorkItemIdAsync(workItemId);
            var commentsDto = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDto>>(comments);

            return commentsDto;
        }

        public async Task Remove(int commentId)
        {
            await _commentRepository.Delete(commentId);
        }

        public async Task<bool> CommentExists(int commentId)
        {
            return (await _commentRepository.GetById(commentId) != null);
        }

        public async Task<int> GetCommentAuthorId(int commentId)
        {
            return (await _commentRepository.GetById(commentId)).AuthorId;
        }
    }
}
