using TD1.Models;
using TD1.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1.Models.DTO;

namespace TD1.Models.DataManager
{
    public class TypeProduitManager : IDataRepository<TypeProduit, TypeProduitDto, TypeProduitDetailDto>
    {
        readonly ProduitsDBContext produitsDBContext;

        public TypeProduitManager() { }

        public TypeProduitManager(ProduitsDBContext context)
        {
            produitsDBContext = context;
        }

        public async Task<ActionResult<IEnumerable<TypeProduitDto>>> GetAllAsync()
        {
            var typeProduitDtos = await produitsDBContext.TypeProduits
                .Select(t => new TypeProduitDto
                {
                    IdTypeProduit = t.IdTypeProduit,
                    NomTypeProduit = t.NomTypeProduit,
                    NbProduits = t.Produits.Count() // Comptage des produits associés
                })
                .ToListAsync();

            return new ActionResult<IEnumerable<TypeProduitDto>>(typeProduitDtos);
        }

        public async Task<ActionResult<TypeProduit>> GetEntityByIdAsync(int id)
        {
            return await produitsDBContext.TypeProduits.FirstOrDefaultAsync(p => p.IdTypeProduit == id);
        }

        public async Task<ActionResult<TypeProduitDetailDto>> GetEntityDtoByIdAsync(int id)
        {
            var typeProduit = await produitsDBContext.Marques
                .Include(m => m.Produits)
                .FirstOrDefaultAsync(m => m.IdMarque == id);

            if (typeProduit == null) return new NotFoundResult();

            var typeProduitDetailDto = new TypeProduitDetailDto
            {
                IdTypeProduit = typeProduit.IdMarque,
                NomTypeProduit = typeProduit.NomMarque,
                NbProduits = typeProduit.Produits.Count
            };

            return new ActionResult<TypeProduitDetailDto>(typeProduitDetailDto);
        }

        public async Task AddAsync(TypeProduit entity)
        {
            await produitsDBContext.TypeProduits.AddAsync(entity);
            await produitsDBContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TypeProduit typeProduit, TypeProduit entity)
        {
            produitsDBContext.Entry(typeProduit).State = EntityState.Modified;
            typeProduit.IdTypeProduit = entity.IdTypeProduit;
            typeProduit.NomTypeProduit = entity.NomTypeProduit;

            await produitsDBContext.SaveChangesAsync();
        }


        public async Task DeleteAsync(TypeProduit typeProduit)
        {
            produitsDBContext.TypeProduits.Remove(typeProduit);
            await produitsDBContext.SaveChangesAsync();
        }
    }
}
