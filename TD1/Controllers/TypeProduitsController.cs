using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TD1.Models;
using TD1.Models.DTO;
using TD1.Models.Repository;

namespace TD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeProduitsController : ControllerBase
    {
        //private readonly ProduitsDBContext _context;

        private readonly IDataRepository<TypeProduit, TypeProduitDto, TypeProduitDetailDto> dataRepository;
        private readonly IMapper _mapper;

        public TypeProduitsController(IDataRepository<TypeProduit, TypeProduitDto, TypeProduitDetailDto> dataRepo, IMapper mapper)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }

        // GET: api/TypeProduits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeProduitDto>>> GetTypeProduits()
        {
            var typeProduits = await dataRepository.GetAllAsync();
            return Ok(typeProduits.Value);
        }

        // GET: api/TypeProduits/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TypeProduitDetailDto>> GetTypeProduit(int id)
        {
            var typeProduit = await dataRepository.GetEntityDtoByIdAsync(id);

            if (typeProduit == null)
            {
                return NotFound();
            }

            return Ok(typeProduit.Value);
        }

        // POST: api/TypeProduits
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

        // PUT: api/Marques/{id}
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
            var typToUpdate = await dataRepository.GetEntityByIdAsync(id);
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

        // DELETE: api/Marques/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeProduit(int id)
        {
            var typeProduit = await dataRepository.GetEntityByIdAsync(id);
            if (typeProduit == null)
            {
                return NotFound();
            }
            await dataRepository.DeleteAsync(typeProduit.Value);
            return NoContent();
        }

    }
}
