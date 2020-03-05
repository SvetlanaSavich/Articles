using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Articles.Data.Articles
{
    public class MongoArticleCategoryRepository : IArticleCategoryRepository
    {
        private readonly MongoDbContext context;

        public MongoArticleCategoryRepository(MongoDbContext context)
        {
            this.context = context;

            if (context.Categories.CountDocuments(_ => true) == 0)
            {
                var category = new ArticleCategory()
                {
                   Id = 1,
                   CategoryName = "Default Category"
                };

                context.Categories.InsertOne(category);
            }
        }

        public async Task<List<ArticleCategory>> GetArticleCategoriesAsync()
        {
            return await context.Categories.Find(_ => true).ToListAsync();
        }

        public async Task<ArticleCategory> GetArticleCategoryAsync(int articleCategoryId)
        {
            return await context.Categories.Find(c => c.Id== articleCategoryId).FirstOrDefaultAsync();
        }

        public async Task<ArticleCategory> CreateArticleCategoryAsync(ArticleCategory articleCategory)
        {
            var articleCategories = await GetArticleCategoriesAsync();

            var newArticleCategoryId = articleCategories.Select(c => c.Id).Max() + 1;

            articleCategory.Id = newArticleCategoryId;

            await context.Categories.InsertOneAsync(articleCategory);

            return articleCategory;
        }

        public async Task<ArticleCategory> UpdateArticleCategoryAsync(ArticleCategory articleCategory)
        {
            await context.Categories.ReplaceOneAsync(c => c.Id == articleCategory.Id, articleCategory);

            return articleCategory;
        }

        public async Task DeleteArticleCategoryAsync(int articleCategoryId)
        {
            await context.Categories.DeleteOneAsync(c => c.Id == articleCategoryId);
        }
    }
}