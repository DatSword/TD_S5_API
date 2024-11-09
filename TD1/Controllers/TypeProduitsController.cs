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

        private readonly IDataRepository<TypeProduit> dataRepository;
        private readonly IMapper _mapper;

        public TypeProduitsController(IDataRepository<TypeProduit> dataRepo, IMapper mapper)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }

        // GET: api/TypeProduits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeProduitDto>>> GetTypeProduits()
        {
            var typeProds = await dataRepository.GetAllAsync();

            var typeProdDtos = typeProds.Value.Select(_mapper.Map<TypeProduitDto>);

            return Ok(typeProdDtos);
        }

        // GET: api/TypeProduits/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TypeProduitDto>> GetTypeProduit(int id)
        {
            var typeProd = await dataRepository.GetByIdAsync(id);

            if (typeProd == null)
            {
                return NotFound();
            }

            var typeProdDto = _mapper.Map<TypeProduitDto>(typeProd.Value);

            return Ok(typeProdDto);
        }

        // POST: api/TypeProduits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeProduitDto>> PostMarque(TypeProduitDto typeProdDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var typeProd = _mapper.Map<TypeProduit>(typeProdDto);

            await dataRepository.AddAsync(typeProd);

            var typeProdDtoResult = _mapper.Map<TypeProduitDto>(typeProd);

            return CreatedAtAction("GetById", new { id = typeProd.IdTypeProduit }, typeProdDtoResult);
        }

        // PUT: api/Marques/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TypeProduitDto>> PutTypeProduit(int id, TypeProduitDto typeprodDto)
        {
            if (id != typeprodDto.IdTypeProduit)
            {
                return BadRequest();
            }

            var typeprodToUpdate = await dataRepository.GetByIdAsync(id);
            if (typeprodToUpdate == null)
            {
                return NotFound();
            }
            var updatedTypeProd = _mapper.Map<TypeProduit>(typeprodDto);

            await dataRepository.UpdateAsync(typeprodToUpdate.Value, updatedTypeProd);

            var updatedTypeProdDto = _mapper.Map<TypeProduitDto>(updatedTypeProd);
            return Ok(updatedTypeProdDto);
        }

        // DELETE: api/Marques/{id}
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
