using AutoMapper;
using MapApp.Domain.Entities;
using MapApp.Infrastructure.Entities;
using MapApp.Application.Dtos;

namespace MapApp.Infrastructure.Mappings
{
    public class AppUserMappingProfile : Profile
    {
        public AppUserMappingProfile()
        {
            // AppUser <-> User mapping
            CreateMap<AppUser, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));

            CreateMap<User, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // AppUser -> UserDto mapping
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}