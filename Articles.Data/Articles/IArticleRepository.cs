using System.Collections.Generic;
using System.Threading.Tasks;

namespace Articles.Data.Articles
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetArticlesAsync();

        Task<Article> GetArticleAsync(int articleId);

        Task<Article> CreateArticlesAsync(Article article);

        Task<Article> UpdateArticleAsync(Article article);

        Task DeleteArticleAsync(int articleId);
    }
}