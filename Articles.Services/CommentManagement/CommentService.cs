using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Articles.Data.Comments;
using AutoMapper;

namespace Articles.Services.CommentManagement
{
    public class CommentService
    {
        private readonly ICommentRepository commentRepository;

        private readonly IMapper mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            this.commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));

            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<CommentDTO>> GetCommentsAsync()
        {
            var dbComments = await commentRepository.GetCommentsAsync();

            var comments = dbComments.Select(c => mapper.Map<CommentDTO>(c)).ToList();

            return comments;
        }

        public async Task<CommentDTO> GetCommentAsync(int commentId)
        {
            var dbComment = await commentRepository.GetCommentAsync(commentId);

            return mapper.Map<CommentDTO>(dbComment);
        }

        public async Task<CommentDTO> CreateCommentAsync(UpdateCommentRequest createdComment)
        {
            var dbComment = mapper.Map<Comment>(createdComment);

            await commentRepository.CreateCommentAsync(dbComment);

            return mapper.Map<CommentDTO>(dbComment);
        }

        public async Task<CommentDTO> UpdateCommentAsync(int commentId, UpdateCommentRequest updatedComment)
        {
            var dbComment = await commentRepository.GetCommentAsync(commentId);

            if (dbComment == null)
            {
                return null;
            }

            mapper.Map(updatedComment, dbComment);

            await commentRepository.UpdateCommentAsync(dbComment);

            return mapper.Map<CommentDTO>(dbComment);
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            await commentRepository.DeleteCommentAsync(commentId);
        }
    }
}