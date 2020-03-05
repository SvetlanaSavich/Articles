using System.Collections.Generic;
using System.Threading.Tasks;

namespace Articles.Data.Users
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();

        Task<User> GetUserAsync(int userId);

        Task<User> CreateUserAsync(User user);

        Task<User> UpdateUserAsync(User user);

        Task DeleteUserAsync(int userId);
    }
}