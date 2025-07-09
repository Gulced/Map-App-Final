using AutoMapper;
using MapApp.Application.Dtos;
using MapApp.Domain.Entities;

namespace MapApp.Application.Mappings
{
    public class AreaMappingProfile : Profile
    {
        public AreaMappingProfile()
        {
            CreateMap<Area, AreaDto>();
            CreateMap<AreaDto, Area>();
        }
    }
}