using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Articles.Data.Users;
using AutoMapper;

namespace Articles.Services.UserManagement
{
    public class UserService
    {
        private readonly IUserRepository userRepository;

        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var dbUsers =  await userRepository.GetUsersAsync();

            var users = dbUsers.Select(u => mapper.Map<UserDTO>(u)).ToList();

            return users;
        }

        public async Task<UserDTO> GetUserAsync(int userId)
        {
            var dbUser = await userRepository.GetUserAsync(userId);

            return mapper.Map<UserDTO>(dbUser);
        }

        public async Task<UserDTO> CreateUserAsync(UpdateUserRequest createdUser)
        {
            CheckThatUserWithSameEmailOrNameExists(createdUser);

            var dbUser = mapper.Map<User>(createdUser);

            await userRepository.CreateUserAsync(dbUser);

            return mapper.Map<UserDTO>(dbUser);
        }

        public async Task<UserDTO> UpdateUserAsync(int userId, UpdateUserRequest updatedUser)
        {
            CheckThatUserWithSameEmailOrNameExists(updatedUser);

            var dbUser = await userRepository.GetUserAsync(userId);

            if (dbUser == null)
            {
                return null;
            }

            mapper.Map(updatedUser, dbUser);

            await userRepository.UpdateUserAsync(dbUser);

            return mapper.Map<UserDTO>(dbUser);
        }

        public async Task DeleteUserAsync(int userId)
        {
            await userRepository.DeleteUserAsync(userId);
        }

        private void CheckThatUserWithSameEmailOrNameExists(UpdateUserRequest createdUser)
        {
            var conflictUser = userRepository.GetUsersAsync().Result.FirstOrDefault(u => u.UserName == createdUser.UserName);

            if (conflictUser != null)
            {
                throw new ResourceHasConflictException($"User with same user name exists.");
            }

            conflictUser = userRepository.GetUsersAsync().Result.FirstOrDefault(u => u.Email == createdUser.Email);

            if (conflictUser != null)
            {
                throw new ResourceHasConflictException($"User with same email exists.");
            }
        }
    }
}