using AutoMapper;
using GloBus.Data.DTOs;
using GloBus.Data.Models;

namespace GloBus_backend.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();
        }
    }
}
