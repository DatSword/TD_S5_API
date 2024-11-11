using Microsoft.AspNetCore.Mvc;

namespace TD1.Models.Repository
{
    public interface IDataRepository<TEntity, TDto, TDetailDto>
    {
        Task<ActionResult<IEnumerable<TDto>>> GetAllAsync();
        Task<ActionResult<TEntity>> GetEntityByIdAsync(int id);
        Task<ActionResult<TDetailDto>> GetEntityDtoByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entityToUpdate, TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
