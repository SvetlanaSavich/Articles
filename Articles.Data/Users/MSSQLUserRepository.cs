using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Articles.Data.Users
{
    /// <summary>
    /// Represents MSSQLUserRepository
    /// </summary>
    /// <seealso cref="Articles.Data.Users.IUserRepository" />
    public class MSSQLUserRepository : IUserRepository
    {
        private readonly ArticlesDbContext context;

        public MSSQLUserRepository(ArticlesDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the users asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        /// <summary>
        /// Gets the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A <see cref="User"/>.</returns>
        public async Task<User> GetUserAsync(int userId)
        {
            return await context.Users.FindAsync(userId);
        }

        /// <summary>
        /// Creates the user asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A <see cref="Task{User}"/>.</returns>
        public async Task<User> CreateUserAsync(User user)
        {
            context.Users.Add(user);

            await context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Updates the user asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A <see cref="Task{User}"/></returns>
        public async Task<User> UpdateUserAsync(User user)
        {
            context.Entry(user).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Deletes the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public async Task DeleteUserAsync(int userId)
        {
            var user = await context.Users.FindAsync(userId);

            context.Users.Remove(user);

            await context.SaveChangesAsync();
        }
    }
}