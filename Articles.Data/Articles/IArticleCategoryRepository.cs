using System.Collections.Generic;
using System.Threading.Tasks;

namespace Articles.Data.Articles
{
    public interface IArticleCategoryRepository
    {
        Task<List<ArticleCategory>> GetArticleCategoriesAsync();

        Task<ArticleCategory> GetArticleCategoryAsync(int articleCategoryId);

        Task<ArticleCategory> CreateArticleCategoryAsync(ArticleCategory articleCategory);

        Task<ArticleCategory> UpdateArticleCategoryAsync(ArticleCategory articleCategory);

        Task DeleteArticleCategoryAsync(int articleCategoryId);
    }
}