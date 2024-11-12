using AutoMapper;
using TD1.Models.DTO;

namespace TD1.Models.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Marque, MarqueDto>()
                .ForMember(dest => dest.IdMarque, opt => opt.MapFrom(src => src.IdMarque))
                .ForMember(dest => dest.NomMarque, opt => opt.MapFrom(src => src.NomMarque))
                .ForMember(dest => dest.NbProduits, opt => opt.MapFrom(src => src.Produits.Count))
                .ReverseMap();

            CreateMap<Produit, ProduitDto>()
                //.ForMember(dest => dest.IdProduit, opt => opt.MapFrom(src => src.IdProduit))
                //.ForMember(dest => dest.NomProduit, opt => opt.MapFrom(src => src.NomProduit))
                .ForMember(dest => dest.NomTypeProduit, opt => opt.MapFrom(src => src.IdtypeProduitNavigation.NomTypeProduit)) // Suppose un type lié
                .ForMember(dest => dest.NomMarque, opt => opt.MapFrom(src => src.IdMarqueNavigation.NomMarque)) // Suppose une marque liée
                .ReverseMap();

            CreateMap<Produit, ProduitDetailDto>()
                //.ForMember(dest => dest.IdProduit, opt => opt.MapFrom(src => src.IdProduit))
                //.ForMember(dest => dest.NomProduit, opt => opt.MapFrom(src => src.NomProduit))
                .ForMember(dest => dest.NomTypeProduit, opt => opt.MapFrom(src => src.IdtypeProduitNavigation.NomTypeProduit))
                .ForMember(dest => dest.NomMarque, opt => opt.MapFrom(src => src.IdMarqueNavigation.NomMarque))
                .ForMember(dest => dest.EnReappro, opt => opt.MapFrom(src => src.StockReel < src.StockMin))
                .ReverseMap();

            CreateMap<TypeProduit, TypeProduitDto>()
                .ForMember(dest => dest.IdTypeProduit, opt => opt.MapFrom(src => src.IdTypeProduit))
                .ForMember(dest => dest.NomTypeProduit, opt => opt.MapFrom(src => src.NomTypeProduit))
                .ForMember(dest => dest.NbProduits, opt => opt.MapFrom(src => src.Produits.Count))
                .ReverseMap();
        }
    }
}
