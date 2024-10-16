using AutoMapper;
using TD1.Models.DTO;

namespace TD1.Models.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Marque, MarqueDto>();

            CreateMap<MarqueDto, Marque>();
        }
    }
}
