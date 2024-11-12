using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        private readonly IDataRepository<Marque, MarqueDto, MarqueDetailDto> dataRepository;
        private readonly IMapper _mapper;

        public MarquesController(IDataRepository<Marque, MarqueDto, MarqueDetailDto> dataRepo, IMapper mapper)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }

        // GET: api/Marques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarqueDto>>> GetMarques()
        {
            var marques = await dataRepository.GetAllAsync();
            return Ok(marques.Value);
        }

        // GET: api/Marques/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MarqueDetailDto>> GetMarque(int id)
        {
            var marque = await dataRepository.GetEntityDtoByIdAsync(id);

            if (marque == null)
            {
                return NotFound();
            }

            return Ok(marque.Value);
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

        // PUT: api/Marques/{id}
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
            var marToUpdate = await dataRepository.GetEntityByIdAsync(id);
            if (marToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(marToUpdate.Value, marque);
                return Ok(marque);
            }
        }

        // DELETE: api/Marques/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarque(int id)
        {
            var marque = await dataRepository.GetEntityByIdAsync(id);
            if (marque == null)
            {
                return NotFound();
            }
            await dataRepository.DeleteAsync(marque.Value);
            return NoContent();
        }
    }
}
