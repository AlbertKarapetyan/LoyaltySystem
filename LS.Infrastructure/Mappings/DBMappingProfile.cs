using AutoMapper;
using LS.Domain.Entities;
using LS.Infrastructure.Data.Models;

namespace LS.Infrastructure.Mappings
{
    public class DBMappingProfile : Profile
    {
        public DBMappingProfile()
        {
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<UserPointModel, UserPoint>().ReverseMap();
        }
    }
}
