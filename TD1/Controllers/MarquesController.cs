using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD1.Models;
using TD1.Models.DTO;
using TD1.Models.Repository;

namespace TD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarquesController : ControllerBase
    {
        //private readonly ProduitsDBContext _context;

        private readonly IDataRepository<Marque> dataRepository;
        private readonly IMapper _mapper;

        public MarquesController(IDataRepository<Marque> dataRepo, IMapper mapper)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }

        // GET: api/Marques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marque>>> GetMarques()
        {
            var marques = await dataRepository.GetAllAsync();

            // Mapper les entités Marque en DTO
            var marqueDtos = _mapper.Map<IEnumerable<MarqueDto>>(marques);

            return Ok(marqueDtos);
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
        public async Task<ActionResult<Marque>> PostMarque(MarqueDto marqueDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //await dataRepository.AddAsync(marque);   

            var marque = _mapper.Map<Marque>(marqueDto);
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
