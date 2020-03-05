using System;
using Articles.Data.Comments;
using AutoMapper;

namespace Articles.Services.CommentManagement
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<Comment, CommentDTO>();

            CreateMap<CommentDTO, Comment>();

            CreateMap<UpdateCommentRequest, Comment>()
                .ForMember(c=>c.CreateDate, opt => opt.MapFrom(p => DateTime.UtcNow));

            CreateMap<Comment, UpdateCommentRequest>();
        }
    }
}