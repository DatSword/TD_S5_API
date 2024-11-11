using TD1.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1.Models.DTO;
using AutoMapper;

namespace TD1.Models.DataManager
{
    public class ProduitManager : IDataRepository<Produit, ProduitDto, ProduitDetailDto>
    {
        private readonly ProduitsDBContext produitsDBContext;
        private readonly IMapper _mapper;

        public ProduitManager() { }

        public ProduitManager(ProduitsDBContext context, IMapper mapper)
        {
            produitsDBContext = context;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<ProduitDto>>> GetAllAsync()
        {
            var produits = await produitsDBContext.Produits
                              .Include(p => p.IdtypeProduitNavigation) // Inclus les relations pour le mapping
                              .Include(p => p.IdMarqueNavigation)
                              .ToListAsync();

            return _mapper.Map<List<ProduitDto>>(produits);
        }

        public async Task<ActionResult<Produit>> GetEntityByIdAsync(int id)
        {
            return await produitsDBContext.Produits.FirstOrDefaultAsync(p => p.IdProduit == id);
        }

        public async Task<ActionResult<ProduitDetailDto>> GetEntityDtoByIdAsync(int id)
        {
            var produit = await produitsDBContext.Produits
                              .Include(p => p.IdtypeProduitNavigation)
                              .Include(p => p.IdMarqueNavigation)
                              .FirstOrDefaultAsync(p => p.IdProduit == id);

            if (produit == null)
                return new NotFoundResult();

            return _mapper.Map<ProduitDetailDto>(produit);
        }

        public async Task AddAsync(Produit entity)
        {
            await produitsDBContext.Produits.AddAsync(entity);
            await produitsDBContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Produit produit, Produit entity)
        {
            produitsDBContext.Entry(produit).State = EntityState.Modified;
            produit.IdMarque = entity.IdMarque;
            produit.NomProduit = entity.NomProduit;
            produit.Description = entity.Description;
            produit.NomPhoto = entity.NomPhoto;
            produit.UriPhoto = entity.UriPhoto;
            produit.IdTypeProduit = entity.IdTypeProduit;
            produit.IdMarque = entity.IdMarque;
            produit.StockReel = entity.StockReel;
            produit.StockMin = entity.StockMin;
            produit.StockMax = entity.StockMax;

            await produitsDBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Produit produit)
        {
            produitsDBContext.Produits.Remove(produit);
            await produitsDBContext.SaveChangesAsync();
        }
    }
}
