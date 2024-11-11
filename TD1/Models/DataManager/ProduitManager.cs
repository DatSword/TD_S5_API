using TD1.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TD1.Models.DataManager
{
    public class ProduitManager : IDataRepository<Produit>
    {
        readonly ProduitsDBContext produitsDBContext;

        public ProduitManager() { }

        public ProduitManager(ProduitsDBContext context)
        {
            produitsDBContext = context;
        }

        public async Task<ActionResult<IEnumerable<Produit>>> GetAllAsync()
        {
            return await produitsDBContext.Produits.ToListAsync();
        }

        public async Task<ActionResult<Produit>> GetByIdAsync(int id)
        {
            return await produitsDBContext.Produits.FirstOrDefaultAsync(p => p.IdProduit == id);
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
