using TD1.Models;
using TD1.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1.Models.DTO;

namespace TD1.Models.DataManager
{
    public class MarqueManager : IDataRepository<Marque, MarqueDto, MarqueDetailDto>
    {
        readonly ProduitsDBContext produitsDBContext;

        public MarqueManager() { }

        public MarqueManager(ProduitsDBContext context)
        {
            produitsDBContext = context;
        }

        public async Task<ActionResult<IEnumerable<MarqueDto>>> GetAllAsync()
        {
            var marqueDtos = await produitsDBContext.Marques
                .Select(m => new MarqueDto
                {
                    IdMarque = m.IdMarque,
                    NomMarque = m.NomMarque,
                    NbProduits = m.Produits.Count() // Comptage des produits associés
                })
                .ToListAsync();

            return new ActionResult<IEnumerable<MarqueDto>>(marqueDtos);
        }

        public async Task<ActionResult<Marque>> GetEntityByIdAsync(int id)
        {
            return await produitsDBContext.Marques.FirstOrDefaultAsync(p => p.IdMarque == id);
        }
        public async Task<ActionResult<MarqueDetailDto>> GetEntityDtoByIdAsync(int id)
        {
            var marque = await produitsDBContext.Marques
                .Include(m => m.Produits)
                .FirstOrDefaultAsync(m => m.IdMarque == id);

            if (marque == null) return new NotFoundResult();

            var marqueDetailDto = new MarqueDetailDto
            {
                IdMarque = marque.IdMarque,
                NomMarque = marque.NomMarque,
                NbProduits = marque.Produits.Count
            };

            return new ActionResult<MarqueDetailDto>(marqueDetailDto);
        }

        public async Task AddAsync(Marque entity)
        {
            await produitsDBContext.Marques.AddAsync(entity);
            await produitsDBContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Marque marque, Marque entity)
        {
            produitsDBContext.Entry(marque).State = EntityState.Modified;
            marque.IdMarque = entity.IdMarque;
            marque.NomMarque = entity.NomMarque;

            await produitsDBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Marque marque)
        {
            produitsDBContext.Marques.Remove(marque);
            await produitsDBContext.SaveChangesAsync();
        }
    }
}
