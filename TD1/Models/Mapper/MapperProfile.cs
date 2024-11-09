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

            CreateMap<Produit, ProduitDto>()
                .ForMember(pDto => pDto.IdProduit, act => act.MapFrom(p => p.IdProduit))
                .ForMember(pDto => pDto.NomProduit, act => act.MapFrom(p => p.NomProduit))
                .ForMember(pDto => pDto.IdTypeProduit, act => act.MapFrom(p => p.IdTypeProduit))
                .ForMember(pDto => pDto.IdMarque, act => act.MapFrom(p => p.IdMarque))
                .ReverseMap();
        }
    }
}
