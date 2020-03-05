using Articles.Data.Articles;
using Articles.Data.Comments;
using Articles.Data.Users;
using MongoDB.Driver;

namespace Articles.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase database;

        public MongoDbContext(IMongoClient client, string dbName)
        {
            database = client.GetDatabase(dbName);
        }

        public IMongoCollection<Article> Articles => database.GetCollection<Article>("Articles");

        public IMongoCollection<User> Users => database.GetCollection<User>("Users");

        public IMongoCollection<Comment> Comments => database.GetCollection<Comment>("Comments");

        public IMongoCollection<ArticleCategory> Categories => database.GetCollection<ArticleCategory>("Categories");
    }
}