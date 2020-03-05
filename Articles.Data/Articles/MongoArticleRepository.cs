using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Articles.Services;
using MongoDB.Driver;

namespace Articles.Data.Articles
{
    public class MongoArticleRepository : IArticleRepository
    {
        private readonly MongoDbContext context;

        public MongoArticleRepository(MongoDbContext context)
        {
            this.context = context;

            if (context.Articles.CountDocuments(_ => true) == 0)
            {
                var article = new Article
                {
                    Id = 1,
                    UserId = 1,
                    CategoryId = 1,
                    Title = "Default Article",
                    Content = "Default content",
                    CreatedDate = DateTime.Now
                };

                context.Articles.InsertOne(article);
            }
        }

        public async Task<List<Article>> GetArticlesAsync()
        {
            return await context.Articles.Find(_ => true).ToListAsync();
        }

        public async Task<Article> GetArticleAsync(int articleId)
        {
            return await context.Articles.Find(a => a.Id == articleId).FirstOrDefaultAsync();
        }

        public async Task<Article> CreateArticlesAsync(Article article)
        {
            await CheckThatArticleUserIdExists(article.UserId);

            await CheckThatArticleCategoryIdExists(article.CategoryId);

            article.Id = await GenerateArticleId();

            await context.Articles.InsertOneAsync(article);

            return article;
        }

        public async Task<Article> UpdateArticleAsync(Article article)
        {
            await CheckThatArticleUserIdExists(article.UserId);

            await CheckThatArticleCategoryIdExists(article.CategoryId);

            await context.Articles.ReplaceOneAsync(a => a.Id == article.Id, article);

            return article;
        }

        private async Task<int> GenerateArticleId()
        {
            var articles = await GetArticlesAsync();

            return articles.Select(a => a.Id).Max() + 1;
        }

        public async Task DeleteArticleAsync(int articleId)
        {
            await context.Articles.DeleteOneAsync(a => a.Id == articleId);
        }

        private async Task CheckThatArticleUserIdExists(int userId)
        {
            var user = await context.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ResourceNotFoundException("User with given id does not exists.");
            }
        }

        private async Task CheckThatArticleCategoryIdExists(int categoryId)
        {
            var category = await context.Categories.Find(c => c.Id == categoryId).FirstOrDefaultAsync();

            if (category == null)
            {
                throw new ResourceNotFoundException("Article category with given id does not exists.");
            }
        }
    }
}