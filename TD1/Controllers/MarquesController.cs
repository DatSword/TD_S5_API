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
    public class MarquesController : ControllerBase
    {
        //private readonly ProduitsDBContext _context;

        private readonly IDataRepository<Marque> dataRepository;

        public MarquesController(IDataRepository<Marque> dataRepo)
        {
            dataRepository = dataRepo;
        }

        // GET: api/Marques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marque>>> GetMarques()
        {
            return await dataRepository.GetAllAsync();
        }

        // GET: api/Marques/5
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Marque>> GetMarque(int id)
        {
            var marque = await dataRepository.GetByIdAsync(id);

            if (marque == null)
            {
                return NotFound();
            }

            return marque;
        }

        // PUT: api/Marques/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMarque(int id, Marque marque)
        {
            if (id != marque.IdMarque)
            {
                return BadRequest();
            }

            var marToUpdate = await dataRepository.GetByIdAsync(id);

            if (marToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(marToUpdate.Value, marque);
                return NoContent();
            }

        }

        // POST: api/Marques
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Marque>> PostMarque(Marque marque)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(marque);

            return CreatedAtAction("GetById", new { id = marque.IdMarque }, marque);
        }

        // DELETE: api/Marques/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarque(int id)
        {
            var marque = await dataRepository.GetByIdAsync(id);
            if (marque == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(marque.Value);
            return NoContent();
        }

    }
}
