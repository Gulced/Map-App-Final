using AutoMapper;
using MapApp.Application.Dtos;
using MapApp.Domain.Entities;

namespace MapApp.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
