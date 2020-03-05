using Articles.Data.Users;
using AutoMapper;

namespace Articles.Services.UserManagement
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDTO>();

            CreateMap<UserDTO, User>();

            CreateMap<User,UpdateUserRequest>();

            CreateMap<UpdateUserRequest, User>();
        }
    }
}