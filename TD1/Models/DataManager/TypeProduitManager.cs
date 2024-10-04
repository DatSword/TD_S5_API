using TD1.Models;
using TD1.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TD1.Models.DataManager
{
    public class TypeProduitManager : IDataRepository<TypeProduit>
    {
        readonly ProduitsDBContext produitsDBContext;

        public TypeProduitManager() { }

        public TypeProduitManager(ProduitsDBContext context)
        {
            produitsDBContext = context;
        }

        public async Task<ActionResult<IEnumerable<TypeProduit>>> GetAllAsync()
        {
            return await produitsDBContext.TypeProduits.ToListAsync();
        }

        public async Task<ActionResult<TypeProduit>> GetByIdAsync(int id)
        {
            return await produitsDBContext.TypeProduits.FirstOrDefaultAsync(p => p.IdTypeProduit == id);
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
