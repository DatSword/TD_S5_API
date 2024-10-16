using AutoMapper;
using TD1.Models.DTO;

namespace TD1.Models.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Marque, MarqueDto>()
                .ForMember(mDto => mDto.IdMarque, act => act.MapFrom(m => m.IdMarque))
                .ForMember(mDto => mDto.NomMarque, act => act.MapFrom(m => m.NomMarque))
                .ReverseMap();
        }
    }
}
