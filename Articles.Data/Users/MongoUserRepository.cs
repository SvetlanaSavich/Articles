using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Articles.Data.Users
{
    public class MongoUserRepository:IUserRepository
    {
        private readonly MongoDbContext context;

        public MongoUserRepository(MongoDbContext context)
        {
            this.context = context;

            if (context.Users.CountDocuments(_ => true) == 0)
            {
                var user = new User 
                {
                    Id = 1, 
                    UserName = "User", 
                    Password = "123456", 
                    Email = "email"

                };

                context.Users.InsertOne(user);
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await context.Users.Find(_ => true).ToListAsync();
        }

        public async Task<User> GetUserAsync(int userId)
        {
            return await context.Users.Find(a => a.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var users = await GetUsersAsync();

            var newUserId = users.Select(a => a.Id).Max() + 1;

            user.Id = newUserId;

            await context.Users.InsertOneAsync(user);

            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            await context.Users.ReplaceOneAsync(u => u.Id == user.Id, user);

            return user;
        }

        public async Task DeleteUserAsync(int userId)
        {
            await context.Users.DeleteOneAsync(u => u.Id == userId);
        }
    }
}