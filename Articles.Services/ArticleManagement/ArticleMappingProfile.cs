using System;
using Articles.Data.Articles;
using AutoMapper;

namespace Articles.Services.ArticleManagement
{
    public class ArticleMappingProfile : Profile
    {
        public ArticleMappingProfile()
        {
            CreateMap<Article, ArticleDTO>();

            CreateMap<ArticleDTO, Article>();

            CreateMap<Article, UpdateArticleRequest>();

            CreateMap<UpdateArticleRequest, Article>()
                .ForMember(a => a.CreatedDate, opt => opt.MapFrom(p => DateTime.UtcNow));

            CreateMap<ArticleCategory, ArticleCategoryDTO>();

            CreateMap<ArticleCategoryDTO, ArticleCategory>();

            CreateMap<ArticleCategory, UpdateArticleCategoryRequest>();

            CreateMap<UpdateArticleCategoryRequest, ArticleCategory>();
        }
    }
}