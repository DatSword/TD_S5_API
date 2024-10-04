using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1.Models;
using TD1.Models.Repository;

namespace TD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeProduitsController : ControllerBase
    {
        //private readonly ProduitsDBContext _context;

        private readonly IDataRepository<TypeProduit> dataRepository;

        public TypeProduitsController(IDataRepository<TypeProduit> dataRepo)
        {
            dataRepository = dataRepo;
        }

        // GET: api/Marques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeProduit>>> GetTypeProduits()
        {
            return await dataRepository.GetAllAsync();
        }

        // GET: api/Marques/5
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TypeProduit>> GetTypeProduit(int id)
        {
            var typeProduit = await dataRepository.GetByIdAsync(id);

            if (typeProduit == null)
            {
                return NotFound();
            }

            return typeProduit;
        }

        // PUT: api/Marques/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTypeProduit(int id, TypeProduit typeProduit)
        {
            if (id != typeProduit.IdTypeProduit)
            {
                return BadRequest();
            }

            var typToUpdate = await dataRepository.GetByIdAsync(id);

            if (typToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(typToUpdate.Value, typeProduit);
                return NoContent();
            }

        }

        // POST: api/Marques
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeProduit>> PostTypeProduit(TypeProduit typeProduit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(typeProduit);

            return CreatedAtAction("GetById", new { id = typeProduit.IdTypeProduit }, typeProduit);
        }

        // DELETE: api/Marques/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeProduit(int id)
        {
            var typeProduit = await dataRepository.GetByIdAsync(id);
            if (typeProduit == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(typeProduit.Value);
            return NoContent();
        }

    }
}
