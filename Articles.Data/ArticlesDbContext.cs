using Articles.Data.Articles;
using Articles.Data.Comments;
using Articles.Data.Users;
using Microsoft.EntityFrameworkCore;

namespace Articles.Data
{
    public class ArticlesDbContext : DbContext
    {
        public ArticlesDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleCategory> ArticleCategories{ get; set; }

        public DbSet<Comment> Comments { get; set; }
    }
}