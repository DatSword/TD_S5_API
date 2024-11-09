using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TD1.Models;
using TD1.Models.DTO;
using TD1.Models.Repository;

namespace TD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        //private readonly ProduitsDBContext _context;

        private readonly IDataRepository<Produit> dataRepository;
        private readonly IMapper _mapper;

        public ProduitsController(IDataRepository<Produit> dataRepo, IMapper mapper)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }

        // GET: api/Produits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProduitDto>>> GetProduits()
        {
            var produits = await dataRepository.GetAllAsync();

            var produitDtos = produits.Value.Select(_mapper.Map<ProduitDto>);

            return Ok(produitDtos);
        }

        // GET: api/Produits/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProduitDto>> GetProduit(int id)
        {
            var produit = await dataRepository.GetByIdAsync(id);

            if (produit == null)
            {
                return NotFound();
            }

            var produitDto = _mapper.Map<ProduitDto>(produit.Value);

            return Ok(produitDto);
        }

        // POST: api/Produits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProduitDto>> PostProduit(ProduitDto produitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produit = _mapper.Map<Produit>(produitDto);

            await dataRepository.AddAsync(produit);

            var produitDtoResult = _mapper.Map<ProduitDto>(produit);

            return CreatedAtAction("GetById", new { id = produit.IdProduit }, produitDtoResult);
        }

        // PUT: api/Produits/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProduitDto>> PutProduit(int id, ProduitDto produitDto)
        {
            if (id != produitDto.IdProduit)
            {
                return BadRequest();
            }

            var produitToUpdate = await dataRepository.GetByIdAsync(id);
            if (produitToUpdate == null)
            {
                return NotFound();
            }
            var updatedProduit = _mapper.Map<Produit>(produitDto);

            await dataRepository.UpdateAsync(produitToUpdate.Value, updatedProduit);

            var updatedProduitDto = _mapper.Map<ProduitDto>(updatedProduit);
            return Ok(updatedProduitDto);
        }

        // DELETE: api/Produits/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await dataRepository.GetByIdAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(produit.Value);
            return NoContent();
        }
    }
}
