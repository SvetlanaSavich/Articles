using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Articles.Data.Articles
{
    public class MSSQLArticleCategoryRepository : IArticleCategoryRepository
    {
        private readonly ArticlesDbContext context;

        public MSSQLArticleCategoryRepository(ArticlesDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<ArticleCategory>> GetArticleCategoriesAsync()
        {
            return await this.context.ArticleCategories.ToListAsync();
        }

        public async Task<ArticleCategory> GetArticleCategoryAsync(int articleCategoryId)
        {
            return await context.ArticleCategories.FindAsync(articleCategoryId);
        }

        public async Task<ArticleCategory> CreateArticleCategoryAsync(ArticleCategory articleCategory)
        {
            context.ArticleCategories.Add(articleCategory);

            await context.SaveChangesAsync();

            return articleCategory;
        }

        public async Task<ArticleCategory> UpdateArticleCategoryAsync(ArticleCategory articleCategory)
        {
            context.Entry(articleCategory).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return articleCategory;
        }

        public async Task DeleteArticleCategoryAsync(int articleCategoryId)
        {
            var articleCategory = await context.ArticleCategories.FindAsync(articleCategoryId);

            context.ArticleCategories.Remove(articleCategory);

            await context.SaveChangesAsync();
        }
    }
}