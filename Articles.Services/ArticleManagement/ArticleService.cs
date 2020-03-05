using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Articles.Data.Articles;
using AutoMapper;

namespace Articles.Services.ArticleManagement
{
    public class ArticleService
    {
        private readonly IArticleRepository articleRepository;

        private readonly IMapper mapper;

        public ArticleService(IArticleRepository articleRepository, IMapper mapper)
        {
            this.articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));

            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ArticleDTO>> GetArticlesAsync()
        {
            var dbArticles = await articleRepository.GetArticlesAsync();

            var articles = dbArticles.Select(a => mapper.Map<ArticleDTO>(a)).ToList();

            return articles;
        }

        public async Task<ArticleDTO> GetArticleAsync(int articleId)
        {
            var dbArticle = await articleRepository.GetArticleAsync(articleId);

            return mapper.Map<ArticleDTO>(dbArticle);
        }

        public async Task<List<ArticleDTO>> GetArticleByUserId(int userId)
        {
            var articles = await GetArticlesAsync();

            return  articles.Where(a => a.UserId == userId).ToList();
        }

        public async Task<ArticleDTO> CreateArticleAsync(UpdateArticleRequest createdArticle)
        {
            var existArticle = articleRepository.GetArticlesAsync().Result.FirstOrDefault(a => a.Title == createdArticle.Title);

            if (existArticle != null)
            {
                throw new ResourceHasConflictException($"Article with title {createdArticle.Title} exists.");
            }

            var dbArticle = mapper.Map<Article>(createdArticle);

            await articleRepository.CreateArticlesAsync(dbArticle);

            return mapper.Map<ArticleDTO>(dbArticle);
        }

        public async Task<ArticleDTO> UpdateArticleAsync(int articleId, UpdateArticleRequest updatedArticle)
        {
            var dbArticle = await articleRepository.GetArticleAsync(articleId);

            if (dbArticle == null)
            {
                return null;
            }

            mapper.Map(updatedArticle, dbArticle);

            await articleRepository.UpdateArticleAsync(dbArticle);

            return mapper.Map<ArticleDTO>(dbArticle);
        }

        public async Task DeleteArticleAsync(int articleId)
        {
            await articleRepository.DeleteArticleAsync(articleId);
        }
    }
}