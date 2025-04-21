using AutoMapper;
using LS.Application.DTOs;
using LS.Domain.Entities;

namespace LS.Application.Mappings
{
    public class ApplicationAutoMapperProfile : Profile
    {
        public ApplicationAutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
