using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Articles.Data.Articles;
using AutoMapper;

namespace Articles.Services.ArticleManagement
{
    public class ArticleCategoryService
    {
        private readonly IArticleCategoryRepository articleCategoryRepository;

        private readonly IMapper mapper;

        public ArticleCategoryService(IArticleCategoryRepository articleCategoryRepository, IMapper mapper)
        {
            this.articleCategoryRepository = articleCategoryRepository ?? throw new ArgumentNullException(nameof(articleCategoryRepository));

            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ArticleCategoryDTO>> GetArticleCategoriesAsync()
        {
            var dbArticleCategories = await articleCategoryRepository.GetArticleCategoriesAsync();

            var articleCategories = dbArticleCategories.Select(c => mapper.Map<ArticleCategoryDTO>(c)).ToList();

            return articleCategories;
        }

        public async Task<ArticleCategoryDTO> GetArticleCategoryAsync(int articleCategoryId)
        {
            var dbArticleCategory = await articleCategoryRepository.GetArticleCategoryAsync(articleCategoryId);

            return mapper.Map<ArticleCategoryDTO>(dbArticleCategory);
        }

        public async Task<ArticleCategoryDTO> CreateArticleCategoryAsync(UpdateArticleCategoryRequest createdArticleCategory)
        {
            var dbArticleCategory = mapper.Map<ArticleCategory>(createdArticleCategory);

            await articleCategoryRepository.CreateArticleCategoryAsync(dbArticleCategory);

            return mapper.Map<ArticleCategoryDTO>(dbArticleCategory);
        }

        public async Task<ArticleCategoryDTO> UpdateArticleCategoryAsync(int articleCategoryId, UpdateArticleCategoryRequest updatedArticleCategory)
        {
            var dbArticleCategory = await articleCategoryRepository.GetArticleCategoryAsync(articleCategoryId);

            if (dbArticleCategory == null)
            {
                return null;
            }

            mapper.Map(updatedArticleCategory, dbArticleCategory);

            await articleCategoryRepository.UpdateArticleCategoryAsync(dbArticleCategory);

            return mapper.Map<ArticleCategoryDTO>(dbArticleCategory);
        }

        public async Task DeleteArticleCategoryAsync(int articleCategoryId)
        {
            await articleCategoryRepository.DeleteArticleCategoryAsync(articleCategoryId);
        }
    }
}