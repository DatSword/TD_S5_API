using TD1.Models;
using TD1.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TD1.Models.DataManager
{
    public class MarqueManager : IDataRepository<Marque>
    {
        readonly ProduitsDBContext produitsDBContext;

        public MarqueManager() { }

        public MarqueManager(ProduitsDBContext context)
        {
            produitsDBContext = context;
        }

        public async Task<ActionResult<IEnumerable<Marque>>> GetAllAsync()
        {
            return await produitsDBContext.Marques.ToListAsync();
        }

        public async Task<ActionResult<Marque>> GetByIdAsync(int id)
        {
            return await produitsDBContext.Marques.FirstOrDefaultAsync(p => p.IdMarque == id);
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
